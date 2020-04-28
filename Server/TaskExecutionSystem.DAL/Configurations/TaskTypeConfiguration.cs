using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskExecutionSystem.DAL.Entities.Task;

namespace TaskExecutionSystem.DAL.Configurations
{
    internal class TaskTypeConfiguration : IEntityTypeConfiguration<TypeOfTask>
    {
        public void Configure(EntityTypeBuilder<TypeOfTask> builder)
        {
            builder.HasMany(type => type.Tasks)
                .WithOne(t => t.Type)
                .HasForeignKey(t => t.TypeId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            builder.Property(t => t.Name)
                .HasMaxLength(200);
        }
    }
}
