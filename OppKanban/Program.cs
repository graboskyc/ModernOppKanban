using OppKanban.Components;
using MongoDB.Driver;
using OppKanban.Datamodels;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

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
