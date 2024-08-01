using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Reflection;
using Asset.Domain.Entities;

namespace Asset.Infrastructure.Persistence
{
    public class AssetContext : DbContext
    {
        private readonly IMediator _mediator;
        private readonly ILogger _logger;
        public AssetContext(DbContextOptions<AssetContext> options, IMediator mediator, ILogger logger) : base(options)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public DbSet<AssetCMMS> Assets { get; set; }
        //private List<BaseEvent> _baseEvents;

        //private void SetBaseEventsBeforeSaveChanges()
        //{
        //    var domainEntities = ChangeTracker.Entries<IEventEntity>()
        //        .Select(x => x.Entity)
        //        .Where(x => x.DomainEvents().Any())
        //        .ToList();

        //    _baseEvents = domainEntities
        //        .SelectMany(x => x.DomainEvents())
        //        .ToList();

        //    domainEntities.ForEach(x => x.ClearDomainEvents());
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            //SetBaseEventsBeforeSaveChanges();
            //var modified = ChangeTracker.Entries()
            //    .Where(e => e.State == EntityState.Modified ||
            //                e.State == EntityState.Added ||
            //                e.State == EntityState.Deleted);

            //TODO: Check State
            //foreach (var item in modified)
            //{
            //    switch (item.State)
            //    {
            //        case EntityState.Added:
            //            if (item.Entity is IDateTracking addedEntity)
            //            {
            //                addedEntity.CreatedDate = DateTime.UtcNow;
            //                item.State = EntityState.Added;
            //            }
            //            break;

            //        case EntityState.Modified:
            //            Entry(item.Entity).Property("Id").IsModified = false;
            //            if (item.Entity is IDateTracking modifiedEntity)
            //            {
            //                modifiedEntity.LastModifiedDate = DateTime.UtcNow;
            //                item.State = EntityState.Modified;
            //            }
            //            break;
            //    }
            //}

            // Dispatch Domain Events collection. 
            // Choices:
            // A) Right BEFORE committing data (EF SaveChanges) into the DB will make a single transaction including  
            // side effects from the domain event handlers which are using the same DbContext with "InstancePerLifetimeScope" or "scoped" lifetime
            // B) Right AFTER committing data (EF SaveChanges) into the DB will make multiple transactions. 
            // You will need to handle eventual consistency and compensatory actions in case of failures in any of the Handlers. 

            //await _mediator.DispatchDomainEventsAsync(_baseEvents, _logger);

            // After executing this line all the changes (from the Command Handler and Domain Event Handlers) 
            // performed through the DbContext will be committed
            var result = await base.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}
