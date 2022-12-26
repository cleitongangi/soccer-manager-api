using Microsoft.EntityFrameworkCore;
using SoccerManager.Domain.Entities;
using SoccerManager.Infra.Data.EntityConfig;

namespace SoccerManager.Infra.Data.Context
{
    public class SoccerManagerDbContext : DbContext, ISoccerManagerDbContext
    {
        public DbSet<PlayerEntity> Players { get; set; } = null!; // Players
        public DbSet<PlayerPositionEntity> PlayerPositions { get; set; } = null!; // PlayerPositions
        public DbSet<TeamEntity> Teams { get; set; } = null!;// Teams
        public DbSet<TeamPlayerEntity> TeamPlayers { get; set; } = null!;// TeamPlayers
        public DbSet<TransferListEntity> TransferLists { get; set; } = null!;// TransferList
        public DbSet<TransferListStatusEntity> TransferListStatus { get; set; } = null!;// TransferListStatus
        public DbSet<UserEntity> Users { get; set; } = null!;// Users

        public SoccerManagerDbContext(DbContextOptions<SoccerManagerDbContext> dbContextOptions)
            : base(dbContextOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new PlayerConfiguration());
            modelBuilder.ApplyConfiguration(new PlayerPositionConfiguration());
            modelBuilder.ApplyConfiguration(new TeamConfiguration());
            modelBuilder.ApplyConfiguration(new TeamPlayerConfiguration());
            modelBuilder.ApplyConfiguration(new TransferListConfiguration());
            modelBuilder.ApplyConfiguration(new TransferListStatusConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
        }
    }
}
