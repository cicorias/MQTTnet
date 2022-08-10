using MQTTnet;
using MQTTnet.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQTTRelayApp
{
    internal class Server
    {
        private Server() { }
        private static readonly object _lock = new();
        private static Server? instance = null;
        public static Server Instance
        {
            get
            {
                lock (_lock)
                {
                    if (null == instance)
                    {
                        instance = new Server();
                    }
                    return instance;
                }
            }
        }

        public static async Task Run_Minimal_Server()
        {
            /*
             * This sample starts a simple MQTT server which will accept any TCP connection.
             */

            var mqttFactory = new MqttFactory(new ConsoleLogger());

            // The port for the default endpoint is 1883.
            // The default endpoint is NOT encrypted!
            // Use the builder classes where possible.
            var mqttServerOptions = new MqttServerOptionsBuilder().WithDefaultEndpoint().Build();

            // The port can be changed using the following API (not used in this example).
            // new MqttServerOptionsBuilder()
            //     .WithDefaultEndpoint()
            //     .WithDefaultEndpointPort(1234)
            //     .Build();

            using (var mqttServer = mqttFactory.CreateMqttServer(mqttServerOptions))
            {
                await mqttServer.StartAsync();

                Console.WriteLine("Press Enter to exit.");
                Console.ReadLine();

                // Stop and dispose the MQTT server if it is no longer needed!


            }
        }
    }
}