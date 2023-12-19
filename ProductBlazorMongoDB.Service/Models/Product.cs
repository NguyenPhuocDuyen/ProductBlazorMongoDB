using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ProductBlazorMongoDB.Service.Models
{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string? ProductName { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string? CategoryId { get; set; }

        public string? CategoryName { get; set; }
    }
}
