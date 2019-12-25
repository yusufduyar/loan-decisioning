using System.Threading.Tasks;

namespace LoanDecisioning.Core.Services
{
    public class SmsSenderService : ISmsSenderService
    {
        //TODO:Sms service is not implemented
        public Task SendLoanResultSmsAsync(string message, string phoneNumber)
        {
            return Task.CompletedTask;
        }
    }
}