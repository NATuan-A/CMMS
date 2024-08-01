using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Application.Services.Interfaces
{
    public interface IAssetService
    {
        IResult GetAssets();
    }
}
