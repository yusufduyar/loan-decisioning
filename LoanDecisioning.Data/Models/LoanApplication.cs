using System;

namespace LoanDecisioning.Data.Models
{
    public class LoanApplication : BaseEntity
    {
        public int LoanApplicationId { get; set; }
        public bool IsApproved { get; set; }
        public decimal LoanLimit { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}