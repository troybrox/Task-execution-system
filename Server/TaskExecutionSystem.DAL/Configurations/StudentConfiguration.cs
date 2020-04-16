using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskExecutionSystem.DAL.Entities.Identity;

namespace TaskExecutionSystem.DAL.Configurations
{
    internal class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.HasAlternateKey(s => s.UserId);

            builder.Property(s => s.Name)
                .HasMaxLength(50)
                .IsRequired();
            builder.Property(s => s.Surname)
                .HasMaxLength(50)
                .IsRequired();
            builder.Property(s => s.Patronymic)
                .HasMaxLength(50)
                .IsRequired();

            // FK ??
        }
    }
}
