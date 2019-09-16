using OmniaDelivery.Infra.Helpers;
using OmniaDelivery.Infra.Interfaces;
using Polly;
using RabbitMQ.Client;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OmniaDelivery.Infra.Implementations
{
    /// <summary>
    /// RabbitMQ implementation of the MessagePublisher.
    /// </summary>
    public class RabbitMQMessagePublisher : IMessagePublisher
    {
        private readonly string _host;
        private readonly string _username;
        private readonly string _password;
        private readonly string _exchange;
        private readonly byte _deliverymode;

        public RabbitMQMessagePublisher(string host, string username, string password, string exchange, byte deliverymode = 1)
        {
            _host = host;
            _username = username;
            _password = password;
            _exchange = exchange;
            _deliverymode = deliverymode;
        }

        /// <summary>
        /// Publish a message.
        /// </summary>
        /// <param name="messageType">Type of the message.</param>
        /// <param name="message">The message to publish.</param>
        /// <param name="routingKey">The routingkey to use (RabbitMQ specific).</param>
        public Task PublishMessageAsync(string messageType, object message, string routingKey, IDictionary<string, object> headers = null)
        {
            return Task.Run(() =>
                Policy
                    .Handle<Exception>()
                    .WaitAndRetry(9, r => TimeSpan.FromSeconds(5), (ex, ts) => { Log.Error("Error connecting to RabbitMQ. Retrying in 5 sec."); })
                    .Execute(() =>
                    {
                        var factory = new ConnectionFactory() { HostName = _host, UserName = _username, Password = _password };
                        using (var connection = factory.CreateConnection())
                        {
                            using (var model = connection.CreateModel())
                            {
                                //model.ExchangeDeclare(_exchange, "direct", durable: true, autoDelete: false);
                                string data = MessageSerializer.Serialize(message);
                                var body = Encoding.UTF8.GetBytes(data);
                                IBasicProperties properties = model.CreateBasicProperties();
                                properties.DeliveryMode = _deliverymode;
                                if (headers == null)
                                {
                                    properties.Headers = new Dictionary<string, object> { { "MessageType", messageType } };
                                }
                                else
                                {
                                    headers.Add("MessageType", messageType);
                                    properties.Headers = headers;

                                }

                                try
                                {
                                    model.BasicPublish(_exchange, routingKey, properties, body);
                                }
                                catch (Exception ex)
                                {

                                }
                            }
                        }
                    }));
        }
    }
}