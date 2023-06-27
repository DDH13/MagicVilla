using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MagicVilla_VillaAPI.Models;

[BsonIgnoreExtraElements]
public class RichGuy
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(30)]
    [BsonElement("name")]
    public string Name { get; set; }

    [BsonElement("netWorth")] 
    public int netWorth { get; set; }
}