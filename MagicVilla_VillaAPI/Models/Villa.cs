using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MagicVilla_VillaAPI.Models
{
    [BsonIgnoreExtraElements]
    public class Villa
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;
        [Required]
        [MaxLength(30)]
        [BsonElement("Name")]
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        [BsonElement("Occupancy")]
        public int Occupancy { get; set; }
        [BsonElement("Sqft")]
        public int Sqft { get; set; }

    }
}
