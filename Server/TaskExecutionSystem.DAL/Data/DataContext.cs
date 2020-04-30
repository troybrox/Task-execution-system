using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using TaskExecutionSystem.DAL.Configurations;
using TaskExecutionSystem.DAL.Entities.Identity;
using TaskExecutionSystem.DAL.Entities.Elements;
using TaskExecutionSystem.DAL.Entities.Registration;
using TaskExecutionSystem.DAL.Entities.Studies;
using TaskExecutionSystem.DAL.Entities.Relations;
using TaskExecutionSystem.DAL.Entities.Task;
using TaskExecutionSystem.DAL.Entities.Repository;
using TaskExecutionSystem.DAL.Entities.File;

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

        public DbSet<TaskModel> TaskModels { get; set; }
        public DbSet<Solution> Solutions { get; set; }
        public DbSet<TypeOfTask> TaskTypes { get; set; }

        public DbSet<RepositoryModel> RepositoryModels { get; set; }
        public DbSet<Theme> Themes { get; set; }
        public DbSet<Paragraph> Paragraphs { get; set; }

        public DbSet<TaskFile> TaskFiles { get; set; }
        public DbSet<SolutionFile> SolutionFiles { get; set; }
        public DbSet<RepoFile> RepoFiles { get; set; }

        public DbSet<TaskStudentItem> TaskStudentItems { get; set; }

        // add Task, Repo configs => relations [!]
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

            modelBuilder.ApplyConfiguration(new TaskTypeConfiguration());
            modelBuilder.ApplyConfiguration(new TaskConfiguration());
            modelBuilder.ApplyConfiguration(new TaskFileConfiguration());
            modelBuilder.ApplyConfiguration(new SolutionFileConfiguration());

            modelBuilder.ApplyConfiguration(new GroupTeacherSubjectConfiguration());
            modelBuilder.ApplyConfiguration(new TaskStudentItemConfiguration());

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
