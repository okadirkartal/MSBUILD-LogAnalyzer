using LogManager.Core.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LogManager.Core.BLL
{
    public class LogParser : ILogParser
    {
        //Read all lines from logs.txt and convert to LogItem object
        public List<LogItem> GetJsonDataFromFile(string filePath)
        {
            List<LogItem> list = new List<LogItem>();

            try
            {
                using (StreamReader sr = File.OpenText(filePath))
                {
                    string s = String.Empty;
                    while ((s = sr.ReadLine()) != null)
                    {
                        list.Add(JsonConvert.DeserializeObject<LogItem>(s));
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return list;
        }

        //Find saveable records,calculate difference of milisecond for each related record
        public List<LogRow> GetSaveableData(List<LogItem> items)
        {
            var resultList = new List<LogRow>();

           try
            {
                foreach (var item in items)
                {
                    if (item.state.Equals("STARTED"))
                    {
                        var finishedItem = items.Where(x => x.id == item.id && x.state == "FINISHED").FirstOrDefault();

                        var differenceMiliSeconds = finishedItem.timestamp - item.timestamp;

                        resultList.Add(new LogRow
                        {
                            id=Guid.NewGuid().ToString().Replace("-",""),
                            EventId = item.id,
                            EventDuration = differenceMiliSeconds,
                            Host = item.host,
                            Type = item.type,
                            Alert = differenceMiliSeconds > 4
                        });
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return resultList;
        }
    }
}
