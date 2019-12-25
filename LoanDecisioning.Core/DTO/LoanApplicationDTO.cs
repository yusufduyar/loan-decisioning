namespace LoanDecisioning.Core.DTO
{
    public class LoanApplicationDTO
    {
        public string IdentityNumber { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public decimal MonthlyIncome { get; set; }
        public string PhoneNumber { get; set; }
    }
}