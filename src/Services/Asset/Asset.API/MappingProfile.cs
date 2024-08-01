using Asset.Application.Common.Models;
using Asset.Domain.Entities;
using AutoMapper;

namespace Asset.API
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AssetCMMS, AssetDto>();
        }
    }
}
