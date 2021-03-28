using System.Collections.Generic;

namespace TimeTracking.Common.RabbitMq
{
    public class RabbitMqConfiguration
    {
        public string Host { get; set; }
        public ushort Port { get; set; }
        public string VirtualHost { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool PublisherConfirmation { get; set; }
        public IEnumerable<string> ClusterMembers { get; set; }
        public bool PurgeOnStartup { get; set; }
        public ushort PrefetchCount { get; set; }
        /// <summary>
        /// The queue endpoint for the current service
        /// </summary>
        public string Endpoint { get; set; }
        public bool DurableQueue { get; set; }

    }

    public class RabbitMqOptions : RabbitMqConfiguration
    {
        public string Namespace { get; set; }
        public int Retries { get; set; }
        public int RetryInterval { get; set; }
    }
}