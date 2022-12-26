using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SoccerManager.Domain.Entities;

namespace SoccerManager.Infra.Data.EntityConfig
{
    // PlayerPositions
    public class PlayerPositionConfiguration : IEntityTypeConfiguration<PlayerPositionEntity>
    {
        public void Configure(EntityTypeBuilder<PlayerPositionEntity> builder)
        {
            builder.ToTable("PlayerPositions", "dbo");
            builder.HasKey(x => x.Id).HasName("PK_PlayerPositions").IsClustered();

            builder.Property(x => x.Id).HasColumnName(@"Id").HasColumnType("smallint").IsRequired().ValueGeneratedNever();
            builder.Property(x => x.PositionName).HasColumnName(@"PositionName").HasColumnType("varchar(50)").IsRequired().IsUnicode(false).HasMaxLength(50);
                        
            builder.HasData(
                    new PlayerPositionEntity
                    {
                        Id = 1,
                        PositionName = "Goalkeeper"
                    },
                    new PlayerPositionEntity
                    {
                        Id = 2,
                        PositionName = "Defender"
                    },
                    new PlayerPositionEntity
                    {
                        Id = 3,
                        PositionName = "Midfielder"
                    },
                    new PlayerPositionEntity
                    {
                        Id = 4,
                        PositionName = "Attacker"
                    }
                );
        }
    }
}
