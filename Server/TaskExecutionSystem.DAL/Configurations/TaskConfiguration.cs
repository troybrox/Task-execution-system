using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskExecutionSystem.DAL.Entities.Task;

namespace TaskExecutionSystem.DAL.Configurations
{
    internal class TaskConfiguration : IEntityTypeConfiguration<TaskModel>
    {
        public void Configure(EntityTypeBuilder<TaskModel> builder)
        {
            builder.HasMany(t => t.Solutions)
                .WithOne(s => s.TaskModel)
                .HasForeignKey(s => s.TaskId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();

            builder.HasMany(t => t.TaskStudentItems)
                .WithOne(ts => ts.Task)
                .HasForeignKey(s => s.TaskId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();

            builder.HasOne(t => t.File)
                .WithOne(s => s.TaskModel)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            builder.Property(t => t.TypeId)
                .IsRequired();

            builder.Property(t => t.SubjectId)
                .IsRequired();

            builder.Property(t => t.TeacherId)
                .IsRequired();

            builder.Property(t => t.GroupId)
                .IsRequired();


            builder.Property(t => t.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(t => t.ContentText)
                .HasMaxLength(2000);

            builder.Property(t => t.BeginDate)
                .HasColumnType("DATETIME");

            builder.Property(t => t.FinishDate)
                .HasColumnType("DATETIME");

            builder.Property(t => t.UpdateDate)
                .HasColumnType("DATETIME");

            builder.Property(t => t.IsOpen)
                .HasDefaultValue(true)
                .IsRequired();

            //builder.Property(t => t.TimePercentage)
            //    .HasMaxLength(3)
            //    .HasDefaultValue(100)
            //    .IsRequired();
        }
    }
}
