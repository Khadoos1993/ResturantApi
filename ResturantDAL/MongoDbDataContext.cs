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

        }

        public IMongoClient Client { get; }

        public IMongoDatabase Database { get; }

        public IMongoCollection<Resturant> GetResturant
        {
            get { return Database.GetCollection<Resturant>(_collectionName); }
        }

    }
}
