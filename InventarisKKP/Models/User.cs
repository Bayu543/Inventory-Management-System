using System.ComponentModel.DataAnnotations;

namespace InventarisKKP.Models
{
    /// <summary>
    /// Model untuk tabel User (autentikasi)
    /// </summary>
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Username wajib diisi")]
        [StringLength(50, ErrorMessage = "Username maksimal 50 karakter")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password wajib diisi")]
        [StringLength(255, ErrorMessage = "Password maksimal 255 karakter")]
        public string Password { get; set; } = string.Empty; // Dalam production, gunakan hash

        [Required(ErrorMessage = "Nama lengkap wajib diisi")]
        [StringLength(100, ErrorMessage = "Nama lengkap maksimal 100 karakter")]
        public string NamaLengkap { get; set; } = string.Empty;

        [Required(ErrorMessage = "Role wajib diisi")]
        [StringLength(20, ErrorMessage = "Role maksimal 20 karakter")]
        public string Role { get; set; } = "User"; // Default role adalah User

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}