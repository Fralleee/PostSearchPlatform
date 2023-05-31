using Microsoft.Extensions.Caching.Distributed;
using PostSearchPlatform.Extensions;
using PostSearchPlatform.Models;
using StackExchange.Redis;

namespace PostSearchPlatform.Services;

public class ClickBatchService : BackgroundService
{
    private readonly IDistributedCache _cache;
    //private readonly IConfiguration _configuration;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IServer _server;

    private const string INSTANCE_NAME = "Fralle_";

    public ClickBatchService(IDistributedCache cache, IServiceScopeFactory scopeFactory, IServer server)
    {
        _cache = cache;
        _scopeFactory = scopeFactory;
        _server = server;
    }

    public async Task BatchEntries(CancellationToken cancellationToken)
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
            cancellationToken.ThrowIfCancellationRequested();

            var keyString = key.ToString().Substring(INSTANCE_NAME.Length);
            var click = await _cache.GetRecordAsync<Click>(keyString);
            if (click != null)
            {
                await clicksService.RecordClick(click, cancellationToken);
                await _cache.RemoveAsync(key, cancellationToken);
            }
        }
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(TimeSpan.FromSeconds(30));
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                if (await timer.WaitForNextTickAsync(stoppingToken))
                {
                    await BatchEntries(stoppingToken);
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                throw;
            }
        }
    }
}
