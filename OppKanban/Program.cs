using OppKanban.Components;
using ApexCharts;
using MongoDB.Bson;
using MongoDB.Driver;
using OppKanban.Datamodels;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddApexCharts();

static void ConfigureMDBServices(IServiceCollection services)
{
    
    string MDBCONNSTR = "mongodb://admin:admin@mongodb:27017";
    var settings = MongoClientSettings.FromConnectionString(MDBCONNSTR);
    settings.ServerApi = new ServerApi(ServerApiVersion.V1);

    services.AddSingleton<IMongoClient>(new MongoClient(settings));
    //services.AddSingleton<IMongoDatabase>(x => x.GetRequiredService<IMongoClient>().GetDatabase("dbname"));
    services.AddSingleton<IMongoCollection<OppBase>>(x => x.GetRequiredService<IMongoClient>().GetDatabase("OppKanban").GetCollection<OppBase>("opps"));    
    services.AddSingleton<IMongoCollection<OppMetadata>>(x => x.GetRequiredService<IMongoClient>().GetDatabase("OppKanban").GetCollection<OppMetadata>("metadata"));    
    services.AddSingleton<IMongoCollection<OppView>>(x => x.GetRequiredService<IMongoClient>().GetDatabase("OppKanban").GetCollection<OppView>("v__oppsAndMetadata"));

}

ConfigureMDBServices(builder.Services);

var app = builder.Build();

// Ensure the view exists on startup
using (var scope = app.Services.CreateScope())
{
    var mongoClient = scope.ServiceProvider.GetRequiredService<IMongoClient>();
    var db = mongoClient.GetDatabase("OppKanban");
    const string viewName = "v__oppsAndMetadata";

    var collections = await db.ListCollectionNamesAsync();
    var allNames = await collections.ToListAsync();

    var pipeline = new[]
    {
        new BsonDocument("$project", new BsonDocument
        {
            { "_id", 1 },
            { "oppDetails", "$$ROOT" }
        }),
        new BsonDocument("$lookup", new BsonDocument
        {
            { "from", "metadata" },
            { "localField", "_id" },
            { "foreignField", "oppId" },
            { "as", "oppMetadata" }
        }),
        new BsonDocument("$project", new BsonDocument
        {
            { "_id", 1 },
            { "oppDetails", 1 },
            { "oppMetadata", new BsonDocument("$first", "$oppMetadata") }
        })
    };

    if (allNames.Contains(viewName))
    {
        await db.RunCommandAsync<BsonDocument>(new BsonDocument
        {
            { "collMod", viewName },
            { "viewOn", "opps" },
            { "pipeline", new BsonArray(pipeline) }
        });
    }
    else
    {
        await db.CreateViewAsync(viewName, "opps", PipelineDefinition<BsonDocument, BsonDocument>.Create(pipeline));
    }

    // Ensure the Atlas Search index used by opportunity search exists on startup
    const string searchIndexName = "default";
    var oppsCollection = db.GetCollection<BsonDocument>("opps");
    var existingIndexes = await (await oppsCollection.SearchIndexes.ListAsync()).ToListAsync();

    if (!existingIndexes.Any(index => index["name"].AsString == searchIndexName))
    {
        var searchIndexDefinition = new BsonDocument("mappings", new BsonDocument("dynamic", true));
        await oppsCollection.SearchIndexes.CreateOneAsync(new CreateSearchIndexModel(searchIndexName, searchIndexDefinition));
    }
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
