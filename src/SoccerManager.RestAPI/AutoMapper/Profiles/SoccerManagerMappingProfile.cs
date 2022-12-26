using AutoMapper;
using SoccerManager.Domain.Core.Pagination;
using SoccerManager.Domain.DTOs;
using SoccerManager.Domain.Entities;
using SoccerManager.RestAPI.ApiResponses;
using SoccerManager.RestAPI.AutoMapper.CustomConverters;

namespace SoccerManager.RestAPI.AutoMapper.Profiles
{
    public class SoccerManagerMappingProfile : Profile
    {
        public SoccerManagerMappingProfile()
        {
            CreateMap<GetTeamDetailsDTO, GetTeamResponse>(MemberList.None);

            CreateMap<PlayerEntity, GetTeamPlayerReponse>(MemberList.None);

            #region Mapp TransferListEntity to GetTransferListResponse
            CreateMap<PlayerEntity, GetTransferListResponse.PlayerResponse>(MemberList.None)
                .ForMember(dest => dest.PositionName, opt => opt.MapFrom(src => src.PlayerPosition.PositionName));
            CreateMap<TeamEntity, GetTransferListResponse.SourceTeamResponse>(MemberList.None);
            CreateMap<TransferListEntity, GetTransferListResponse>(MemberList.None)
                .ForMember(dest => dest.TransferId, opt => opt.MapFrom(src => src.Id));
            CreateMap<PagedResult<TransferListEntity>, PagedResult<GetTransferListResponse>>(MemberList.None)
                .ConvertUsing<PagedResultConverter<TransferListEntity, GetTransferListResponse>>();
            #endregion Mapp TransferListEntity to GetTransferListResponse
        }
    }
}
