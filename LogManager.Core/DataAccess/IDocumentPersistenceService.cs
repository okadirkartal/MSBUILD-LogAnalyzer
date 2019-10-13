using LiteDB;
using LogManager.Core.Models;
using System.Collections.Generic;

namespace LogManager.Core.DataAccess
{
    public interface IDocumentPersistenceService
    {
        BsonValue SaveDocument(List<LogRow> data);

        BsonDocument GetDocument(string bsonId);
    }
}
