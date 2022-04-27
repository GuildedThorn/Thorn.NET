using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ThornData.Models.Bedrock;

namespace ThornData.Services.Bedrock; 

public class UserService {
    
    private readonly IMongoCollection<User> _usersCollection;

    public UserService(IOptions<UsersDatabaseSettings> userSettings) {
        
        var client = new MongoClient(userSettings.Value.ConnectionString);
        var database = client.GetDatabase(userSettings.Value.DatabaseName);
        
        _usersCollection = database.GetCollection<User>(userSettings.Value.CollectionName);
    }

    public async Task<List<User>> GetAll() =>
        await _usersCollection.Find(user => true).ToListAsync();

    public async Task<User> GetUserByXuid(string xuid) { 
        return await _usersCollection.Find(user => user.xuid == xuid).FirstOrDefaultAsync();
    }

    public async Task<User> GetUserByUsername(string username) {
        return await _usersCollection.Find(user => user.username == username).FirstOrDefaultAsync();
    }

    public async Task CreateUser(User user) =>
        await _usersCollection.InsertOneAsync(user);
    
    public async Task UpdateUserByXuid(string xuid, User user) =>
        await _usersCollection.ReplaceOneAsync(x => x.xuid == xuid, user);
    
    public async Task UpdateUserByUsername(string username, User user) =>
        await _usersCollection.ReplaceOneAsync(x => x.username == username, user);

}