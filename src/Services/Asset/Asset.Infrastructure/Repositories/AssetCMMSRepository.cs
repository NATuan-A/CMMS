using Asset.Application.Common.Interfaces;
using Asset.Domain.Entities;
using Asset.Infrastructure.Persistence;
using Contracts.Domains.Interfaces;
using Infrastructure.Common.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Infrastructure.Repositories
{
    public class AssetCMMSRepository : RepositoryBase<AssetCMMS, long, AssetContext>, IAssetRepository
    {
        public AssetCMMSRepository(AssetContext dbContext, IUnitOfWork<AssetContext> unitOfWork) : base(dbContext, unitOfWork)
        {
        }
    }
}
