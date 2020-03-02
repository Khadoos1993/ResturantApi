using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using ResturantApi;
using System;
using System.Collections.Generic;
using System.Text;

namespace ResturantDAL
{
    public class MongoDbDataContext : IMongoDbDataContext
    {
        private readonly IMongoCollection<Resturant> _resturant;
        private string _connectionStrings = string.Empty;
        private string _databaseName = string.Empty;
        private string _collectionName = string.Empty;

        public MongoDbDataContext(IOptions<AppSettings> options)
        {
            _collectionName = options.Value.CollectionName;
            _connectionStrings = options.Value.ServerName;
            _databaseName = options.Value.DatabaseName;
            Client = new MongoClient(_connectionStrings);
            Database = Client.GetDatabase(_databaseName);
            _resturant = Database.GetCollection<Resturant>(_collectionName);
        }

        public IMongoClient Client { get; }

        public IMongoDatabase Database { get; }

        public IMongoCollection<Resturant> GetResturant
        {
            get { return Database.GetCollection<Resturant>(_collectionName); }
        }

        public List<Resturant> Get() =>
            _resturant.Find(book => true).ToList();

        public Resturant Get(ObjectId id) =>
            _resturant.Find(book => book.ObjectId == id).FirstOrDefault();

        public Resturant Create(Resturant resturant)
        {
            _resturant.InsertOne(resturant);
            return resturant;
        }
        public void Update(ObjectId id, Resturant resturant) =>
            _resturant.ReplaceOne(r => r.ObjectId == id, resturant);

        public void Remove(Resturant resturant) =>
            _resturant.DeleteOne(r => resturant.ObjectId == resturant.ObjectId);

        public void Remove(ObjectId id) =>
            _resturant.DeleteOne(r => r.ObjectId == id);
    }

}
