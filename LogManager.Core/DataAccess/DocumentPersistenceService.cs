using LiteDB;
using LogManager.Core.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace LogManager.Core.DataAccess
{
    public class DocumentPersistenceService : IDocumentPersistenceService
    {
        private readonly IConfiguration _configuration;

        private readonly string _liteDbConnectionString;

        public DocumentPersistenceService(IConfiguration configuration)
        {
            this._configuration = configuration;
            _liteDbConnectionString = _configuration[Common.Constants.StorageConnectionString];
        }

        public BsonValue SaveDocument(List<LogRow> data)
        {
            using var db = new LiteDatabase(_liteDbConnectionString);
            var col = db.GetCollection<LogRow>(nameof(LogRow));
                 
            return col.InsertBulk(data.ToArray());
        }

        public BsonDocument GetDocument(string bsonId)
        {
            using var db = new LiteDatabase(_liteDbConnectionString);
            return db.Engine.FindById(nameof(LogRow), bsonId);
        }
    }
}
