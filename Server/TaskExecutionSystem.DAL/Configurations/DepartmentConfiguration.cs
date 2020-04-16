using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskExecutionSystem.DAL.Entities.Studies;

namespace TaskExecutionSystem.DAL.Configurations
{
    internal class DepartmentConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.HasAlternateKey(d => d.Name);

            builder.Property(d => d.Name)
                .HasMaxLength(255)
                .IsRequired();

            builder.HasMany(d => d.Teachers)
                .WithOne(t => t.Department)
                .HasForeignKey(t => t.DepartmentId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        }
    }
}
