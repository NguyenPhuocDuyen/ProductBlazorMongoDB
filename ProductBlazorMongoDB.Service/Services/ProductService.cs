using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using ProductBlazorMongoDB.Service.Models;

namespace ProductBlazorMongoDB.Service.Services
{
    public class ProductService : IProductService
    {
        private readonly IMongoCollection<Product> _productCollection;

        public ProductService(IOptions<DatabaseSettings> dbSettings)
        {
            var mongoClient = new MongoClient(dbSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(dbSettings.Value.DatabaseName);
            _productCollection = mongoDatabase.GetCollection<Product>(dbSettings.Value.ProductsCollectionName);
        }


        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            // Define the pipeline for the aggregation query
            var pipeline = new BsonDocument[]
            {
              new("$lookup", new BsonDocument
              {
                { "from", "CategoryCollection" },
                { "localField", "CategoryId" },
                { "foreignField", "_id" },
                { "as", "product_category" }
              }),
              new("$unwind", "$product_category"),
              new("$project", new BsonDocument
              {
                { "_id", 1 },
                { "CategoryId", 1},
                { "ProductName",1 },
                { "CategoryName", "$product_category.CategoryName" }
              })
            };

            var results = await _productCollection.Aggregate<Product>(pipeline).ToListAsync();

            return results;
        }

        public async Task<Product> GetById(string id) =>
            await _productCollection.Find(a => a.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Product Product) =>
            await _productCollection.InsertOneAsync(Product);

        public async Task UpdateAsync(string id, Product Product) 
            => await _productCollection.ReplaceOneAsync(a => a.Id == id, Product);

        public async Task DeleteAsync(string id) =>
            await _productCollection.DeleteOneAsync(a => a.Id == id);
    }
}
