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

            builder.Property(t => t.Name)
                .HasMaxLength(50)
                .IsRequired();
            builder.Property(t => t.Surname)
                .HasMaxLength(50)
                .IsRequired();
            builder.Property(t => t.Patronymic)
                .HasMaxLength(50)
                .IsRequired();

            builder.HasMany(t => t.GroupTeacherSubjectItems)
                .WithOne(gts => gts.Teacher)
                .HasForeignKey(gts => gts.TeacherId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        }
    }
}
