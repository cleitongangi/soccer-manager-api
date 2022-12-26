using SoccerManager.Domain.ValueObject;

namespace SoccerManager.Domain.Entities
{
    // Teams
    public class TeamEntity
    {
        public long TeamId { get; set; } // TeamId (Primary key)
        public string TeamName { get; set; } = null!;// TeamName (length: 50)
        public string TeamCountry { get; set; } = null!;// TeamCountry (length: 50)
        public decimal Budget { get; set; } // Budget

        // Reverse navigation

        /// <summary>
        /// Child TeamPlayers where [TeamPlayers].[UserId] point to this entity (FK_TeamPlayers_Teams)
        /// </summary>
        public virtual ICollection<TeamPlayerEntity> TeamPlayers { get; set; } = null!; // TeamPlayers.FK_TeamPlayers_Teams

        /// <summary>
        /// Child TransferLists where [TransferList].[SourceUserId] point to this entity (FK_TransferList_SourceTeam)
        /// </summary>
        public virtual ICollection<TransferListEntity> TransferLists_SourceTeams { get; set; } = null!; // TransferList.FK_TransferList_SourceTeam

        /// <summary>
        /// Child TransferLists where [TransferList].[TargetUserId] point to this entity (FK_TransferList_TargetTeam)
        /// </summary>
        public virtual ICollection<TransferListEntity> TransferLists_TargetTeams { get; set; } = null!; // TransferList.FK_TransferList_TargetTeam

        // Foreign keys

        /// <summary>
        /// Parent User pointed by [Teams].([UserId]) (FK_Teams_Users)
        /// </summary>
        public virtual UserEntity? User { get; set; } // FK_Teams_Users

        public TeamEntity()
        {
            TeamPlayers = new List<TeamPlayerEntity>();
            TransferLists_SourceTeams = new List<TransferListEntity>();
            TransferLists_TargetTeams = new List<TransferListEntity>();
        }

        public TeamEntity(long teamId, string teamName, string teamCountry)
            : base()
        {
            TeamId = teamId;
            TeamName = teamName;
            TeamCountry = teamCountry;
        }

        public static class Factory
        {
            public static TeamEntity NewInitialTeam(long teamId)
            {
                return new TeamEntity
                {
                    TeamId = teamId,
                    TeamCountry = CountryVO.GetRandomCountry(), // TODO: If I have time, change to get random from a list
                    TeamName = $"{NameVO.GenerateName()}'s Team",
                    Budget = 5000000
                };
            }
        }
    }
}

