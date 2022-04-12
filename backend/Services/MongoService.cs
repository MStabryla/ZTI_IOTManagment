using System.Collections.Generic;
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
        Task UpdateDocuments(string collectionName, object queryObject,object setObject);
        Task RemoveDocuments(string collectionName, object queryObject);
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
            return results.ToEnumerable();
        }

        public async Task<IEnumerable<T>> GetDocumentsAsync<T>(string collectionName, BsonDocument queryObject)
        {
            var collection = database.GetCollection<T>(collectionName);
            var results = await collection.FindAsync<T>(queryObject);
            return results.ToEnumerable();
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

        public async Task RemoveDocuments(string collectionName, object queryObject)
        {
            await Task.Run(() => {});
            throw new System.NotImplementedException();
        }

        public async Task UpdateDocuments(string collectionName, object queryObject, object setObject)
        {
            await Task.Run(() => {});
            throw new System.NotImplementedException();
        }
    }
}