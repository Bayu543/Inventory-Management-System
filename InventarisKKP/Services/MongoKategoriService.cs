using MongoDB.Driver;
using InventarisKKP.Models;

namespace InventarisKKP.Services
{
    public class MongoKategoriService : IMongoKategoriService
    {
        private readonly IMongoCollection<MongoKategori> _kategoris;

        public MongoKategoriService(MongoDbService mongoDbService)
        {
            _kategoris = mongoDbService.Kategoris;
        }

        public async Task<List<MongoKategori>> GetAllAsync()
        {
            return await _kategoris.Find(_ => true)
                .SortBy(k => k.NamaKategori)
                .ToListAsync();
        }

        public async Task<MongoKategori?> GetByIdAsync(string id)
        {
            return await _kategoris.Find(k => k.Id == id).FirstOrDefaultAsync();
        }

        public async Task<MongoKategori?> GetByKategoriIdAsync(int kategoriId)
        {
            return await _kategoris.Find(k => k.KategoriId == kategoriId).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(MongoKategori kategori)
        {
            kategori.CreatedAt = DateTime.Now;
            await _kategoris.InsertOneAsync(kategori);
        }

        public async Task UpdateAsync(string id, MongoKategori kategori)
        {
            kategori.UpdatedAt = DateTime.Now;
            await _kategoris.ReplaceOneAsync(k => k.Id == id, kategori);
        }

        public async Task DeleteAsync(string id)
        {
            await _kategoris.DeleteOneAsync(k => k.Id == id);
        }

        public async Task<bool> ExistsAsync(string namaKategori)
        {
            var filter = Builders<MongoKategori>.Filter.Regex(
                k => k.NamaKategori, 
                new MongoDB.Bson.BsonRegularExpression($"^{namaKategori}$", "i")
            );
            return await _kategoris.Find(filter).AnyAsync();
        }

        public async Task<int> GetNextKategoriIdAsync()
        {
            var lastKategori = await _kategoris.Find(_ => true)
                .SortByDescending(k => k.KategoriId)
                .FirstOrDefaultAsync();
            
            return lastKategori != null ? lastKategori.KategoriId + 1 : 1;
        }
    }
}
