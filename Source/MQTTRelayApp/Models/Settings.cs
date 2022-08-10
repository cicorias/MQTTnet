using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQTTRelayApp.Models
{
    public sealed class Settings
    {
        public string? Hostname { get; set; }
        public string? SasToken { get; set; }
        public string? DeviceId { get; set; }
        public int Port { get; set; }
        public string? ApiVersion { get; set; }
    }
}
