using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using TaskExecutionSystem.DAL.Entities.Relations;
using TaskExecutionSystem.DAL.Entities.Task;

namespace TaskExecutionSystem.DAL.Configurations
{
    internal class TaskStudentItemConfiguration : IEntityTypeConfiguration<TaskStudentItem>
    {
        public void Configure(EntityTypeBuilder<TaskStudentItem> builder)
        {
            builder.HasOne(ts => ts.Task)
                .WithMany(t => t.TaskStudentItems)
                .HasForeignKey(ts => ts.TaskId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();

            //builder.HasOne(ts => ts.Student)
            //    .WithMany(s => s.TaskStudentItems)
            //    .HasForeignKey(ts => ts.StudentId)
            //    .OnDelete(DeleteBehavior.NoAction)
            //    .IsRequired();

            builder.Property(s => s.TaskId)
                .IsRequired();

            builder.Property(s => s.StudentId)
                .IsRequired();
        }
    }
}
