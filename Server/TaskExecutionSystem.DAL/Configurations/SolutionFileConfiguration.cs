using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using TaskExecutionSystem.DAL.Entities.File;
using TaskExecutionSystem.DAL.Entities.Relations;
using TaskExecutionSystem.DAL.Entities.Task;

namespace TaskExecutionSystem.DAL.Configurations 
{
    internal class SolutionFileConfiguration : IEntityTypeConfiguration<SolutionFile>
    {
        public void Configure(EntityTypeBuilder<SolutionFile> builder)
        {
            builder.Property(f => f.SolutionId)
                .IsRequired();
        }
    }
}
