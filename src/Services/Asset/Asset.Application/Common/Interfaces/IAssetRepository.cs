using Asset.Domain.Entities;
using Contracts.Domains.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Application.Common.Interfaces
{
    public interface IAssetRepository : IRepositoryBase<AssetCMMS, long>
    {
    }
}
