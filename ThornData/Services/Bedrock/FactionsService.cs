using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ThornData.Models.Bedrock;

namespace ThornData.Services.Bedrock; 

public class FactionsService {

    private readonly IMongoCollection<Faction> _factionsCollection;

    public FactionsService(IOptions<FactionsDatabaseSettings> factionSettings) {
        
        var client = new MongoClient(factionSettings.Value.ConnectionString);
        var database = client.GetDatabase(factionSettings.Value.DatabaseName);

        _factionsCollection = database.GetCollection<Faction>(factionSettings.Value.CollectionName);
    }
    
    public async Task<List<Faction>> GetAll() =>
        await _factionsCollection.Find(faction => true).ToListAsync();

    public async Task<Faction?> GetFaction(string arg) {
        if (arg.All(char.IsDigit)) {
            return await _factionsCollection.Find(faction => faction.id == arg).FirstOrDefaultAsync();
        }
        return await _factionsCollection.Find(faction => faction.name == arg).FirstOrDefaultAsync();
    }

    public async Task CreateFaction(Faction faction) =>
        await _factionsCollection.InsertOneAsync(faction);

    public async Task UpdateFaction(Faction faction) =>
        await _factionsCollection.ReplaceOneAsync(x => x.id == faction.id, faction);
    
    public async Task DeleteFaction(string id) =>
        await _factionsCollection.DeleteOneAsync(x => x.id == id);

}