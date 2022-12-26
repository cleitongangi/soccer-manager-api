using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SoccerManager.Domain.Entities;

namespace SoccerManager.Infra.Data.EntityConfig
{
    // Players
    public class PlayerConfiguration : IEntityTypeConfiguration<PlayerEntity>
    {
        public void Configure(EntityTypeBuilder<PlayerEntity> builder)
        {
            builder.ToTable("Players", "dbo");
            builder.HasKey(x => x.Id).HasName("PK_Players").IsClustered();

            builder.Property(x => x.Id).HasColumnName(@"Id").HasColumnType("bigint").IsRequired().ValueGeneratedOnAdd().UseIdentityColumn();
            builder.Property(x => x.FirstName).HasColumnName(@"FirstName").HasColumnType("nvarchar(50)").IsRequired().HasMaxLength(50);
            builder.Property(x => x.LastName).HasColumnName(@"LastName").HasColumnType("nvarchar(50)").IsRequired().HasMaxLength(50);
            builder.Property(x => x.Country).HasColumnName(@"Country").HasColumnType("nvarchar(50)").IsRequired().HasMaxLength(50);
            builder.Property(x => x.Age).HasColumnName(@"Age").HasColumnType("int").IsRequired();
            builder.Property(x => x.MarketValue).HasColumnName(@"MarketValue").HasColumnType("money").IsRequired();
            builder.Property(x => x.PositionId).HasColumnName(@"PositionId").HasColumnType("smallint").IsRequired();
            builder.Property(x => x.CreatedAt).HasColumnName(@"CreatedAt").HasColumnType("datetime").IsRequired();
            builder.Property(x => x.UpdatedAt).HasColumnName(@"UpdatedAt").HasColumnType("datetime").IsRequired();

            // Foreign keys
            builder.HasOne(a => a.PlayerPosition).WithMany(b => b.Players).HasForeignKey(c => c.PositionId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_Players_PlayerPositions");
        }
    }
}
