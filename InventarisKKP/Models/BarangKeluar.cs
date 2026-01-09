using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventarisKKP.Models
{
    /// <summary>
    /// Model untuk tabel BarangKeluar (transaksi barang keluar)
    /// </summary>
    public class BarangKeluar
    {
        [Key]
        public int KeluarId { get; set; }

        [Required(ErrorMessage = "Barang wajib dipilih")]
        public int BarangId { get; set; }

        [Required(ErrorMessage = "Jumlah wajib diisi")]
        [Range(1, int.MaxValue, ErrorMessage = "Jumlah harus lebih dari 0")]
        public int Jumlah { get; set; }

        [Required]
        public DateTime TanggalKeluar { get; set; } = DateTime.Now;

        // Foreign Key Navigation
        [ForeignKey("BarangId")]
        public virtual Barang? Barang { get; set; }
    }
}