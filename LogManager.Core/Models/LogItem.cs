namespace LogManager.Core.Models
{
    public class LogItem
    {
        public string id { get; set; }

        public string state { get; set; }

        public string type { get; set; }

        public string host { get; set; }

        public long timestamp { get; set; }
    }
}
