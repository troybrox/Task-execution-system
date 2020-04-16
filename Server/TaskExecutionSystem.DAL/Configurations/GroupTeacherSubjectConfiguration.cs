﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskExecutionSystem.DAL.Entities.Relations;

namespace TaskExecutionSystem.DAL.Configurations
{
    internal class GroupTeacherSubjectConfiguration : IEntityTypeConfiguration<GroupTeacherSubjectItem>
    {
        public void Configure(EntityTypeBuilder<GroupTeacherSubjectItem> builder)
        {
            builder.HasOne(i => i.Group)
                .WithMany(g => g.GroupTeacherSubjectItems)
                .HasForeignKey(i => i.GroupId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            builder.HasOne(i => i.Teacher)
                .WithMany(t => t.GroupTeacherSubjectItems)
                .HasForeignKey(i => i.TeacherId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            builder.HasOne(i => i.Subject)
                .WithMany(s => s.GroupTeacherSubjectItems)
                .HasForeignKey(i => i.SubjectId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        }
    }
}
