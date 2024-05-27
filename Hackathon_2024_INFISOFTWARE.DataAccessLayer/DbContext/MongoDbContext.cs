using Hackathon_2024_INFISOFTWARE.Domain.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Hackathon_2024_INFISOFTWARE.DataAccessLayer.DbContext
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IOptions<MongoDbSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            _database = client.GetDatabase(settings.Value.DatabaseName);
        }

        public IMongoCollection<Workflow> Workflows => _database.GetCollection<Workflow>("Workflows");

    }
}
