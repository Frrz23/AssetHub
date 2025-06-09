using AssetHub.Application.Contracts.Tags;
using AssetHub.Entities.Tag;
using AutoMapper;

namespace AssetHub;

public class AssetHubApplicationAutoMapperProfile : Profile
{
    public AssetHubApplicationAutoMapperProfile()
    {
        CreateMap<Tag, TagDto>();
        CreateMap<CreateTagDto, Tag>();

    }
}
