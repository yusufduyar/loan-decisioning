using System.Threading.Tasks;
using LoanDecisioning.API.Controllers;
using LoanDecisioning.Core.DTO;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;

namespace LoanDecisioning.API.Tests
{
    public class LoanApplicationControllerTests
    {
        private ApplicationReviewServiceFake applicationReviewServiceFake;
        private LoanApplicationController loanApplicationController;
        [SetUp]
        public void Setup()
        {            
            applicationReviewServiceFake = new ApplicationReviewServiceFake();
             loanApplicationController = new LoanApplicationController(applicationReviewServiceFake);
        }

        [Test]
        public async Task Post_ReturnsOkResult_WhenCalled()
        {
            var loanApplicationDTO = new LoanApplicationDTO
            {
                IdentityNumber = "95587479936",
                Name = "Yusuf",
                Surname = "Duyar",
                MonthlyIncome = 9500,
                PhoneNumber = "5521234578"
            };

            var result = await loanApplicationController.LoanApplication(loanApplicationDTO);
            var okResult = result as OkObjectResult;

            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
        }

        [Test]
        public async Task Post_ReturnsBadRequestResult_WhenIdentityNumberLengthNotEqualTo11()
        {
            var loanApplicationDTO = new LoanApplicationDTO
            {
                IdentityNumber = "2",
                Name = "Yusuf",
                Surname = "Duyar",
                MonthlyIncome = 9500,
                PhoneNumber = "5521234578"
            };

            var result = await loanApplicationController.LoanApplication(loanApplicationDTO);
            var okResult = result as BadRequestObjectResult;

            Assert.IsNotNull(okResult);
            Assert.AreEqual(400, okResult.StatusCode);
        }

        [Test]
        public async Task Post_ReturnsBadRequestResult_WhenNameIsEmpty()
        {
            var loanApplicationDTO = new LoanApplicationDTO
            {
                IdentityNumber = "2",
                Name = string.Empty,
                Surname = "Duyar",
                MonthlyIncome = 9500,
                PhoneNumber = "5521234578"
            };

            var result = await loanApplicationController.LoanApplication(loanApplicationDTO);
            var okResult = result as BadRequestObjectResult;

            Assert.IsNotNull(okResult);
            Assert.AreEqual(400, okResult.StatusCode);
        }

        [Test]
        public async Task Post_ReturnsBadRequestResult_WhenSurnameIsEmpty()
        {
            var loanApplicationDTO = new LoanApplicationDTO
            {
                IdentityNumber = "2",
                Name = "Yusuf",
                Surname = string.Empty,
                MonthlyIncome = 9500,
                PhoneNumber = "5521234578"
            };

            var result = await loanApplicationController.LoanApplication(loanApplicationDTO);
            var okResult = result as BadRequestObjectResult;

            Assert.IsNotNull(okResult);
            Assert.AreEqual(400, okResult.StatusCode);
        }

        [Test]
        public async Task Post_ReturnsBadRequestResult_WhenMonthlyIncomeIsNotGreaterThan0()
        {
            var loanApplicationDTO = new LoanApplicationDTO
            {
                IdentityNumber = "2",
                Name = "Yusuf",
                Surname = string.Empty,
                MonthlyIncome = 0,
                PhoneNumber = "5521234578"
            };

            var result = await loanApplicationController.LoanApplication(loanApplicationDTO);
            var okResult = result as BadRequestObjectResult;

            Assert.IsNotNull(okResult);
            Assert.AreEqual(400, okResult.StatusCode);
        }
    }
}