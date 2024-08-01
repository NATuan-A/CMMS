using Asset.Application.Common.Models;
using Asset.Application.Services;
using Asset.Application.Services.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Asset.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AssetController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IAssetService _assetService;

        public AssetController(IAssetService assetService, IMapper mapper)
        {
            _mapper = mapper;
            _assetService = assetService;
        }

        private static class RouteNames
        {
            public const string GetAssets = nameof(GetAssets);
        }

        #region CRUD
        [HttpGet("getAll", Name = RouteNames.GetAssets)]
        public async Task<ActionResult<List<AssetDto>>> GetOrdersByUserName()
        {
            var result = _assetService.GetAssets();
            return Ok(result);
        }

        #endregion
    }
}
