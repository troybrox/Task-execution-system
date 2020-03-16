using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using TaskExecutionSystem.DAL;

namespace TaskExecutionSystem.DAL.Data
{
    public class TaskDBContext : DbContext
    {
        public TaskDBContext(DbContextOptions options) : base(options) { }

        public class EFDBContextFactory : IDesignTimeDbContextFactory<TaskDBContext>
        {
            TaskDBContext IDesignTimeDbContextFactory<TaskDBContext>.CreateDbContext(string[] args)
            {
                var optionsBuilder = new DbContextOptionsBuilder();
                optionsBuilder.UseSqlServer(@"Server = localhost\SQLEXPRESS; Database = TESystem; Trusted_Connection = True;");
                return new TaskDBContext(optionsBuilder.Options);
            }
        }
    }
}
