using InventarisKKP.Models;

namespace InventarisKKP.Services
{
    public interface IMongoBarangService
    {
        Task<List<MongoBarang>> GetAllAsync();
        Task<MongoBarang?> GetByIdAsync(string id);
        Task<MongoBarang?> GetByBarangIdAsync(int barangId);
        Task CreateAsync(MongoBarang barang);
        Task UpdateAsync(string id, MongoBarang barang);
        Task DeleteAsync(string id);
        Task<int> GetNextBarangIdAsync();
        Task<long> CountByKategoriIdAsync(int kategoriId);
    }
}
