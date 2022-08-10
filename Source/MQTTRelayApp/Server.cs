using Microsoft.Extensions.Configuration;
using MQTTnet;
using MQTTnet.Server;
using MQTTRelayApp.Models;
using System.Text;

namespace MQTTRelayApp
{
    internal class Server
    {
        IConfiguration _configuration;
        Settings? _settings;
        private Server()
        {
            //https://stackoverflow.com/questions/58105146/how-to-set-hosting-environment-name-for-net-core-console-app-using-generic-host
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.dev.json", true, true)
                .AddEnvironmentVariables()
                .Build();


            _settings = _configuration.GetRequiredSection("Settings").Get<Settings>();


            //_configuration = new ConfigurationManager();
            //_configuration.add
            //_apiVersion = _configuration["apiVersion"];
        }
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

        public async Task Run()
        {
            /*
             * This sample starts a simple MQTT server which will accept any TCP connection.
             */

            var logger = new ConsoleLogger();

            var mqttFactory = new MqttFactory(logger);

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

                mqttServer.InterceptingPublishAsync += async e =>
                {

                    logger.Publish(MQTTnet.Diagnostics.MqttNetLogLevel.Info,
                        "app",
                        $"'{e.ClientId}' reported '{e.ApplicationMessage.Topic}' > '{Encoding.UTF8.GetString(e.ApplicationMessage.Payload ?? new byte[0])}'",
                        null, null);

                    var client = new Client(_configuration);
                    var r = await client.Connect(_settings!.DeviceId!, 
                        _settings!.Hostname!, 
                        _settings!.Port,
                        _settings!.SasToken!);

                    var result = await client.Publish(e.ApplicationMessage);

                    //return Task.CompletedTask;
                };
                await mqttServer.StartAsync();

                Console.WriteLine("Press Enter to exit.");
                Console.ReadLine();

                // Stop and dispose the MQTT server if it is no longer needed!


            }
        }
    }
}