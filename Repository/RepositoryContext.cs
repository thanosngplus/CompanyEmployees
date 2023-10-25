﻿using Entities.Models;
using Microsoft.EntityFrameworkCore;


namespace Repository
{
    public class RepositoryContext : DbContext
    {
        public DbSet<Company>? Companies { get; set; }
        public DbSet<Employee>? Employees { get; set; }


        public RepositoryContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CompanyConfiguration());
            modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
        }
    }
}
