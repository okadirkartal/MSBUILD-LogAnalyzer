using LogManager.Core.BLL;
using LogManager.Core.DataAccess;
using LogManager.Core.Models;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;

namespace LogManager.Tests
{
    public class LogManagerTests
    {
        private IConfiguration _configuration;

        private ILogParser _logParser;

        private IDocumentPersistenceService _documentPersistenceService;

        private string logFileDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");


        [SetUp]
        public void SetUp()
        {
            _configuration = Substitute.For<IConfiguration>();

            _configuration[Common.Constants.StorageConnectionString].Returns("FileName=Data/Documents.db;Timeout=10; Journal=false;Mode=Exclusive");
            _configuration[Common.Constants.LogFile].Returns("log.txt");

            if (!Directory.Exists(logFileDirectory))
                Directory.CreateDirectory(logFileDirectory);
        }

        [Test]
        public void ReadLogFile_WhenJsonDataExists_ReturnsTrue()
        {
           _logParser = new LogParser();

            var result = _logParser.GetJsonDataFromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _configuration[Common.Constants.LogFile]));

            Assert.True(result.Count > 0);
        }


        [Test]
        public void GetSaveableData_WhenDataExists_ReturnsTrue()
        {
            _logParser = new LogParser();
            var jsonData = _logParser.GetJsonDataFromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _configuration[Common.Constants.LogFile]));
            var result = _logParser.GetSaveableData(jsonData);

            Assert.True(result.Count > 0);
            Assert.Greater(jsonData.Count, result.Count);
        }


        [Test]
        public void SaveJsonData_WhenInserted_ReturnsTrue()
        {
            _documentPersistenceService = new DocumentPersistenceService(_configuration);

            var bsonDocumentId = _documentPersistenceService.SaveDocument(GetLogRows());

            Assert.NotNull(bsonDocumentId);
        }

        [TearDown]
        public void TearDown()
        {
            Directory.Delete(logFileDirectory, true);
        }

        private List<LogRow> GetLogRows()
        {
            return new List<LogRow>()
            {
                new LogRow(){ id=Guid.NewGuid().ToString().Replace("-",""), EventId="scsmbstgra", EventDuration=3,Host="some host", Type="some type", Alert=false },
                  new LogRow(){ id=Guid.NewGuid().ToString().Replace("-",""), EventId="scsmbstgrb", EventDuration=5,Host="some host", Type="some type", Alert=true },
                    new LogRow(){ id=Guid.NewGuid().ToString().Replace("-",""), EventId="scsmbstgrc", EventDuration=1,Host="some host", Type="some type", Alert=false }
            };
        }
    }
}
