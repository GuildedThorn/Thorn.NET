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
    
    public async Task<Faction> GetFactionById(string id) =>
        await _factionsCollection.Find(faction => faction.id == id).FirstOrDefaultAsync();

    public async Task<Faction> GetFactionByName(string name) =>
        await _factionsCollection.Find(faction => faction.name == name).FirstOrDefaultAsync();

    public async Task CreateFaction(Faction faction) =>
        await _factionsCollection.InsertOneAsync(faction);
    
    public async Task UpdateFactionById(string id, Faction faction) =>
        await _factionsCollection.ReplaceOneAsync(x => x.id == id, faction);
    
    public async Task UpdateFactionByName(string name, Faction faction) =>
        await _factionsCollection.ReplaceOneAsync(x => x.name == name, faction);

    public async Task DeleteFactionById(string id) =>
        await _factionsCollection.DeleteOneAsync(x => x.id == id);
    
    public async Task DeleteFactionByName(string name) =>
        await _factionsCollection.DeleteOneAsync(x => x.name == name);
    
}