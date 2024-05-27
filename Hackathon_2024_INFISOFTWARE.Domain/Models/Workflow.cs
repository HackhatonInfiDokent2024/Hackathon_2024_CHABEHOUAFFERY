using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Hackathon_2024_INFISOFTWARE.Domain.Models
{
    public class Workflow
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public Dictionary<string, Etape> ProcessusDemandeFormation { get; set; }
    }
}
