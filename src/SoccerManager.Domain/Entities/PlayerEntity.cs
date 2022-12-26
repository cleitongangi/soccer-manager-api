using SoccerManager.Domain.Enum;
using SoccerManager.Domain.ValueObject;

namespace SoccerManager.Domain.Entities
{
    // Players
    public class PlayerEntity
    {
        public long Id { get; set; } // Id (Primary key)
        public string FirstName { get; set; } = null!;// FirstName (length: 50)
        public string LastName { get; set; } = null!;// LastName (length: 50)
        public string Country { get; set; } = null!;// Country (length: 50)
        public int Age { get; set; } // Age
        public decimal MarketValue { get; set; } // MarketValue
        public short PositionId { get; set; } // PositionId
        public DateTime CreatedAt { get; set; } // CreatedAt
        public DateTime UpdatedAt { get; set; } // UpdatedAt

        // Reverse navigation

        /// <summary>
        /// Child TeamPlayers where [TeamPlayers].[PlayerId] point to this entity (FK_TeamPlayers_Players)
        /// </summary>
        public virtual ICollection<TeamPlayerEntity> TeamPlayers { get; set; } // TeamPlayers.FK_TeamPlayers_Players

        /// <summary>
        /// Child TransferLists where [TransferList].[PlayerId] point to this entity (FK_TransferList_Players)
        /// </summary>
        public virtual ICollection<TransferListEntity> TransferLists { get; set; } // TransferList.FK_TransferList_Players

        // Foreign keys

        /// <summary>
        /// Parent PlayerPosition pointed by [Players].([PositionId]) (FK_Players_PlayerPositions)
        /// </summary>
        public virtual PlayerPositionEntity PlayerPosition { get; set; } = null!; // FK_Players_PlayerPositions

        public PlayerEntity()
        {
            TeamPlayers = new List<TeamPlayerEntity>();
            TransferLists = new List<TransferListEntity>();
        }

        /// <summary>
        /// Increase MarketValue randomly between 10% and 100%
        /// </summary>
        public void IncreaseMarketValue()
        {
            var factor = new Random().Next(10, 100) / 100.0M;
            MarketValue = MarketValue * (factor + 1);
        }

        public static class Factory
        {
            public static PlayerEntity NewInitialPlayer(PlayerPositionEnum position)
            {
                return new PlayerEntity
                {
                    Age = new Random().Next(18, 40),
                    MarketValue = 1000000,
                    Country = CountryVO.GetRandomCountry(),
                    FirstName = NameVO.GenerateName(),
                    LastName = NameVO.GenerateName(),
                    PositionId = (short)position,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
            }

            public static PlayerEntity NewForUpdate(long playerId, string? firstName, string? lastName, string? country)
            {
                return new PlayerEntity
                {
                    Id = playerId,
                    FirstName = firstName ?? "",
                    LastName = lastName ?? "",
                    Country = country ?? "",
                    UpdatedAt = DateTime.Now,
                };
            }

            public static PlayerEntity CreateForTransfer(long playerId, decimal price)
            {
                return new PlayerEntity
                {
                    Id = playerId,
                    MarketValue = price,
                    UpdatedAt = DateTime.Now
                };
            }
        }
    }
}

