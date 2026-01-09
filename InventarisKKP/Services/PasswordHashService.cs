using BCrypt.Net;

namespace InventarisKKP.Services
{
    /// <summary>
    /// Service untuk hashing dan verifikasi password
    /// </summary>
    public static class PasswordHashService
    {
        /// <summary>
        /// Hash password menggunakan BCrypt
        /// </summary>
        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);
        }

        /// <summary>
        /// Verifikasi password dengan hash
        /// </summary>
        public static bool VerifyPassword(string password, string hash)
        {
            try
            {
                return BCrypt.Net.BCrypt.Verify(password, hash);
            }
            catch
            {
                return false;
            }
        }
    }
}
