using NUnit.Framework;
using LoanDecisioning.Core.Services;
using Moq;
using LoanDecisioning.Data;
using Microsoft.EntityFrameworkCore;
using System;
using LoanDecisioning.Data.Models;
using LoanDecisioning.Core.DTO;
using System.Threading.Tasks;
using System.Linq;

namespace LoanDecisioning.Core.Tests
{
    public class DecisioningServiceTests
    {
        private DbContextOptions<LoanApplicationDbContext> dbOptions;
        private LoanApplicationDbContext mockDbContext;
        private Mock<ISmsSenderService> mockSmsSenderService;

        [SetUp]
        public void Setup()
        {
            dbOptions = new DbContextOptionsBuilder<LoanApplicationDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            mockDbContext = new LoanApplicationDbContext(dbOptions);

            mockSmsSenderService = new Mock<ISmsSenderService>();
            mockSmsSenderService.Setup(s => s.SendLoanResultSmsAsync(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);
        }

        [Test]
        public async Task MakeDecision_RejectsApplication_WhenLoanScoreUnder500()
        {
            var mockLoanScoreService = new Mock<ILoanScoreService>();
            mockLoanScoreService.Setup(s => s.GetLoanScoreByIdentityNumberAsync(It.IsAny<long>())).Returns(Task.FromResult(150));

            var loanApplicationDTO = new LoanApplicationDTO
            {
                IdentityNumber = "95587479936",
                Name = "Yusuf",
                Surname = "Duyar",
                MonthlyIncome = 9500,
                PhoneNumber = "5521234578"
            };

            var applicationReviewService = new ApplicationReviewService(mockDbContext, mockLoanScoreService.Object, mockSmsSenderService.Object);
            var applicationResult = await applicationReviewService.MakeDecisionAsync(loanApplicationDTO);

            Assert.AreEqual(false, applicationResult.IsApproved);
            Assert.AreEqual(0, applicationResult.LoanLimit);
        }

        [Test]
        public async Task MakeDecision_ApprovesApplicationAndSetsLoanLimitTo10000_WhenLoanScoreBetween500And1000AndMonthlyIncomeUnder5000()
        {
            var mockLoanScoreService = new Mock<ILoanScoreService>();
            mockLoanScoreService.Setup(s => s.GetLoanScoreByIdentityNumberAsync(It.IsAny<long>())).Returns(Task.FromResult(750));

            var loanApplicationDTO = new LoanApplicationDTO
            {
                IdentityNumber = "95587479936",
                Name = "Yusuf",
                Surname = "Duyar",
                MonthlyIncome = 4000,
                PhoneNumber = "5521234578"
            };

            var applicationReviewService = new ApplicationReviewService(mockDbContext, mockLoanScoreService.Object, mockSmsSenderService.Object);
            var applicationResult = await applicationReviewService.MakeDecisionAsync(loanApplicationDTO);

            Assert.AreEqual(true, applicationResult.IsApproved);
            Assert.AreEqual(10000, applicationResult.LoanLimit);
        }

        [Test]
        public async Task MakeDecision_ApprovesApplicationAndSetsLoanLimitTo4TimesMonthlyIncome_WhenLoanScoreBetween500And1000AndMonthlyIncomeOver5000()
        {
            var mockLoanScoreService = new Mock<ILoanScoreService>();
            mockLoanScoreService.Setup(s => s.GetLoanScoreByIdentityNumberAsync(It.IsAny<long>())).Returns(Task.FromResult(750));

            var loanApplicationDTO = new LoanApplicationDTO
            {
                IdentityNumber = "95587479936",
                Name = "Yusuf",
                Surname = "Duyar",
                MonthlyIncome = 7000,
                PhoneNumber = "5521234578"
            };

            var applicationReviewService = new ApplicationReviewService(mockDbContext, mockLoanScoreService.Object, mockSmsSenderService.Object);
            var applicationResult = await applicationReviewService.MakeDecisionAsync(loanApplicationDTO);

            Assert.AreEqual(true, applicationResult.IsApproved);
            Assert.AreEqual(28000, applicationResult.LoanLimit);
        }

        [Test]
        public async Task MakeDecision_ApprovesApplicationAndSetsLoanLimitTo4TimesMonthlyIncome_WhenLoanScoreOver1000()
        {
            var mockLoanScoreService = new Mock<ILoanScoreService>();
            mockLoanScoreService.Setup(s => s.GetLoanScoreByIdentityNumberAsync(It.IsAny<long>())).Returns(Task.FromResult(1000));

            var loanApplicationDTO = new LoanApplicationDTO
            {
                IdentityNumber = "95587479936",
                Name = "Yusuf",
                Surname = "Duyar",
                MonthlyIncome = 6000,
                PhoneNumber = "5521234578"
            };

            var applicationReviewService = new ApplicationReviewService(mockDbContext, mockLoanScoreService.Object, mockSmsSenderService.Object);
            var applicationResult = await applicationReviewService.MakeDecisionAsync(loanApplicationDTO);

            Assert.AreEqual(true, applicationResult.IsApproved);
            Assert.AreEqual(24000, applicationResult.LoanLimit);
        }

        [Test]
        public async Task MakeDecision_SavesCustomerAndLoanApplication_WhenApplicationApproved()
        {
            var mockLoanScoreService = new Mock<ILoanScoreService>();
            mockLoanScoreService.Setup(s => s.GetLoanScoreByIdentityNumberAsync(It.IsAny<long>())).Returns(Task.FromResult(750));

            var loanApplicationDTO = new LoanApplicationDTO
            {
                IdentityNumber = "95587479936",
                Name = "Yusuf",
                Surname = "Duyar",
                MonthlyIncome = 4000,
                PhoneNumber = "5521234578"
            };

            var applicationReviewService = new ApplicationReviewService(mockDbContext, mockLoanScoreService.Object, mockSmsSenderService.Object);
            var applicationResult = await applicationReviewService.MakeDecisionAsync(loanApplicationDTO);

            var customers = await mockDbContext.Customers.Include(c => c.LoanApplications).ToListAsync();

            Assert.AreEqual(true, applicationResult.IsApproved);
            Assert.AreEqual(1, customers.Count);
            Assert.AreEqual(1, customers.First().LoanApplications.Count);
        }

        [Test]
        public async Task MakeDecision_AddsLoanApplicationToCustomer_WhenApplicationApprovedAndCustomerAlreadyExists()
        {
            var mockLoanScoreService = new Mock<ILoanScoreService>();
            mockLoanScoreService.Setup(s => s.GetLoanScoreByIdentityNumberAsync(It.IsAny<long>())).Returns(Task.FromResult(750));

            var loanApplicationDTO = new LoanApplicationDTO
            {
                IdentityNumber = "95587479936",
                Name = "Yusuf",
                Surname = "Duyar",
                MonthlyIncome = 4000,
                PhoneNumber = "5521234578"
            };

            var applicationReviewService = new ApplicationReviewService(mockDbContext, mockLoanScoreService.Object, mockSmsSenderService.Object);
            var applicationResult = await applicationReviewService.MakeDecisionAsync(loanApplicationDTO);

            var secondApplicationResult = await applicationReviewService.MakeDecisionAsync(loanApplicationDTO);

            var customers = await mockDbContext.Customers.Include(c => c.LoanApplications).ToListAsync();

            Assert.AreEqual(true, applicationResult.IsApproved);
            Assert.AreEqual(1, customers.Count);
            Assert.AreEqual(2, customers.First().LoanApplications.Count);
        }

    }
}