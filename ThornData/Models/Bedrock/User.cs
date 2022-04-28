using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ThornData.Models.Bedrock; 

public class User {

    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public string xuid { get; set; }
    
    public string username { get; set; }
    
    public int? level { get; set; }
    
    public int? xp { get; set; }
    
}