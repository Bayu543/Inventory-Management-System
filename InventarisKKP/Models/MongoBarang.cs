using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace InventarisKKP.Models
{
    /// <summary>
    /// Model MongoDB untuk Barang
    /// </summary>
    public class MongoBarang
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("barangId")]
        public int BarangId { get; set; }

        [BsonElement("namaBarang")]
        [Required(ErrorMessage = "Nama barang wajib diisi")]
        [StringLength(200, ErrorMessage = "Nama barang maksimal 200 karakter")]
        public string NamaBarang { get; set; } = string.Empty;

        [BsonElement("kategoriId")]
        [Required(ErrorMessage = "Kategori wajib dipilih")]
        public int KategoriId { get; set; }

        [BsonElement("namaKategori")]
        public string NamaKategori { get; set; } = string.Empty;

        [BsonElement("stok")]
        [Range(0, int.MaxValue, ErrorMessage = "Stok tidak boleh negatif")]
        public int Stok { get; set; } = 0;

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [BsonElement("updatedAt")]
        public DateTime? UpdatedAt { get; set; }
    }
}
