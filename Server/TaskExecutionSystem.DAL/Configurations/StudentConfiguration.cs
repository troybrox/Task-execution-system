using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskExecutionSystem.DAL.Entities.Identity;

namespace TaskExecutionSystem.DAL.Configurations
{
    internal class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.HasMany(student => student.Solutions)
                .WithOne(s => s.Student)
                .HasForeignKey(s => s.StudentId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            builder.HasMany(s => s.TaskStudentItems)
                .WithOne(ts => ts.Student)
                .HasForeignKey(s => s.StudentId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();


            builder.Property(s => s.UserId)
                .IsRequired();

            builder.Property(s => s.GroupId)
                .IsRequired();


            builder.Property(s => s.Name)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(s => s.Surname)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(s => s.Patronymic)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(s => s.NotificationCounter)
                .HasDefaultValue(0)
                .IsRequired();
        }
    }
}
