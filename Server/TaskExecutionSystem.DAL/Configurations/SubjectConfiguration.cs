﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskExecutionSystem.DAL.Entities.Studies;

namespace TaskExecutionSystem.DAL.Configurations
{
    internal class SubjectConfiguration : IEntityTypeConfiguration<Subject>
    {
        public void Configure(EntityTypeBuilder<Subject> builder)
        {
            builder.HasAlternateKey(s => s.Name);

            builder.Property(s => s.Name)
                .HasMaxLength(255)
                .IsRequired();

            builder.HasMany(s => s.GroupTeacherSubjectItems)
                .WithOne(gts => gts.Subject)
                .HasForeignKey(gts => gts.SubjectId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            builder.HasMany(s => s.Tasks)
                .WithOne(t => t.Subject)
                .HasForeignKey(t => t.SubjectId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            builder.HasMany(s => s.Repositories)
               .WithOne(r => r.Subject)
               .HasForeignKey(r => r.SubjectId)
               .OnDelete(DeleteBehavior.Cascade)
               .IsRequired();
        }
    }
}
