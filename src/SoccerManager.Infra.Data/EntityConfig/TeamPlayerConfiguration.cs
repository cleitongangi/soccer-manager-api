using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SoccerManager.Domain.Entities;

namespace SoccerManager.Infra.Data.EntityConfig
{
    // TeamPlayers
    public class TeamPlayerConfiguration : IEntityTypeConfiguration<TeamPlayerEntity>
    {
        public void Configure(EntityTypeBuilder<TeamPlayerEntity> builder)
        {
            builder.ToTable("TeamPlayers", "dbo");
            builder.HasKey(x => new { x.TeamId, x.PlayerId, x.Sequence }).HasName("PK_TeamPlayers").IsClustered();

            builder.Property(x => x.TeamId).HasColumnName(@"TeamId").HasColumnType("bigint").IsRequired().ValueGeneratedNever();
            builder.Property(x => x.PlayerId).HasColumnName(@"PlayerId").HasColumnType("bigint").IsRequired().ValueGeneratedNever();
            builder.Property(x => x.Sequence).HasColumnName(@"Sequence").HasColumnType("int").IsRequired().ValueGeneratedNever();
            builder.Property(x => x.CreatedAt).HasColumnName(@"CreatedAt").HasColumnType("datetime").IsRequired();
            builder.Property(x => x.RemovedAt).HasColumnName(@"RemovedAt").HasColumnType("datetime").IsRequired(false);
            builder.Property(x => x.Active).HasColumnName(@"Active").HasColumnType("bit").IsRequired();

            // Foreign keys
            builder.HasOne(a => a.Player).WithMany(b => b.TeamPlayers).HasForeignKey(c => c.PlayerId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_TeamPlayers_Players");
            builder.HasOne(a => a.Team).WithMany(b => b.TeamPlayers).HasForeignKey(c => c.TeamId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_TeamPlayers_Teams");
        }
    }
}
