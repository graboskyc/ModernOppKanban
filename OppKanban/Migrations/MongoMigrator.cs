using MongoDB.Bson;
using MongoDB.Driver;

namespace OppKanban.Migrations;

/// <summary>
/// Applies the declarative migration definitions (views, search indexes) stored as JSON in the Migrations folder.
/// </summary>
public static class MongoMigrator
{
    public static async Task ApplyAsync(IMongoDatabase db, string migrationsPath, ILogger? logger = null)
    {
        if (!Directory.Exists(migrationsPath))
        {
            logger?.LogWarning("Migrations folder not found at {Path}; skipping migrations.", migrationsPath);
            return;
        }

        foreach (var file in Directory.EnumerateFiles(migrationsPath, "*.json").OrderBy(f => f, StringComparer.OrdinalIgnoreCase))
        {
            BsonDocument migration;
            try
            {
                migration = BsonDocument.Parse(await File.ReadAllTextAsync(file));
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "Failed to parse migration file {File}.", file);
                continue;
            }

            var type = migration.GetValue("type", BsonNull.Value).AsNullableString();

            switch (type)
            {
                case "view":
                    await ApplyViewAsync(db, migration, file, logger);
                    break;
                case "searchIndex":
                    await ApplySearchIndexAsync(db, migration, file, logger);
                    break;
                default:
                    logger?.LogWarning("Migration {File} has unknown or missing \"type\" ({Type}); skipping.", file, type ?? "null");
                    break;
            }
        }
    }

    private static async Task ApplyViewAsync(IMongoDatabase db, BsonDocument migration, string file, ILogger? logger)
    {
        var name = migration.GetValue("name", BsonNull.Value).AsNullableString();
        var viewOn = migration.GetValue("viewOn", BsonNull.Value).AsNullableString();
        var pipeline = migration.GetValue("pipeline", BsonNull.Value) as BsonArray;

        if (name is null || viewOn is null || pipeline is null)
        {
            logger?.LogError("View migration {File} requires \"name\", \"viewOn\" and \"pipeline\".", file);
            return;
        }

        var stages = pipeline.Select(stage => stage.AsBsonDocument).ToArray();
        var existing = await (await db.ListCollectionNamesAsync()).ToListAsync();

        if (existing.Contains(name))
        {
            await db.RunCommandAsync<BsonDocument>(new BsonDocument
            {
                { "collMod", name },
                { "viewOn", viewOn },
                { "pipeline", new BsonArray(stages) }
            });
            logger?.LogInformation("Updated view {View}.", name);
        }
        else
        {
            await db.CreateViewAsync(name, viewOn, PipelineDefinition<BsonDocument, BsonDocument>.Create(stages));
            logger?.LogInformation("Created view {View}.", name);
        }
    }

    private static async Task ApplySearchIndexAsync(IMongoDatabase db, BsonDocument migration, string file, ILogger? logger)
    {
        var name = migration.GetValue("name", BsonNull.Value).AsNullableString();
        var collectionName = migration.GetValue("collection", BsonNull.Value).AsNullableString();
        var definition = migration.GetValue("definition", BsonNull.Value) as BsonDocument;

        if (name is null || collectionName is null || definition is null)
        {
            logger?.LogError("Search index migration {File} requires \"name\", \"collection\" and \"definition\".", file);
            return;
        }

        var collection = db.GetCollection<BsonDocument>(collectionName);
        
        // Ensure collection exists
        var collections = await (await db.ListCollectionNamesAsync()).ToListAsync();
        if (!collections.Contains(collectionName))
        {
            await db.CreateCollectionAsync(collectionName);
            logger?.LogInformation("Created collection {Collection} for search index.", collectionName);
        }

        var existing = await (await collection.SearchIndexes.ListAsync()).ToListAsync();

        if (existing.Any(index => index.GetValue("name", BsonNull.Value).AsNullableString() == name))
        {
            await collection.SearchIndexes.UpdateAsync(name, definition);
            logger?.LogInformation("Updated search index {Index} on {Collection}.", name, collectionName);
        }
        else
        {
            await collection.SearchIndexes.CreateOneAsync(new CreateSearchIndexModel(name, definition));
            logger?.LogInformation("Created search index {Index} on {Collection}.", name, collectionName);
        }
    }

    private static string? AsNullableString(this BsonValue value) => value.IsString ? value.AsString : null;
}
