using System.Threading.Tasks;
using LoanDecisioning.Core.DTO;
using LoanDecisioning.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace LoanDecisioning.API.Controllers
{

    [Route("v1")]
    [ApiController]
    public class LoanApplicationController : ControllerBase
    {
        private readonly IApplicationReviewService applicationReviewService;

        public LoanApplicationController(IApplicationReviewService applicationReviewService)
        {
            this.applicationReviewService = applicationReviewService;
        }

        [Route("loanapplication")]
        [HttpPost]
        public async Task<IActionResult> LoanApplication([FromBody]LoanApplicationDTO loanApplicaton)
        {
            if (string.IsNullOrWhiteSpace(loanApplicaton.IdentityNumber)) return BadRequest("Identity number should not be null or empty");
            if (loanApplicaton.IdentityNumber.Length != 11) return BadRequest("Identity number length should be equal to 11");
            if (loanApplicaton.MonthlyIncome <= 0) return BadRequest("Monthly income should be greater than 0");
            if (string.IsNullOrWhiteSpace(loanApplicaton.Name)) return BadRequest("Name should not be null or empty");
            if (string.IsNullOrWhiteSpace(loanApplicaton.Surname)) return BadRequest("Surname should not be null or empty");

            var applicationResult = await applicationReviewService.MakeDecisionAsync(loanApplicaton);

            return Ok(applicationResult);
        }
    }
}