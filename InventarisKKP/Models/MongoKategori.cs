using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace InventarisKKP.Models
{
    /// <summary>
    /// Model MongoDB untuk Kategori
    /// </summary>
    public class MongoKategori
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("kategoriId")]
        public int KategoriId { get; set; }

        [BsonElement("namaKategori")]
        [Required(ErrorMessage = "Nama kategori wajib diisi")]
        [StringLength(100, ErrorMessage = "Nama kategori maksimal 100 karakter")]
        public string NamaKategori { get; set; } = string.Empty;

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [BsonElement("updatedAt")]
        public DateTime? UpdatedAt { get; set; }
    }
}
