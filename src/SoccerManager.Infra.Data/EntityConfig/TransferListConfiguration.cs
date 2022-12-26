using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SoccerManager.Domain.Entities;

namespace SoccerManager.Infra.Data.EntityConfig
{
    // TransferList
    public class TransferListConfiguration : IEntityTypeConfiguration<TransferListEntity>
    {
        public void Configure(EntityTypeBuilder<TransferListEntity> builder)
        {
            builder.ToTable("TransferList", "dbo");
            builder.HasKey(x => x.Id).HasName("PK_TransferList").IsClustered();

            builder.Property(x => x.Id).HasColumnName(@"Id").HasColumnType("bigint").IsRequired().ValueGeneratedOnAdd().UseIdentityColumn();
            builder.Property(x => x.PlayerId).HasColumnName(@"PlayerId").HasColumnType("bigint").IsRequired();
            builder.Property(x => x.CreatedAt).HasColumnName(@"CreatedAt").HasColumnType("datetime").IsRequired();
            builder.Property(x => x.SourceTeamId).HasColumnName(@"SourceTeamId").HasColumnType("bigint").IsRequired();
            builder.Property(x => x.Price).HasColumnName(@"Price").HasColumnType("money").IsRequired();
            builder.Property(x => x.UpdatedAt).HasColumnName(@"UpdatedAt").HasColumnType("datetime").IsRequired();
            builder.Property(x => x.TransferedAt).HasColumnName(@"TransferedAt").HasColumnType("datetime").IsRequired(false);
            builder.Property(x => x.TargetTeamId).HasColumnName(@"TargetTeamId").HasColumnType("bigint").IsRequired(false);
            builder.Property(x => x.StatusId).HasColumnName(@"StatusId").HasColumnType("smallint").IsRequired();

            // Foreign keys
            builder.HasOne(a => a.Player).WithMany(b => b.TransferLists).HasForeignKey(c => c.PlayerId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_TransferList_Players");
            builder.HasOne(a => a.SourceTeam).WithMany(b => b.TransferLists_SourceTeams).HasForeignKey(c => c.SourceTeamId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_TransferList_SourceTeam");
            builder.HasOne(a => a.TargetTeam).WithMany(b => b.TransferLists_TargetTeams).HasForeignKey(c => c.TargetTeamId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_TransferList_TargetTeam");
            builder.HasOne(a => a.TransferListStatus).WithMany(b => b.TransferLists).HasForeignKey(c => c.StatusId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_TransferList_TransferListStatus");
        }
    }
}
