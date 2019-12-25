namespace LoanDecisioning.Core.DTO
{
    public class ApplicationResultDTO
    {
        public bool IsApproved { get; set; }
        public decimal LoanLimit { get; set; }
    }
}