using System.Threading.Tasks;

namespace LoanDecisioning.Core.Services
{
    public class LoanScoreService : ILoanScoreService
    {
        public Task<int> GetLoanScoreByIdentityNumberAsync(long identityNumber)
        {
            if (identityNumber < 40000000000) return Task.FromResult(400);
            else if (identityNumber >= 40000000000 && identityNumber < 70000000000) return Task.FromResult(700);
            else return Task.FromResult(1200);
        }
    }
}