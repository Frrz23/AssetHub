using AssetHub.Application.Contracts.Tags;
using AssetHub.AuditLogService;
using AssetHub.Entities.Asset;
using AssetHub.Entities.AuditLog;
using AssetHub.Entities.Tag;
using AutoMapper;

namespace AssetHub;

public class AssetHubApplicationAutoMapperProfile : Profile
{
    public AssetHubApplicationAutoMapperProfile()
    {
        CreateMap<Tag, TagDto>();
        CreateMap<CreateTagDto, Tag>();
        CreateMap<AssetHub.Entities.Asset.CreateAssetDto, AssetHub.Entities.Asset.Asset>();
        CreateMap<AssetHub.Entities.Asset.Asset, AssetHub.Entities.Asset.AssetDto>();
        CreateMap<AuditLog, AuditLogDto>();


    }
}
