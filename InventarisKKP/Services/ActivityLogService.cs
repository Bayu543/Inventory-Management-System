using MongoDB.Driver;
using InventarisKKP.Models;

namespace InventarisKKP.Services
{
    /// <summary>
    /// Service untuk mengelola ActivityLog di MongoDB
    /// </summary>
    public class ActivityLogService : IActivityLogService
    {
        private readonly IMongoCollection<ActivityLog>? _activityLogs;

        public ActivityLogService(IConfiguration configuration)
        {
            try
            {
                // Disable MongoDB untuk sementara - fokus ke user management
                Console.WriteLine("MongoDB disabled - Activity logging will be skipped");
                _activityLogs = null;
                return;
                
                /* MongoDB connection code - disabled temporarily
                var connectionString = configuration.GetConnectionString("MongoDB");
                
                if (string.IsNullOrEmpty(connectionString))
                {
                    Console.WriteLine("MongoDB connection string not configured");
                    _activityLogs = null;
                    return;
                }
                
                var settings = MongoClientSettings.FromConnectionString(connectionString);
                settings.ConnectTimeout = TimeSpan.FromSeconds(2);
                settings.ServerSelectionTimeout = TimeSpan.FromSeconds(2);
                settings.SocketTimeout = TimeSpan.FromSeconds(2);
                
                var client = new MongoClient(settings);
                var database = client.GetDatabase("InventarisKKP");
                _activityLogs = database.GetCollection<ActivityLog>("ActivityLogs");
                
                // Test connection dengan timeout
                Task.Run(async () =>
                {
                    try
                    {
                        await database.ListCollectionNamesAsync(new ListCollectionNamesOptions(), 
                            new System.Threading.CancellationTokenSource(TimeSpan.FromSeconds(2)).Token);
                        Console.WriteLine("MongoDB connected successfully");
                    }
                    catch
                    {
                        Console.WriteLine("MongoDB connection test failed, but will continue");
                    }
                }).Wait(TimeSpan.FromSeconds(3));
                */
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Warning: MongoDB connection failed: {ex.Message}");
                Console.WriteLine("Activity logging will be disabled");
                _activityLogs = null;
            }
        }

        /// <summary>
        /// Menyimpan log aktivitas ke MongoDB
        /// </summary>
        public async Task LogActivityAsync(string user, string action, string description)
        {
            try
            {
                if (_activityLogs == null)
                {
                    Console.WriteLine("MongoDB not available, skipping activity log");
                    return;
                }

                var log = new ActivityLog
                {
                    User = user,
                    Action = action,
                    Description = description,
                    Timestamp = DateTime.Now
                };

                await _activityLogs.InsertOneAsync(log);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error menyimpan activity log: {ex.Message}");
            }
        }

        /// <summary>
        /// Mengambil log aktivitas terbaru
        /// </summary>
        public async Task<List<ActivityLog>> GetLogsAsync(int limit = 100)
        {
            try
            {
                if (_activityLogs == null)
                {
                    Console.WriteLine("MongoDB not available, returning empty logs");
                    return new List<ActivityLog>();
                }

                return await _activityLogs
                    .Find(_ => true)
                    .SortByDescending(log => log.Timestamp)
                    .Limit(limit)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error mengambil activity logs: {ex.Message}");
                return new List<ActivityLog>();
            }
        }
    }
}