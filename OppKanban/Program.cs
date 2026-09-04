using OppKanban.Components;
using ApexCharts;
using MongoDB.Driver;
using OppKanban.Datamodels;
using OppKanban.Migrations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddApexCharts();

static void ConfigureMDBServices(IServiceCollection services)
{
    var connectionString = Environment.GetEnvironmentVariable("MongoDb__ConnectionString")
        ?? throw new InvalidOperationException("MongoDb__ConnectionString environment variable is required.");

    var settings = MongoClientSettings.FromConnectionString(connectionString);
    settings.ServerApi = new ServerApi(ServerApiVersion.V1);

    services.AddSingleton<IMongoClient>(new MongoClient(settings));
    services.AddSingleton<IMongoDatabase>(x => x.GetRequiredService<IMongoClient>().GetDatabase("OppKanban"));
    //services.AddSingleton<IMongoDatabase>(x => x.GetRequiredService<IMongoClient>().GetDatabase("dbname"));
    services.AddSingleton<IMongoCollection<OppBase>>(x => x.GetRequiredService<IMongoDatabase>().GetCollection<OppBase>("opps"));    
    services.AddSingleton<IMongoCollection<OppMetadata>>(x => x.GetRequiredService<IMongoDatabase>().GetCollection<OppMetadata>("metadata"));    
    services.AddSingleton<IMongoCollection<OppView>>(x => x.GetRequiredService<IMongoDatabase>().GetCollection<OppView>("v__oppsAndMetadata"));
    services.AddSingleton<IMongoCollection<SA>>(x => x.GetRequiredService<IMongoDatabase>().GetCollection<SA>("sas"));

}

ConfigureMDBServices(builder.Services);

var app = builder.Build();

// Apply the view / search index definitions declared in the Migrations folder
using (var scope = app.Services.CreateScope())
{
    var mongoClient = scope.ServiceProvider.GetRequiredService<IMongoClient>();
    var db = mongoClient.GetDatabase("OppKanban");
    var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("MongoMigrator");

    await MongoMigrator.ApplyAsync(db, Path.Combine(AppContext.BaseDirectory, "Migrations"), logger);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.Run();
