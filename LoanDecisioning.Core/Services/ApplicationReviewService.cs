using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LoanDecisioning.Core.DTO;
using LoanDecisioning.Data;
using LoanDecisioning.Data.Models;

namespace LoanDecisioning.Core.Services
{
    public class ApplicationReviewService : IApplicationReviewService
    {
        private readonly int LOAN_LIMIT_MULTIPLIER = 4;
        private readonly LoanApplicationDbContext dbContext;
        private readonly ILoanScoreService loanScoreService;
        private readonly ISmsSenderService smsSenderService;

        public ApplicationReviewService(LoanApplicationDbContext dbContext, ILoanScoreService loanScoreService, ISmsSenderService smsSenderService)
        {
            this.dbContext = dbContext;
            this.loanScoreService = loanScoreService;
            this.smsSenderService = smsSenderService;
        }
        public async Task<ApplicationResultDTO> MakeDecisionAsync(LoanApplicationDTO loanApplicationDTO)
        {
            var isIdentityLong = long.TryParse(loanApplicationDTO.IdentityNumber, out long identityAsLong);

            if (!isIdentityLong || identityAsLong < 0) throw new ArgumentException("Identity Number is not valid");

            var loanScore = await loanScoreService.GetLoanScoreByIdentityNumberAsync(identityAsLong);

            ApplicationResultDTO applicationResultDTO = CreateApplicationResult(loanApplicationDTO, loanScore);

            await SaveCustomerAndApplication(loanApplicationDTO, applicationResultDTO);

            await smsSenderService.SendLoanResultSmsAsync("Message", loanApplicationDTO.PhoneNumber);

            return applicationResultDTO;
        }

        private async Task SaveCustomerAndApplication(LoanApplicationDTO loanApplicationDTO, ApplicationResultDTO applicationResultDTO)
        {
            var customer = dbContext.Customers.Where(c => c.IdentityNumber == loanApplicationDTO.IdentityNumber).SingleOrDefault();
            var newLoanApplication = new LoanApplication { IsApproved = applicationResultDTO.IsApproved, LoanLimit = applicationResultDTO.LoanLimit };

            if (customer == default(Customer))
            {
                customer = new Customer
                {
                    IdentityNumber = loanApplicationDTO.IdentityNumber,
                    Name = loanApplicationDTO.Name,
                    Surname = loanApplicationDTO.Surname,
                    MonthlyIncome = loanApplicationDTO.MonthlyIncome,
                    LoanApplications = new List<LoanApplication>
                    {
                        newLoanApplication
                    }
                };
                dbContext.Customers.Add(customer);
            }
            else
            {
                customer.LoanApplications.Add(newLoanApplication);
            }
            await dbContext.SaveChangesAsync();
        }

        private ApplicationResultDTO CreateApplicationResult(LoanApplicationDTO loanApplicationDTO, int loanScore)
        {
            var applicationResultDTO = new ApplicationResultDTO();
            if (loanScore < 500)
            {
                applicationResultDTO.IsApproved = false;
                applicationResultDTO.LoanLimit = 0;
            }
            else if (loanScore >= 500 && loanScore < 1000)
            {
                applicationResultDTO.IsApproved = true;
                applicationResultDTO.LoanLimit = loanApplicationDTO.MonthlyIncome < 5000 ? 10000 : loanApplicationDTO.MonthlyIncome * LOAN_LIMIT_MULTIPLIER;
            }
            else
            {
                applicationResultDTO.IsApproved = true;
                applicationResultDTO.LoanLimit = loanApplicationDTO.MonthlyIncome * LOAN_LIMIT_MULTIPLIER;
            }

            return applicationResultDTO;
        }
    }
}