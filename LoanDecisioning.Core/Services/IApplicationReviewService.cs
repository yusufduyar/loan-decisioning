using System.Threading.Tasks;
using LoanDecisioning.Core.DTO;

namespace LoanDecisioning.Core.Services
{
    public interface IApplicationReviewService
    {
        Task<ApplicationResultDTO> MakeDecisionAsync(LoanApplicationDTO loanApplicationDTO);
    }
}