using LogManager.Core.Models;
using System.Collections.Generic;

namespace LogManager.Core.BLL
{
    public interface ILogParser
    {
        List<LogItem> GetJsonDataFromFile(string filePath);

        List<LogRow> GetSaveableData(List<LogItem> items);
    }
}
