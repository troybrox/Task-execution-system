using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskExecutionSystem.DAL.Entities.Studies;

namespace TaskExecutionSystem.DAL.Configurations
{
    internal class GroupConfiguration : IEntityTypeConfiguration<Group>
    {
        public void Configure(EntityTypeBuilder<Group> builder)
        {
            builder.HasAlternateKey(g => g.NumberName);

            builder.Property(g => g.NumberName)
                .HasMaxLength(12)
                .IsRequired();

            builder.HasMany(g => g.Students)
                .WithOne(s => s.Group)
                .HasForeignKey(s => s.GroupId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            builder.HasMany(g => g.GroupTeacherSubjectItems)
                .WithOne(gts => gts.Group)
                .HasForeignKey(gts => gts.GroupId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        }
    }
}
