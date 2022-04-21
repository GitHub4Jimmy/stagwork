using Azure.Messaging.ServiceBus;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StagwellTech.ServiceBusRPC
{
    public interface IServiceBusRPCClient
    {
        Task<ServiceBusReceivedMessage> rpcRequest(string queueName, string responseQueueName, string messageBody);
    }

    public class ServiceBusRPCQueueClient
    {
        protected readonly string queueName;
        protected readonly string responseQueueName;
        protected readonly ServiceBusClient client;
        protected readonly ServiceBusSender queueClient;
        protected readonly SessionClient responseClient;
        private static double MAX_SERVICE_BUS_TIMEOUT = 10000;

        public ServiceBusRPCQueueClient(string queueName, string serviceBusConnectionString = null, string serviceBusConnectionPrefix = null)
        {
            if(String.IsNullOrWhiteSpace(serviceBusConnectionString))
            {
                serviceBusConnectionString = Environment.GetEnvironmentVariable("ServiceBusConnectionString");
            }

            if (String.IsNullOrWhiteSpace(serviceBusConnectionPrefix))
            {
                serviceBusConnectionPrefix = Environment.GetEnvironmentVariable("ServiceBusConnectionPrefix");
            }

            if (!String.IsNullOrWhiteSpace(serviceBusConnectionPrefix))
            {
                queueName = serviceBusConnectionPrefix + "-" + queueName;
            }

            this.queueName = queueName;
            this.responseQueueName = queueName + "-res";

            client = new ServiceBusClient(serviceBusConnectionString);

            queueClient = client.CreateSender(this.queueName);//new QueueClient(serviceBusConnectionString, this.queueName);
            responseClient = new SessionClient(serviceBusConnectionString, this.responseQueueName);


        }

        public async Task rpcRequestNoResponse(string messageBody)
        {
            var message = new ServiceBusMessage(Encoding.UTF8.GetBytes(messageBody));
            message.MessageId = Guid.NewGuid().ToString();
            await queueClient.SendMessageAsync(message);
        }

        public async Task<ServiceBusReceivedMessage> rpcRequest(string messageBody)
        {
            var message = new ServiceBusMessage(Encoding.UTF8.GetBytes(messageBody));

            message.MessageId = Guid.NewGuid().ToString();
            message.ReplyTo = responseQueueName;
            message.ReplyToSessionId = Guid.NewGuid().ToString();

            //DebugLog.WriteToLog("queueName => " + queueName);
            //DebugLog.WriteToLog("responseQueueName => " + responseQueueName);
            //DebugLog.WriteToLog("messageBody => " + messageBody);
            //DebugLog.WriteToLog("message => " + message.ToString());
            //DebugLog.WriteToLog("message.MessageId => " + message.MessageId.ToString());
            //DebugLog.WriteToLog("message.ReplyTo => " + message.ReplyTo.ToString());
            //DebugLog.WriteToLog("message.ReplyToSessionId => " + message.ReplyToSessionId.ToString());

            try
            {
                //var responseSession = responseClient.AcceptMessageSessionAsync(message.ReplyToSessionId);
                ServiceBusSessionReceiver receiver = await client.AcceptSessionAsync(responseQueueName, message.ReplyToSessionId);

                await queueClient.SendMessageAsync(message);

                var response = await receiver.ReceiveMessageAsync(TimeSpan.FromMilliseconds(MAX_SERVICE_BUS_TIMEOUT));

                await receiver.DisposeAsync();

                return response;
            } catch (Exception e)
            {
                return null;
            }
        }
        //public ServiceBusRPCQueueClient(string queueName, string serviceBusConnectionString, string serviceBusConnectionPrefix)
        //    : base(serviceBusConnectionString, serviceBusConnectionPrefix)
        //{

        //    if (!String.IsNullOrWhiteSpace(this.ServiceBusConnectionPrefix))
        //    {
        //        queueName = this.ServiceBusConnectionPrefix + "-" + queueName;
        //    }

        //    this.queueName = queueName;
        //    this.responseQueueName = queueName + "-res";

        //    queueClient = new QueueClient(this.serviceBusConnectionString, queueName);
        //    responseClient = new SessionClient(this.serviceBusConnectionString, responseQueueName);
        //}

    }

    public class ServiceBusRPCClient : IServiceBusRPCClient
    {
        protected readonly string serviceBusConnectionString;
        protected readonly string ServiceBusConnectionPrefix;

        protected static Dictionary<string, ServiceBusSender> QueueClients = new Dictionary<string, ServiceBusSender>();
        protected static Dictionary<string, SessionClient> SessionClients = new Dictionary<string, SessionClient>();

        protected readonly ServiceBusClient client;
        private static double MAX_SERVICE_BUS_TIMEOUT = 10000;

        public ServiceBusRPCClient(string serviceBusConnectionString)
        {
            this.serviceBusConnectionString = serviceBusConnectionString;
            this.ServiceBusConnectionPrefix = Environment.GetEnvironmentVariable("ServiceBusConnectionPrefix");
            if (String.IsNullOrWhiteSpace(this.ServiceBusConnectionPrefix))
            {
                this.ServiceBusConnectionPrefix = "";
            }
            this.client = new ServiceBusClient(serviceBusConnectionString);
        }
        public ServiceBusRPCClient(string serviceBusConnectionString, string serviceBusConnectionPrefix)
        {
            this.serviceBusConnectionString = serviceBusConnectionString;
            this.ServiceBusConnectionPrefix = serviceBusConnectionPrefix;

            if (String.IsNullOrWhiteSpace(this.ServiceBusConnectionPrefix))
            {
                this.ServiceBusConnectionPrefix = "";
            }
            this.client = new ServiceBusClient(serviceBusConnectionString);
        }

        protected static ServiceBusSender GetQueueClient(string queueName, string serviceBusConnectionString)
        {
            var key = queueName + "_" + serviceBusConnectionString;
            if (QueueClients.ContainsKey(key) && !QueueClients[key].IsClosed)
            {
                return QueueClients[key];
            }
            else
            {
                QueueClients.Remove(key);
            }
            var client = new ServiceBusClient(serviceBusConnectionString);
            var queueClient = client.CreateSender(queueName);
            QueueClients.Add(key, queueClient);

            return QueueClients[key];
        }

        public async Task<ServiceBusReceivedMessage> rpcRequest(string queueName, string messageBody)
        {
            return await rpcRequest(queueName, queueName + "-res", messageBody);
        }

        public async Task<ServiceBusReceivedMessage> rpcRequest(string queueName, string responseQueueName, string messageBody)
        {

            if (!String.IsNullOrWhiteSpace(this.ServiceBusConnectionPrefix))
            {
                queueName = this.ServiceBusConnectionPrefix + "-" + queueName;
            }

            if (!String.IsNullOrWhiteSpace(this.ServiceBusConnectionPrefix))
            {
                responseQueueName = this.ServiceBusConnectionPrefix + "-" + responseQueueName;
            }

            var queueClient = GetQueueClient(queueName, this.serviceBusConnectionString);// new QueueClient(this.serviceBusConnectionString, queueName);
            //var responseClient = GeSessionClient(responseQueueName, this.serviceBusConnectionString);// new SessionClient(this.serviceBusConnectionString, responseQueueName);

            var message = new ServiceBusMessage(Encoding.UTF8.GetBytes(messageBody));

            message.MessageId = Guid.NewGuid().ToString();
            message.ReplyTo = responseQueueName;
            message.ReplyToSessionId = Guid.NewGuid().ToString();

            var responseSession = await client.AcceptSessionAsync(responseQueueName, message.ReplyToSessionId);

            await queueClient.SendMessageAsync(message);

            var response = await responseSession.ReceiveMessageAsync(TimeSpan.FromMilliseconds(MAX_SERVICE_BUS_TIMEOUT));
            await responseSession.DisposeAsync();

            return response;

        }
    }
}
