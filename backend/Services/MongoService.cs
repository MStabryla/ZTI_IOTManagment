using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;

namespace SysOT.Services
{
    public interface IMongoService
    {
        void InsertCollection(string collectionName);
        void InsertDocument<T>(string collectionName, T document);
        void InsertDocuments<T>(string collectionName, IEnumerable<T> document);
        IEnumerable<T> GetDocuments<T>(string collectionName, BsonDocument queryObject);

        Task InsertCollectionAsync(string collectionName);
        Task InsertDocumentAsync<T>(string collectionName, T document);
        Task InsertDocumentsAsync<T>(string collectionName, IEnumerable<T> document);
        Task<IEnumerable<T>> GetDocumentsAsync<T>(string collectionName, BsonDocument queryObject);
        Task<IEnumerable<T>> GetDocumentsAsync<T>(string collectionName, FilterDefinition<T> filter);
        Task<IEnumerable<T>> GetDocumentsAsync<T>(string collectionName, Expression<Func<T,bool>> filter);
        Task<long> UpdateDocuments<T>(string collectionName, Expression<Func<T,bool>> filter, T setObject);
        Task<long> RemoveDocuments<T>(string collectionName, Expression<Func<T,bool>> filter);
    }

    public class MongoService : IMongoService
    {
        private readonly string _connectionString;
        private MongoClient client;
        private IMongoDatabase database;
        public MongoService(IConfiguration configuration)
        {
            _connectionString = configuration["Database:ConnectionString"];
            client = new MongoClient(_connectionString);
            database = client.GetDatabase(configuration["Database:DatabaseName"]);
        }

        public IEnumerable<T> GetDocuments<T>(string collectionName, BsonDocument queryObject)
        {
            var collection = database.GetCollection<T>(collectionName);
            var results = collection.Find<T>(queryObject);
            return results.ToList();
        }

        public async Task<IEnumerable<T>> GetDocumentsAsync<T>(string collectionName, BsonDocument queryObject)
        {
            var collection = database.GetCollection<T>(collectionName);
            var results = await collection.FindAsync<T>(queryObject);
            return results.ToList();
        }
        public async Task<IEnumerable<T>> GetDocumentsAsync<T>(string collectionName, FilterDefinition<T> filter)
        {
            var collection = database.GetCollection<T>(collectionName);
            var results = await collection.FindAsync<T>(filter);
            return results.ToList();
        }
        public async Task<IEnumerable<T>> GetDocumentsAsync<T>(string collectionName, Expression<Func<T,bool>> filter)
        {
            var collection = database.GetCollection<T>(collectionName);
            var results = await collection.FindAsync<T>(filter);
            return results.ToList();
        }

        public void InsertCollection(string collectionName){
            database.CreateCollection(collectionName);
        }

        public async Task InsertCollectionAsync(string collectionName){
            await database.CreateCollectionAsync(collectionName);
        }

        public void InsertDocument<T>(string collectionName, T document)
        {
            var collection = database.GetCollection<T>(collectionName);
            collection.InsertOne(document);
        }

        public async Task InsertDocumentAsync<T>(string collectionName, T document)
        {
            var collection = database.GetCollection<T>(collectionName);
            await collection.InsertOneAsync(document);
        }

        public void InsertDocuments<T>(string collectionName, IEnumerable<T> document)
        {
            var collection = database.GetCollection<T>(collectionName);
            collection.InsertMany(document);
        }

        public async Task InsertDocumentsAsync<T>(string collectionName, IEnumerable<T> document)
        {
            var collection = database.GetCollection<T>(collectionName);
            await collection.InsertManyAsync(document);
        }

        public async Task<long> RemoveDocuments<T>(string collectionName, Expression<Func<T,bool>> filter)
        {
            var collection = database.GetCollection<T>(collectionName);
            var result = await collection.DeleteManyAsync<T>(filter);
            return result.DeletedCount;
        }

        public async Task<long> UpdateDocuments<T>(string collectionName, Expression<Func<T,bool>> filter, T setObject)
        {
            var collection = database.GetCollection<T>(collectionName);
            var type = setObject.GetType();
            UpdateDefinition<T> uDef = null;
            foreach(var field in type.GetProperties())
            {
                if(field.Name == "Id")
                    continue;
                var value = field.GetValue(setObject);
                if(uDef == null)
                    uDef = Builders<T>.Update.Set(field.Name,value);
                else
                    uDef = uDef.Set(field.Name,value);
            }
            var result = await collection.UpdateManyAsync(filter,uDef);
            return result.ModifiedCount;
        }
    }
}