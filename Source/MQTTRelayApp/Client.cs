using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Diagnostics;
using MQTTnet.Exceptions;
using MQTTnet.Formatter;
using MQTTRelayApp.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
//using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime;

namespace MQTTRelayApp
{
    internal class Client
    {

        IMqttClient? _mqttClient;
        MqttProtocolVersion _version = MqttProtocolVersion.V311;
        int _timeoutSeconds = 10;
        string? _apiVersion;
        
        public Client(IConfiguration configuration)
        {
            var _settings = configuration.GetRequiredSection("Settings").Get<Settings>();
            _apiVersion = _settings.ApiVersion; // ["apiVersion"];
        }


        public async Task<MqttClientConnectResult> Connect(string deviceId, string server, int port, string SasToken)
        {
            var userName = $"{server}/{deviceId}/?api-version={_apiVersion}";
            var password = SasToken;

            _mqttClient = new MqttFactory(new ConsoleLogger()).CreateMqttClient();
            var clientOptionsBuilder = new MqttClientOptionsBuilder().WithTimeout(TimeSpan.FromSeconds(_timeoutSeconds))
                .WithProtocolVersion(_version)
                .WithClientId(deviceId)
                //.WithCleanSession(item.SessionOptions.CleanSession)
                .WithCredentials(userName, password)
            //.WithRequestProblemInformation(item.SessionOptions.RequestProblemInformation)
            //.WithRequestResponseInformation(item.SessionOptions.RequestResponseInformation)
            //.WithKeepAlivePeriod(TimeSpan.FromSeconds(item.SessionOptions.KeepAliveInterval));
                .WithTcpServer(server, port)
                .WithTls();

            _mqttClient.DisconnectedAsync += OnDisconnected;


            using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(_timeoutSeconds));
            try
            {
                return await _mqttClient.ConnectAsync(clientOptionsBuilder.Build(), timeout.Token);
            }
            catch (OperationCanceledException)
            {
                if (timeout.IsCancellationRequested)
                {
                    throw new MqttCommunicationTimedOutException();
                }

                throw;
            }

        }

        public async Task<MqttClientPublishResult> Publish(MqttApplicationMessage message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            ThrowIfNotConnected();
            return await _mqttClient!.PublishAsync(message);
        }

        public async Task<MqttClientPublishResult> Publish(PublishItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            ThrowIfNotConnected();


            var applicationMessageBuilder = new MqttApplicationMessageBuilder().WithTopic(item.Topic)
                .WithQualityOfServiceLevel(item.QualityOfServiceLevel.Value)
                .WithRetainFlag(item.Retain)
                .WithMessageExpiryInterval(item.MessageExpiryInterval)
                .WithContentType(item.ContentType)
                //.WithPayloadFormatIndicator(item.PayloadFormatIndicator.Value)
                //.WithPayload(item.PayloadInputFormat.ConvertPayloadInput(item.Payload))
                .WithResponseTopic(item.ResponseTopic);


            if (item.TopicAlias > 0)
            {
                applicationMessageBuilder.WithTopicAlias(item.TopicAlias);
            }

            //foreach (var userProperty in item.UserProperties.Items)
            //{
            //    if (!string.IsNullOrEmpty(userProperty.Name))
            //    {
            //        applicationMessageBuilder.WithUserProperty(userProperty.Name, userProperty.Value);
            //    }
            //}

            return await _mqttClient!.PublishAsync(applicationMessageBuilder.Build());

        }

        Task Disconnect()
        {
            ThrowIfNotConnected();
            return _mqttClient!.DisconnectAsync();
        }


        void ThrowIfNotConnected()
        {
            if (_mqttClient == null || !_mqttClient.IsConnected)
            {
                throw new InvalidOperationException("The MQTT client is not connected.");

            }
        }

        Task OnDisconnected(MqttClientDisconnectedEventArgs eventArgs)
        {
            //mark disconnected.

            return Task.CompletedTask;
        }
    }
}
