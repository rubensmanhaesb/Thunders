using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TarefasApp.Infra.Storage.Collections;
using TarefasApp.Infra.Storage.Settings;

namespace TarefasApp.Infra.Storage.Contexts
{

    public class MongoDBContext
    {
        private readonly MongoDBSettings _mongoDBSettings;

        public MongoDBContext(MongoDBSettings mongoDBSettings)
        {
            _mongoDBSettings = mongoDBSettings;
            Configure();
        }

        private IMongoDatabase? _mongoDatabase;

        private void Configure()
        {

            var mongoClientSettings = MongoClientSettings.FromUrl(new MongoUrl(_mongoDBSettings?.Host));


            if (_mongoDBSettings.IsSSL)
                mongoClientSettings.SslSettings = new SslSettings
                {
                    EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12
                };


            var mongoClient = new MongoClient(mongoClientSettings);
            _mongoDatabase = mongoClient.GetDatabase(_mongoDBSettings.Database);
        }


        public IMongoCollection<TarefaCollection> Tarefa
            => _mongoDatabase.GetCollection<TarefaCollection>("Tarefa");
    }
}
