using System.ComponentModel.DataAnnotations;

namespace InventarisKKP.Models
{
    /// <summary>
    /// Model untuk tabel Kategori barang
    /// </summary>
    public class Kategori
    {
        [Key]
        public int KategoriId { get; set; }

        [Required(ErrorMessage = "Nama kategori wajib diisi")]
        [StringLength(100, ErrorMessage = "Nama kategori maksimal 100 karakter")]
        public string NamaKategori { get; set; } = string.Empty;

        // Navigation property untuk relasi one-to-many dengan Barang
        public virtual ICollection<Barang> Barangs { get; set; } = new List<Barang>();
    }
}