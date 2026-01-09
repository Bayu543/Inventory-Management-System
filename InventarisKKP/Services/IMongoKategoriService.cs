using InventarisKKP.Models;

namespace InventarisKKP.Services
{
    public interface IMongoKategoriService
    {
        Task<List<MongoKategori>> GetAllAsync();
        Task<MongoKategori?> GetByIdAsync(string id);
        Task<MongoKategori?> GetByKategoriIdAsync(int kategoriId);
        Task CreateAsync(MongoKategori kategori);
        Task UpdateAsync(string id, MongoKategori kategori);
        Task DeleteAsync(string id);
        Task<bool> ExistsAsync(string namaKategori);
        Task<int> GetNextKategoriIdAsync();
    }
}
