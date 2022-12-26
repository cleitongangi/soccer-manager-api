using SoccerManager.Domain.Enum;

namespace SoccerManager.Domain.Entities
{
    // TransferList
    public class TransferListEntity
    {
        public long Id { get; set; } // Id (Primary key)
        public long PlayerId { get; set; } // PlayerId
        public DateTime CreatedAt { get; set; } // CreatedAt
        public long SourceTeamId { get; set; } // SourceTeamId
        public decimal Price { get; set; } // Price
        public DateTime UpdatedAt { get; set; } // UpdatedAt
        public DateTime? TransferedAt { get; set; } // TransferedAt
        public long? TargetTeamId { get; set; } // TargetTeamId
        public short StatusId { get; set; } // StatusId

        // Foreign keys

        /// <summary>
        /// Parent Player pointed by [TransferList].([PlayerId]) (FK_TransferList_Players)
        /// </summary>
        public virtual PlayerEntity Player { get; set; } = null!; // FK_TransferList_Players

        /// <summary>
        /// Parent Team pointed by [TransferList].([SourceUserId]) (FK_TransferList_SourceTeam)
        /// </summary>
        public virtual TeamEntity SourceTeam { get; set; } = null!; // FK_TransferList_SourceTeam

        /// <summary>
        /// Parent Team pointed by [TransferList].([TargetUserId]) (FK_TransferList_TargetTeam)
        /// </summary>
        public virtual TeamEntity TargetTeam { get; set; } = null!; // FK_TransferList_TargetTeam

        /// <summary>
        /// Parent TransferListStatu pointed by [TransferList].([StatusId]) (FK_TransferList_TransferListStatus)
        /// </summary>
        public virtual TransferListStatusEntity TransferListStatus { get; set; } = null!; // FK_TransferList_TransferListStatus

        public static class Factory
        {
            public static TransferListEntity NewTransfer(long teamId, long playerId, decimal price)
            {
                return new TransferListEntity
                {
                    PlayerId = playerId,
                    SourceTeamId = teamId,
                    Price = price,
                    StatusId = (short)TransferListStatusEnum.Open,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
            }

            public static TransferListEntity CreateToCancelTransfer(long transferId, long sourceTeamId)
            {
                return new TransferListEntity
                {
                    Id = transferId,
                    SourceTeamId = sourceTeamId,
                    StatusId = (short)TransferListStatusEnum.Canceled,
                    UpdatedAt = DateTime.Now
                };
            }

            public static TransferListEntity CreateToBuy(long transferId, long targetTeamId)
            {
                return new TransferListEntity
                {
                    Id = transferId,
                    TargetTeamId = targetTeamId,
                    StatusId = (short)TransferListStatusEnum.Transferred,
                    TransferedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
            }
        }
    }
}
