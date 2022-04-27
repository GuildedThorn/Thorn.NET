using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ThornData.Models.Bedrock; 

public class Faction {
    
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public string? id { get; set; }
    
    public string? name { get; set; }

    public int? level { get; set; }

    public int? xp { get; set; }
    
}