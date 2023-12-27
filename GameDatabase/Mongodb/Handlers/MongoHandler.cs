using System.Collections.Generic;
using GameDatabase.Mongodb.Interfaces;
using MongoDB.Driver;

namespace GameDatabase.Mongodb.Handlers;

public class MongoHandler<T> : IGameDB<T> where T : class
{
    private IMongoDatabase _database;
    private IMongoCollection<T> Collection{get;set;}
    public MongoHandler(IMongoDatabase database)
    {
        _database = database;
        this.SetCollection();// = _database.GetCollection<T>(name:"Users");
    }

    // public T Create(T item)
    // {
    //     _collection.InsertOne(item);
    //     return item;
    // }

    public T Get(FilterDefinition<T> filter)
    {
        return Collection.Find(filter).FirstOrDefault();
    }
    public List<T> GetAll()
    {
        var filter = Builders<T>.Filter.Empty;
        return Collection.Find(filter).ToList();    
    }
    private void SetCollection(){
        switch (typeof(T).Name){
            case "User":
                Collection = _database.GetCollection<T>(name:"Users");
                break;
            case "Room":
                break;
        }
    }
    public IMongoCollection<T> GetCollection(string name)
    {
        throw new NotImplementedException();
    }

    public IMongoDatabase GetDatabase()
    {
        return _database;
    }

    public void Remove(FilterDefinition<T> filter)
    {
        throw new NotImplementedException();
    }

    public T Update(FilterDefinition<T> filter, UpdateDefinition<T> updater)
    {
        throw new NotImplementedException();
    }

    public T Create(T item)
    {
        Collection.InsertOne(item);
        return item;
    }
}
