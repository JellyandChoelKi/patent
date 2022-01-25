using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;

namespace K2GGTT.Utils
{
    public class MongoDBConf
    {
        private readonly IMongoDatabase db;

        public MongoDBConf()
        {
            var client = new MongoClient(@"mongodb://k2ggttmongodb:eb8VcsPlXWexzkutuFPwdPJZ4DcE0TcUEWG7xtO1SLVJ908UKjg49tJf96g5Jt8YgBQ7eAX9Jpg4aZka3E1zFg==@k2ggttmongodb.mongo.cosmos.azure.com:10255/?ssl=true&retrywrites=false&replicaSet=globaldb&maxIdleTimeMS=120000&appName=@k2ggttmongodb@");
            db = client.GetDatabase("k2g_gtt");
        }

        // insert
        public void InsertRecord<T>(string table, T record)
        {
            var collection = db.GetCollection<T>(table);
            collection.InsertOne(record);
        }

        // select list
        public List<T> LoadRecord<T>(string table)
        {
            var collection = db.GetCollection<T>(table);
            return collection.Find(new BsonDocument()).ToList();
        }

        // select one field
        public T LoadRecordById<T>(string table, string col, string val)
        {
            var collection = db.GetCollection<T>(table);
            var filter = Builders<T>.Filter.Eq(col, val);
            return collection.Find(filter).First();
        }

        // select one field list
        public List<T> LoadRecordByIdList<T>(string table, string LogUUID)
        {
            var collection = db.GetCollection<T>(table);
            var filter = Builders<T>.Filter.Eq("LogUUID", LogUUID);
            return collection.Find(filter).ToList();
        }

        // insert, update
        public void UpsertRecord<T>(string table, ObjectId id, T record)
        {
            var collection = db.GetCollection<T>(table);
            collection.ReplaceOne(new BsonDocument("_id", id), record, new ReplaceOptions { IsUpsert = true });
        }

        // delete
        public void DeleteRecord<T>(string table, ObjectId id)
        {
            var collection = db.GetCollection<T>(table);
            var filter = Builders<T>.Filter.Eq("_id", id);
            collection.DeleteOne(filter);
        }
    }
}
