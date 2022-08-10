using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQTTRelayApp.Models
{
    internal class PublishItem
    {
        string? _contentType;
        uint _messageExpiryInterval;
        string _name = string.Empty;
        string? _payload;
        string? _responseTopic;
        bool _retain;
        uint _subscriptionIdentifier;
        string? _topic;
        ushort _topicAlias;


        public QualityOfService QualityOfServiceLevel{ get; } = new();
        public PayloadFormat PayloadFormatIndicator { get; } = new();
        public string? ContentType { get => _contentType; set => _contentType = value; }
        public uint MessageExpiryInterval { get => _messageExpiryInterval; set => _messageExpiryInterval = value; }
        public string Name { get => _name; set => _name = value; }
        public string? Payload { get => _payload; set => _payload = value; }
        public string? ResponseTopic { get => _responseTopic; set => _responseTopic = value; }
        public bool Retain { get => _retain; set => _retain = value; }
        public uint SubscriptionIdentifier { get => _subscriptionIdentifier; set => _subscriptionIdentifier = value; }
        public string? Topic { get => _topic; set => _topic = value; }
        public ushort TopicAlias { get => _topicAlias; set => _topicAlias = value; }
    }
}
