using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventarisKKP.Models
{
    /// <summary>
    /// Model untuk tabel BarangMasuk (transaksi barang masuk)
    /// </summary>
    public class BarangMasuk
    {
        [Key]
        public int MasukId { get; set; }

        [Required(ErrorMessage = "Barang wajib dipilih")]
        public int BarangId { get; set; }

        [Required(ErrorMessage = "Jumlah wajib diisi")]
        [Range(1, int.MaxValue, ErrorMessage = "Jumlah harus lebih dari 0")]
        public int Jumlah { get; set; }

        [Required]
        public DateTime TanggalMasuk { get; set; } = DateTime.Now;

        // Foreign Key Navigation
        [ForeignKey("BarangId")]
        public virtual Barang? Barang { get; set; }
    }
}