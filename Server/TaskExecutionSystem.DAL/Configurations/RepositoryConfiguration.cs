using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskExecutionSystem.DAL.Entities.Repository;

namespace TaskExecutionSystem.DAL.Configurations
{
    internal class RepositoryConfiguration : IEntityTypeConfiguration<RepositoryModel>
    {
        public void Configure(EntityTypeBuilder<RepositoryModel> builder)
        {
            builder.HasMany(repo => repo.Files)
                .WithOne(f => f.RepositoryModel)
                .HasForeignKey(f => f.RepositoryModelId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        }
    }
}
