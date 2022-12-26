using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SoccerManager.Domain.Entities;

namespace SoccerManager.Infra.Data.EntityConfig
{
    // Teams
    public class TeamConfiguration : IEntityTypeConfiguration<TeamEntity>
    {
        public void Configure(EntityTypeBuilder<TeamEntity> builder)
        {
            builder.ToTable("Teams", "dbo");
            builder.HasKey(x => x.TeamId).HasName("PK_Teams").IsClustered();

            builder.Property(x => x.TeamId).HasColumnName(@"TeamId").HasColumnType("bigint").IsRequired().ValueGeneratedNever();
            builder.Property(x => x.TeamName).HasColumnName(@"TeamName").HasColumnType("nvarchar(50)").IsRequired().HasMaxLength(50);
            builder.Property(x => x.TeamCountry).HasColumnName(@"TeamCountry").HasColumnType("nvarchar(50)").IsRequired().HasMaxLength(50);
            builder.Property(x => x.Budget).HasColumnName(@"Budget").HasColumnType("money").IsRequired();

            // Foreign keys
            builder.HasOne(a => a.User).WithOne(b => b.Team).HasForeignKey<TeamEntity>(c => c.TeamId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_Teams_Users");
        }
    }
}
