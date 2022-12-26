namespace SoccerManager.Domain.Entities
{
    // PlayerPositions
    public class PlayerPositionEntity
    {
        public short Id { get; set; } // Id (Primary key)
        public string PositionName { get; set; } = null!;// PositionName (length: 50)

        // Reverse navigation

        /// <summary>
        /// Child Players where [Players].[PositionId] point to this entity (FK_Players_PlayerPositions)
        /// </summary>
        public virtual ICollection<PlayerEntity> Players { get; set; } // Players.FK_Players_PlayerPositions

        public PlayerPositionEntity()
        {
            Players = new List<PlayerEntity>();
        }
    }
}

