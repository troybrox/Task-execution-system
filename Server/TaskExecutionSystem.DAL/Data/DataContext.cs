using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using TaskExecutionSystem.DAL.Configurations;
using TaskExecutionSystem.DAL.Entities.Identity;
using TaskExecutionSystem.DAL.Entities.Elements;
using TaskExecutionSystem.DAL.Entities.Registration;
using TaskExecutionSystem.DAL.Entities.Studies;
using TaskExecutionSystem.DAL.Entities.Relations;

namespace TaskExecutionSystem.DAL.Data
{
    public partial class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<TeacherRegisterRequest> TeacherRegisterRequests { get; set; }
        public DbSet<StudentRegisterRequest> StudentRegisterRequests { get; set; }
        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<GroupTeacherSubjectItem> GroupTeacherSubjectItems { get; set; }

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

            modelBuilder.ApplyConfiguration(new FacultyConfiguration());
            modelBuilder.ApplyConfiguration(new GroupConfiguration());
            modelBuilder.ApplyConfiguration(new DepartmentConfiguration());
            modelBuilder.ApplyConfiguration(new SubjectConfiguration());
            modelBuilder.ApplyConfiguration(new TeacherConfiguration());
            modelBuilder.ApplyConfiguration(new StudentConfiguration());
            modelBuilder.ApplyConfiguration(new GroupTeacherSubjectConfiguration());

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
