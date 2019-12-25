using System.Threading.Tasks;

namespace LoanDecisioning.Core.Services
{
    public interface ISmsSenderService
    {
        Task SendLoanResultSmsAsync(string message, string phoneNumber);
    }
}