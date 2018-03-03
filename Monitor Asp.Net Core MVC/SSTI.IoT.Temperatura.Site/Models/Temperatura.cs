using System;
using System.Linq;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace SSTI.IoT.Temperatura.Site.Models
{
    public class Temperatura
    {
        private readonly IMongoDatabase _database;
        private readonly string         _collectionName = "iot";

        public Temperatura()
        {
            var _client = new MongoClient("mongodb://localhost:27017");

            _database   =_client.GetDatabase("29aso");
        }

        [BsonIgnoreExtraElements]
        public class TemperaturaModel
        {
            [BsonId]
            public ObjectId Id          { get; set; }
            [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
            public DateTime DataHora    { get; set; }
            public float    Temperatura { get; set; }
        }
        public class TemperaturaViewModel
        {
            public TemperaturaViewModel()
            {
                Lista = new List<TemperaturaModel>();
            }

            public TemperaturaModel         Ultima  { get; set; }

            public List<TemperaturaModel>   Lista   { get; set; }
        }

        public TemperaturaViewModel Select()
        {
            var collection = _database.GetCollection<TemperaturaModel>(_collectionName);

            var result     = collection.Find (Builders<TemperaturaModel>.Filter.Empty)
                                       .Sort (Builders<TemperaturaModel>.Sort.Descending(f => f.DataHora))
                                       .Limit(11);

            var model = new TemperaturaViewModel();

            if (!result.Any())
            {
                return model;
            }

            var list  = result.ToList();

            model.Ultima   = new TemperaturaModel
            {
                Id          = list[0].Id,
                DataHora    = list[0].DataHora,
                Temperatura = list[0].Temperatura
            };

            model.Lista.AddRange(list.Skip(1));

            return model;
        }

        public bool Insert(TemperaturaModel modelo)
        {
            var collection  = _database.GetCollection<TemperaturaModel>(_collectionName);

            modelo.DataHora = DateTime.Now;

            collection.InsertOne(modelo);

            return true;
        }
    }
}