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
    
    public async Task<User?> GetUser(string arg) { 
        if (arg.All(char.IsDigit)) { 
            return await _usersCollection.Find(user => user.xuid == arg).FirstOrDefaultAsync();
        }
        return await _usersCollection.Find(user => user.username == arg).FirstOrDefaultAsync();
    }

    public async Task CreateUser(User user) =>
        await _usersCollection.InsertOneAsync(user);
    
    public async Task UpdateUser(User user) {
        await _usersCollection.ReplaceOneAsync(x => x.xuid == user.xuid, user);
    }
}