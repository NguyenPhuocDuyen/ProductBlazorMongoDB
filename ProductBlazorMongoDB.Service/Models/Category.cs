﻿using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ProductBlazorMongoDB.Service.Models
{
    public class Category
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string? CategoryName { get; set; }
    }
}
