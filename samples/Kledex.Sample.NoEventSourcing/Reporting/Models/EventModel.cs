using System;
using System.Collections.Generic;

namespace Kledex.Sample.NoEventSourcing.Reporting.Models
{
    public class EventModel
    {
        public string Type { get; set; }
        public Dictionary<string, string> Data { get; set; } = new Dictionary<string, string>();
        public DateTime TimeStamp { get; set; }
    }
}
