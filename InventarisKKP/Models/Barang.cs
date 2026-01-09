using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventarisKKP.Models
{
    /// <summary>
    /// Model untuk tabel Barang
    /// </summary>
    public class Barang
    {
        [Key]
        public int BarangId { get; set; }

        [Required(ErrorMessage = "Nama barang wajib diisi")]
        [StringLength(200, ErrorMessage = "Nama barang maksimal 200 karakter")]
        public string NamaBarang { get; set; } = string.Empty;

        [Required(ErrorMessage = "Kategori wajib dipilih")]
        public int KategoriId { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Stok tidak boleh negatif")]
        public int Stok { get; set; } = 0;

        // Foreign Key Navigation
        [ForeignKey("KategoriId")]
        public virtual Kategori? Kategori { get; set; }

        // Navigation properties untuk relasi dengan transaksi
        public virtual ICollection<BarangMasuk> BarangMasuks { get; set; } = new List<BarangMasuk>();
        public virtual ICollection<BarangKeluar> BarangKeluars { get; set; } = new List<BarangKeluar>();
    }
}