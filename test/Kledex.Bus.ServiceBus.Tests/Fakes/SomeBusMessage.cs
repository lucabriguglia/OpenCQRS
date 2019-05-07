using System;
using System.Collections.Generic;
using System.Text;

namespace Kledex.Bus.ServiceBus.Tests.Fakes
{
    internal class SomeBusMessage : IBusMessage
    {
        public DateTime? ScheduledEnqueueTimeUtc { get; set; }
        public string SessionId { get; set; }
        public string CorrelationId { get; set; }
        public IDictionary<string, object> UserProperties { get; set; }
        public string Label { get; set; }
    }
}
