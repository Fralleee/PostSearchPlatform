using CareersFralle.Extensions;
using CareersFralle.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using System.Net;

namespace CareersFralle.Services
{
    public class ClickBatchService : BackgroundService
    {
        private readonly IDistributedCache _cache;
        private readonly IConfiguration _configuration;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IServer _server;

        private const string INSTANCE_NAME = "Fralle_";

        public ClickBatchService(IDistributedCache cache, IConfiguration configuration, IServiceScopeFactory scopeFactory)
        {
            _cache = cache;
            _configuration = configuration;
            _scopeFactory = scopeFactory;

            ConfigurationOptions options = ConfigurationOptions.Parse(_configuration.GetConnectionString("Redis"));
            ConnectionMultiplexer connection = ConnectionMultiplexer.Connect(options);
            EndPoint endPoint = connection.GetEndPoints().First();
            _server = connection.GetServer(endPoint);
        }

        public async void BatchEntries()
        {
            using var scope = _scopeFactory.CreateScope();
            var clicksService = scope.ServiceProvider.GetService<IClicksService>();
            if (clicksService == null)
            {
                return;
            }

            var keys = _server.Keys();
            foreach (var key in keys)
            {
                var keyString = key.ToString().Substring(INSTANCE_NAME.Length);
                var click = await _cache.GetRecordAsync<Click>(keyString);
                if (click != null)
                {
                    await clicksService.RecordClick(click);
                    await _cache.RemoveAsync(key);
                }
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var timer = new PeriodicTimer(TimeSpan.FromSeconds(30));
            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                BatchEntries();
            }
        }
    }
}
