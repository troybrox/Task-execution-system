using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskExecutionSystem.DAL.Entities.Identity;

namespace TaskExecutionSystem.DAL.Configurations
{
    internal class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
    {
        public void Configure(EntityTypeBuilder<Teacher> builder)
        {
            builder.HasAlternateKey(t => t.UserId);

            builder.HasMany(t => t.GroupTeacherSubjectItems)
                .WithOne(gts => gts.Teacher)
                .HasForeignKey(gts => gts.TeacherId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            builder.HasMany(teacher => teacher.Tasks)
                .WithOne(task => task.Teacher)
                .HasForeignKey(task => task.TeacherId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();

            builder.Property(t => t.Name)
                .HasMaxLength(50)
                .IsRequired();
            builder.Property(t => t.Surname)
                .HasMaxLength(50)
                .IsRequired();
            builder.Property(t => t.Patronymic)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(t => t.DepartmentId)
                .IsRequired();

            builder.Property(t => t.UserId)
                .IsRequired();

            builder.Property(t => t.NotificationCounter)
                .HasDefaultValue(0);
        }
    }
}
