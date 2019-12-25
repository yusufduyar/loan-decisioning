using System;

namespace LoanDecisioning.Data.Models
{
    public class BaseEntity
    {
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}