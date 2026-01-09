using MongoDB.Driver;
using InventarisKKP.Models;

namespace InventarisKKP.Services
{
    public class MongoBarangService : IMongoBarangService
    {
        private readonly IMongoCollection<MongoBarang> _barangs;

        public MongoBarangService(MongoDbService mongoDbService)
        {
            _barangs = mongoDbService.Barangs;
        }

        public async Task<List<MongoBarang>> GetAllAsync()
        {
            return await _barangs.Find(_ => true)
                .SortBy(b => b.NamaBarang)
                .ToListAsync();
        }

        public async Task<MongoBarang?> GetByIdAsync(string id)
        {
            return await _barangs.Find(b => b.Id == id).FirstOrDefaultAsync();
        }

        public async Task<MongoBarang?> GetByBarangIdAsync(int barangId)
        {
            return await _barangs.Find(b => b.BarangId == barangId).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(MongoBarang barang)
        {
            barang.CreatedAt = DateTime.Now;
            await _barangs.InsertOneAsync(barang);
        }

        public async Task UpdateAsync(string id, MongoBarang barang)
        {
            barang.UpdatedAt = DateTime.Now;
            await _barangs.ReplaceOneAsync(b => b.Id == id, barang);
        }

        public async Task DeleteAsync(string id)
        {
            await _barangs.DeleteOneAsync(b => b.Id == id);
        }

        public async Task<int> GetNextBarangIdAsync()
        {
            var lastBarang = await _barangs.Find(_ => true)
                .SortByDescending(b => b.BarangId)
                .FirstOrDefaultAsync();
            
            return lastBarang != null ? lastBarang.BarangId + 1 : 1;
        }

        public async Task<long> CountByKategoriIdAsync(int kategoriId)
        {
            return await _barangs.CountDocumentsAsync(b => b.KategoriId == kategoriId);
        }
    }
}
