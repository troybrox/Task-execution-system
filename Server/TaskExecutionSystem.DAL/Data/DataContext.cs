using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using TaskExecutionSystem.DAL.Entities.Identity;

namespace TaskExecutionSystem.DAL.Data
{
    public partial class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasAlternateKey(u => u.UserName);

            modelBuilder.Entity<IdentityUserRole<long>>()
                .HasKey(r => new { r.UserId, r.RoleId });

            modelBuilder.Entity<IdentityUserRole<long>>()
                .ToTable("UserRoles");

            modelBuilder.Entity<IdentityUserClaim<long>>()
                .ToTable("UserClaims");

            base.OnModelCreating(modelBuilder);
        }
    }

    public partial class DataContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<IdentityUserRole<long>> UserRoles { get; set; }

        public DbSet<IdentityUserClaim<long>> IdentityUserClaims { get; set; }

        public DbSet<IdentityRoleClaim<long>> IdentityRoleClaims { get; set; }
    }
}
