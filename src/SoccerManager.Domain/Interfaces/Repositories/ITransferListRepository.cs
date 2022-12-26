using SoccerManager.Domain.Core.Pagination;
using SoccerManager.Domain.Entities;

namespace SoccerManager.Domain.Interfaces.Repositories
{
    public interface ITransferListRepository
    {
        Task AddAsync(TransferListEntity entity);
        Task<bool> CanBuyAsync(long id, long targetTeamId);
        Task<int> CancelAsync(TransferListEntity entity);
        Task<PagedResult<TransferListEntity>> GetAsync(long? teamId, int page = 1);
        Task<TransferListEntity?> GetAsync(long? transferId);
        Task<TransferListEntity?> GetDataToTransferAsync(long id);
        Task<bool> HasOpenAsync(long sourceTeamId, long playerId);
        Task<bool> HasOpenByTransferIdAsync(long id, long sourceTeamId);
        Task<int> TransferAsync(TransferListEntity entity);
    }
}
