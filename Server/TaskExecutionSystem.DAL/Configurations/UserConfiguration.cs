using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskExecutionSystem.DAL.Entities.Identity;

namespace TaskExecutionSystem.DAL.Configurations
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasOne(user => user.Teacher)
                .WithOne(t => t.User)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            builder.HasOne(user => user.Student)
                .WithOne(s => s.User)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        }
    }
}
