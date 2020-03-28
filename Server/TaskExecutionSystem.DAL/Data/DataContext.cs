using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using TaskExecutionSystem.DAL.Entities;

namespace TaskExecutionSystem.DAL.Data
{
    public class DataContext : IdentityDbContext
    {
        public DataContext(DbContextOptions options) : base(options) { }

        public class EFDBContextFactory : IDesignTimeDbContextFactory<DataContext>
        {
            DataContext IDesignTimeDbContextFactory<DataContext>.CreateDbContext(string[] args)
            {
                var optionsBuilder = new DbContextOptionsBuilder();
                optionsBuilder.UseSqlServer(@"Server = localhost\SQLEXPRESS; Database = TESystem; Trusted_Connection = True;");
                return new DataContext(optionsBuilder.Options);
            }
        }
    }
}
