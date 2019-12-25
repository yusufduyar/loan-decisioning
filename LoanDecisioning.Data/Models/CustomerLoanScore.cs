namespace LoanDecisioning.Data.Models
{
    public class CustomerLoanScore : BaseEntity
    {
        public int CustomerLoanScoreId { get; set; }
        public string CustomerIdentityNumber { get; set; }
        public int LoanScore { get; set; }
    }
}