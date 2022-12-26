namespace SoccerManager.Domain.Entities
{
    // TransferListStatus
    public class TransferListStatusEntity
    {
        public short Id { get; set; } // Id (Primary key)
        public string StatusName { get; set; } = null!;// StatusName (length: 11)

        // Reverse navigation

        /// <summary>
        /// Child TransferLists where [TransferList].[StatusId] point to this entity (FK_TransferList_TransferListStatus)
        /// </summary>
        public virtual ICollection<TransferListEntity> TransferLists { get; set; } // TransferList.FK_TransferList_TransferListStatus

        public TransferListStatusEntity()
        {
            TransferLists = new List<TransferListEntity>();
        }
    }
}
