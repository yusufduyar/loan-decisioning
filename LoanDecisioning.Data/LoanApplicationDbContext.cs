using System;
using LoanDecisioning.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace LoanDecisioning.Data
{
    public class LoanApplicationDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerLoanScore> CustomerLoanScores { get; set; }
        public DbSet<LoanApplication> LoanApplications { get; set; }

        public LoanApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // modelBuilder.Entity<Customer>().Property(c => c.CreatedAt).HasDefaultValue(DateTime.UtcNow);

            // modelBuilder.Entity<Customer>().Property(c => c.IsDeleted).HasDefaultValue(false);

            // modelBuilder.Entity<CustomerLoanScore>().Property(c => c.CreatedAt).HasDefaultValue(DateTime.UtcNow);

            // modelBuilder.Entity<CustomerLoanScore>().Property(c => c.IsDeleted).HasDefaultValue(false);

            // modelBuilder.Entity<LoanApplication>().Property(c => c.CreatedAt).HasDefaultValue(DateTime.UtcNow);

            // modelBuilder.Entity<LoanApplication>().Property(c => c.IsDeleted).HasDefaultValue(false);

        }
    }
}