using AssetHub.Application.Contracts.Tags;
using AssetHub.Entities.Asset;
using AssetHub.Entities.Tag;
using AutoMapper;

namespace AssetHub;

public class AssetHubApplicationAutoMapperProfile : Profile
{
    public AssetHubApplicationAutoMapperProfile()
    {
        CreateMap<Tag, TagDto>();
        CreateMap<CreateTagDto, Tag>();
        CreateMap<CreateAssetDto, Asset>();
        CreateMap<Asset, AssetDto>();


    }
}
