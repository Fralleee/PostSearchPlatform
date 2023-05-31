using CareersFralle.Models;
using Nest;

namespace CareersFralle.Extensions;

public static class ElasticSearchExtensions
{
    public static void AddElasticsearch(
        this IServiceCollection services, IConfiguration configuration)
    {
        var url = configuration["ELKConfiguration:Uri"];
        var defaultIndex = configuration["ELKConfiguration:index"];
        var username = configuration["ELKConfiguration:Username"];
        var password = configuration["ELKConfiguration:Password"];

        var settings = new ConnectionSettings(new Uri(url))
            .BasicAuthentication(username, password)
            .PrettyJson()
            .DefaultIndex(defaultIndex);

        AddDefaultMappings(settings);

        var client = new ElasticClient(settings);

        services.AddSingleton<IElasticClient>(client);

        CreateIndex(client, defaultIndex);
    }

    private static void AddDefaultMappings(ConnectionSettings settings)
    {
        settings.DefaultMappingFor<Post>(m => m);
        //settings.DefaultMappingFor<Post>(m => m.Ignore(p => p.Host));
    }

    private static void CreateIndex(IElasticClient client, string indexName)
    {
        var createIndexResponse = client.Indices.Create(indexName,
            index => index.Map<Post>(x => x.AutoMap())
        );
    }
}
