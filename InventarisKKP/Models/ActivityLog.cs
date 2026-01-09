using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace InventarisKKP.Models
{
    /// <summary>
    /// Model untuk MongoDB ActivityLog
    /// Digunakan untuk mencatat semua aktivitas user
    /// </summary>
    public class ActivityLog
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("user")]
        public string User { get; set; } = string.Empty;

        [BsonElement("action")]
        public string Action { get; set; } = string.Empty;

        [BsonElement("description")]
        public string Description { get; set; } = string.Empty;

        [BsonElement("timestamp")]
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}