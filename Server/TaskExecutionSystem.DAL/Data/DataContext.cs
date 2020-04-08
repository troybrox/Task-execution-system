using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using TaskExecutionSystem.DAL.Entities;
using TaskExecutionSystem.DAL.Entities.Identity;

namespace TaskExecutionSystem.DAL.Data
{
    public class DataContext : IdentityDbContext<User, Role, long>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>()
                .HasAlternateKey(u => u.UserName);
        }

        public class EFDBContextFactory : IDesignTimeDbContextFactory<DataContext>
        {
            DataContext IDesignTimeDbContextFactory<DataContext>.CreateDbContext(string[] args)
            {
                var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
                optionsBuilder.UseSqlServer(@"Server = localhost\SQLEXPRESS; Database = TaskExecutionSystem; Trusted_Connection = True;");
                return new DataContext(optionsBuilder.Options);
            }
        }
    }

    //public partial class DataContext
    //{
    //    public DbSet<StudentUser> StudentUsers { get; set; }

    //    public DbSet<TeacherUser> TeacherUsers { get; set; }

    //    public DbSet<Role> Roles { get; set; }

    //    public DbSet<IdentityUserRole<long>> UserRoles { get; set; }

    //    public DbSet<IdentityUserClaim<long>> IdentityUserClaims { get; set; }

    //    public DbSet<IdentityRoleClaim<long>> IdentityRoleClaims { get; set; }
    //}
}
