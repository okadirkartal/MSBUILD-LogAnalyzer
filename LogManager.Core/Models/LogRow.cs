namespace LogManager.Core.Models
{
    public class LogRow
    {
        public string id { get; set; }
        public string EventId { get; set; }
        public long EventDuration { get; set; }
        public string Type { get; set; }
        public string Host { get; set; }
        public bool Alert { get; set; }
    }
}
