using System;
using System.Collections.Generic;
using System.Text;

namespace Zero99Lotto.SRC.Common.Bus
{
    public class EventBusSettings
    {
        public string SubscriptionClientName { get; set; }
        public string EventBusConnection { get; set; }
        public int EventBusRetryCount { get; set; }
        public string EventBusUserName { get; set; }
        public string EventBusPassword { get; set; }
        public bool AzureServiceBusEnabled { get; set; }
        public string ExchangeName { get; set; }
        public bool LogMessages { get; set; }
        public string VirtualHost { get; set; }
    }
}
