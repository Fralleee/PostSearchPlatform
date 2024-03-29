using Microsoft.EntityFrameworkCore;
using PostSearchPlatform.Data;
using PostSearchPlatform.Extensions;
using PostSearchPlatform.Repository;
using PostSearchPlatform.Services;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DataContext") ?? throw new InvalidOperationException("Connection string 'DataContext' not found.")));

builder.Services.AddElasticsearch(builder.Configuration);

builder.Services.AddSingleton(sp =>
{
    var configurationOptions = ConfigurationOptions.Parse(builder.Configuration.GetConnectionString("Redis") ?? throw new InvalidOperationException("Connection string 'Redis' not found."));
    var connection = ConnectionMultiplexer.Connect(configurationOptions);
    return connection.GetServer(connection.GetEndPoints().First());
});
builder.Services.AddHostedService<ClickBatchService>();

builder.Services.AddScoped<IPostsRepository, PostsRepository>();
builder.Services.AddScoped<IClicksRepository, ClicksRepository>();

builder.Services.AddScoped<IPostsService, PostsService>();
builder.Services.AddScoped<IClicksService, ClicksService>();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<DataContext>();
        context.Database.Migrate();
    }
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
