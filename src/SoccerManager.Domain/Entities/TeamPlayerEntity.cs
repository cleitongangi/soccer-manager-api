namespace SoccerManager.Domain.Entities
{
    // TeamPlayers
    public class TeamPlayerEntity
    {
        public long TeamId { get; set; } // TeamId (Primary key)
        public long PlayerId { get; set; } // PlayerId (Primary key)
        public int Sequence { get; set; } // Sequence (Primary key)
        public DateTime CreatedAt { get; set; } // CreatedAt
        public DateTime? RemovedAt { get; set; } // RemovedAt
        public bool Active { get; set; } // Active

        // Foreign keys

        /// <summary>
        /// Parent Player pointed by [TeamPlayers].([PlayerId]) (FK_TeamPlayers_Players)
        /// </summary>
        public virtual PlayerEntity? Player { get; set; } // FK_TeamPlayers_Players

        /// <summary>
        /// Parent Team pointed by [TeamPlayers].([UserId]) (FK_TeamPlayers_Teams)
        /// </summary>
        public virtual TeamEntity? Team { get; set; } // FK_TeamPlayers_Teams

        private TeamPlayerEntity() { } // For Entity Framework

        public static class Factory
        {
            public static TeamPlayerEntity NewInitialPlayer(long teamId, long playerId)
            {
                return new TeamPlayerEntity
                {
                    TeamId = teamId,
                    PlayerId = playerId,
                    Sequence = 1,
                    CreatedAt = DateTime.Now,
                    RemovedAt = DateTime.Now,
                    Active = true
                };
            }

            public static TeamPlayerEntity CreateToTransfer(long teamId, long playerId, int sequence)
            {
                return new TeamPlayerEntity
                {
                    TeamId = teamId,
                    PlayerId = playerId,
                    Sequence = sequence,
                    CreatedAt = DateTime.Now,
                    RemovedAt = DateTime.Now,
                    Active = true
                };
            }
        }
    }
}
