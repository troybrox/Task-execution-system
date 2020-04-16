using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskExecutionSystem.DAL.Entities.Studies;

namespace TaskExecutionSystem.DAL.Configurations
{
    internal class FacultyConfiguration : IEntityTypeConfiguration<Faculty>
    {
        public void Configure(EntityTypeBuilder<Faculty> builder)
        {
            builder.HasAlternateKey(f => f.Name);

            builder.Property(f => f.Name)
                .HasMaxLength(255)
                .IsRequired();

            builder.HasMany(f => f.Groups)
                .WithOne(g => g.Faculty)
                .HasForeignKey(g => g.FacultyId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            builder.HasMany(f => f.Departments)
                .WithOne(d => d.Faculty)
                .HasForeignKey(d => d.FacultyId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        }
    }
}
