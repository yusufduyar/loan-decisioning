using System;
using System.Collections.Generic;

namespace LoanDecisioning.Data.Models
{
    public class Customer : BaseEntity
    {
        public int CustomerId { get; set; }
        public string IdentityNumber { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public decimal MonthlyIncome { get; set; }
        public string PhoneNumber { get; set; }

        public List<LoanApplication> LoanApplications { get; set; }
    }
}