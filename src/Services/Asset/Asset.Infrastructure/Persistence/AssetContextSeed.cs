using Microsoft.EntityFrameworkCore;
using Asset.Domain.Entities;
using Serilog;

namespace Asset.Infrastructure.Persistence
{
    public class AssetContextSeed
    {
        private readonly ILogger _logger;
        private readonly AssetContext _context;

        public AssetContextSeed(ILogger logger, AssetContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task InitialiseAsync()
        {
            try
            {
                if (_context.Database.IsSqlServer())
                {
                    await _context.Database.MigrateAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An error occurred while initialising the database.");
                throw;
            }
        }

        public async Task SeedAsync()
        {
            try
            {
                await TrySeedAsync();
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An error occurred while seeding the database.");
                throw;
            }
        }

        public async Task TrySeedAsync()
        {
            if (!_context.Assets.Any())
            {
                await _context.Assets.AddRangeAsync(
                    new AssetCMMS
                    {
                        Code = "ASSET001",
                        Name = "Asset 1",
                    });
            }
        }
    }
}
