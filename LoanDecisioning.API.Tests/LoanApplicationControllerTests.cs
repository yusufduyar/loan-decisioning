using System.Threading.Tasks;
using LoanDecisioning.API.Controllers;
using LoanDecisioning.Core.DTO;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;

namespace LoanDecisioning.API.Tests
{
    public class LoanApplicationControllerTests
    {
        [Test]
        public async Task Post_WhenCalled_ReturnsOkResult()
        {
            var applicationReviewServiceFake = new ApplicationReviewServiceFake();
            var loanApplicationController = new LoanApplicationController(applicationReviewServiceFake);

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
    }
}