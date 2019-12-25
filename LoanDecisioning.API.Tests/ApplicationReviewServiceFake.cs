using System.Threading.Tasks;
using LoanDecisioning.Core.DTO;
using LoanDecisioning.Core.Services;

namespace LoanDecisioning.API.Tests
{
    public class ApplicationReviewServiceFake : IApplicationReviewService
    {
        public Task<ApplicationResultDTO> MakeDecisionAsync(LoanApplicationDTO loanApplicationDTO)
        {
            return Task.FromResult(new ApplicationResultDTO
            {
                IsApproved = true,
                LoanLimit = 10000
            });
        }
    }
}