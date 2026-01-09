using MongoDB.Driver;
using InventarisKKP.Models;

namespace InventarisKKP.Services
{
    /// <summary>
    /// Service untuk koneksi dan operasi MongoDB
    /// </summary>
    public class MongoDbService
    {
        private readonly IMongoDatabase _database;

        public MongoDbService(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("MongoDB") ?? "mongodb://localhost:27017";
            var mongoClient = new MongoClient(connectionString);
            _database = mongoClient.GetDatabase("InventarisKKP");
        }

        public IMongoCollection<MongoKategori> Kategoris => 
            _database.GetCollection<MongoKategori>("Kategoris");

        public IMongoCollection<MongoBarang> Barangs => 
            _database.GetCollection<MongoBarang>("Barangs");
    }
}
