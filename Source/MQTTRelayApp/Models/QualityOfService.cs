using MQTTnet.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQTTRelayApp.Models
{
    internal class QualityOfService
    {
        bool _isLevel0 = true;
        bool _isLevel1;
        bool _isLevel2;

        public QualityOfService()
        {
            Value = MqttQualityOfServiceLevel.AtMostOnce;
        }

        public bool IsLevel0
        {
            get => _isLevel0;
            set => _isLevel0 = value;
        }

        public bool IsLevel1
        {
            get => _isLevel1;
            set => _isLevel1 = value;
        }

        public bool IsLevel2
        {
            get => _isLevel2;
            set => _isLevel2 = value;
        }

        public MqttQualityOfServiceLevel Value
        {
            get
            {
                if (IsLevel0)
                {
                    return MqttQualityOfServiceLevel.AtMostOnce;
                }

                if (IsLevel1)
                {
                    return MqttQualityOfServiceLevel.AtLeastOnce;
                }

                if (IsLevel2)
                {
                    return MqttQualityOfServiceLevel.ExactlyOnce;
                }

                throw new NotSupportedException();
            }

            set
            {
                IsLevel0 = value == MqttQualityOfServiceLevel.AtMostOnce;
                IsLevel1 = value == MqttQualityOfServiceLevel.AtLeastOnce;
                IsLevel2 = value == MqttQualityOfServiceLevel.ExactlyOnce;
            }
        }
    }
}
