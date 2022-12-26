using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SoccerManager.Domain.Entities;

namespace SoccerManager.Infra.Data.EntityConfig
{
    // TransferListStatus
    public class TransferListStatusConfiguration : IEntityTypeConfiguration<TransferListStatusEntity>
    {
        public void Configure(EntityTypeBuilder<TransferListStatusEntity> builder)
        {
            builder.ToTable("TransferListStatus", "dbo");
            builder.HasKey(x => x.Id).HasName("PK_TransferListStatus").IsClustered();

            builder.Property(x => x.Id).HasColumnName(@"Id").HasColumnType("smallint").IsRequired().ValueGeneratedNever();
            builder.Property(x => x.StatusName).HasColumnName(@"StatusName").HasColumnType("varchar(11)").IsRequired().IsUnicode(false).HasMaxLength(11);

            builder.HasData(
                    new TransferListStatusEntity
                    {
                        Id = 1,
                        StatusName = "Open"
                    },
                    new TransferListStatusEntity
                    {
                        Id = 2,
                        StatusName = "Transferred"
                    },
                    new TransferListStatusEntity
                    {
                        Id = 3,
                        StatusName = "Canceled"
                    }
                );
        }
    }
}
