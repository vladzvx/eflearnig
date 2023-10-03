using EF.Learning.Storage;
using Microsoft.EntityFrameworkCore;

namespace EF.Learning.Services
{
    public class DbCreator : IHostedService
    {
        private readonly IDbContextFactory<TestDbContext> _fact;
        public DbCreator(IDbContextFactory<TestDbContext> factory)
        {
            _fact = factory;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                using var cont =await _fact.CreateDbContextAsync();
                cont.Database.EnsureCreated();
            }
            catch (Exception ex) 
            { 
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
