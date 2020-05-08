using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskExecutionSystem.DAL.Entities.Task;

namespace TaskExecutionSystem.DAL.Configurations
{
    internal class SolutionConfiguration : IEntityTypeConfiguration<Solution>
    {
        public void Configure(EntityTypeBuilder<Solution> builder)
        {
            builder.Property(s => s.TaskId)
                .IsRequired();

            builder.Property(s => s.StudentId)
                .IsRequired();


            builder.Property(s => s.ContentText)
                .HasMaxLength(2000);

            builder.Property(t => t.CreationDate)
                .HasColumnType("DATETIME");

            builder.Property(s => s.InTime)
                .HasDefaultValue(true);
        }
    }
}
