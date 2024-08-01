using Asset.Application.Common.Interfaces;
using Asset.Application.Common.Models;
using Asset.Application.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Http;

namespace Asset.Application.Services
{
    public class AssetService : IAssetService
    {
        private readonly IMapper _mapper;
        private readonly IAssetRepository _assetRepository;

        public AssetService(IAssetRepository assetRepository, IMapper mapper) {
            _mapper = mapper;
            _assetRepository = _assetRepository;
        }

        public IResult GetAssets()
        {
            var assets = _assetRepository.FindAll().ToList();
            var result = _mapper.Map<List<AssetDto>>(assets);
            return Results.Ok(result);
        }
    }
}
