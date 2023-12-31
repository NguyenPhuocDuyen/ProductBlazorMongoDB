﻿using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ProductBlazorMongoDB.Service.Models;

namespace ProductBlazorMongoDB.Service.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IMongoCollection<Category> _categoryCollection;

        public CategoryService(IOptions<DatabaseSettings> dbSettings)
        {
            var mongoClient = new MongoClient(dbSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase (dbSettings.Value.DatabaseName);
            _categoryCollection = mongoDatabase.GetCollection<Category>(dbSettings.Value.CategoriesCollectionName);
        }

        public async Task<IEnumerable<Category>> GetAllAsync() =>
            await _categoryCollection.Find(_ => true).ToListAsync();

        public async Task<Category?> GetById(string id) =>
            await _categoryCollection.Find(a => a.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Category Category) =>
            await _categoryCollection.InsertOneAsync(Category);

        public async Task UpdateAsync(string id, Category Category) =>
            await _categoryCollection
            .ReplaceOneAsync(a => a.Id == id, Category);

        public async Task DeleteAsync(string id) =>
            await _categoryCollection.DeleteOneAsync(a => a.Id == id);
    }
}
