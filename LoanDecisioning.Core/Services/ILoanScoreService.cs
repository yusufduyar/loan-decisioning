using System.Threading.Tasks;

namespace LoanDecisioning.Core.Services
{
    public interface ILoanScoreService
    {
        Task<int> GetLoanScoreByIdentityNumberAsync(long identityNumber);
    }
}