using Azure.Messaging.ServiceBus;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Management;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StagwellTech.ServiceBusRPC
{
    public class ServiceBusRPCServiceAttribute : Attribute
    {
        public ServiceBusRPCServiceAttribute(string queueName)
        {
            this.queueName = queueName;
        }

        public string queueName { get; }
    }

    public interface IServiceBusRPCService
    {
        void registerHandler(Func<string, string> handler, string queueName);
    }
    public class ServiceBusRPCService : IServiceBusRPCService
    {
        string serviceBusConnectionString { get; set; }
        string ServiceBusConnectionPrefix { get; set; }
        private ManagementClient managementClient;


        protected static Dictionary<string, QueueClient> ResponseQueueClients = new Dictionary<string, QueueClient>();
        protected static Dictionary<string, ServiceBusSender> ResponseQueueSenders = new Dictionary<string, ServiceBusSender>();

        public ServiceBusRPCService(string serviceBusConnectionString)
            : this(serviceBusConnectionString, true) { }

        public ServiceBusRPCService(string serviceBusConnectionString, bool autoRegistration)
        {
            this.serviceBusConnectionString = serviceBusConnectionString;
            this.managementClient = new ManagementClient(this.serviceBusConnectionString);

            this.ServiceBusConnectionPrefix = Environment.GetEnvironmentVariable("ServiceBusConnectionPrefix");
            

            if (String.IsNullOrWhiteSpace(this.ServiceBusConnectionPrefix))
            {
                this.ServiceBusConnectionPrefix = "";
            }

            if (!autoRegistration)
                return;

            /* this will loop through the calling Assembly to identify anything marked with a "ServiceBusRPCServiceAttribute" and automatically register the handler */
            Assembly myAssembly = Assembly.GetEntryAssembly();

            foreach (Type type in myAssembly.GetTypes())
            {
                foreach (var method in type.GetMethods())
                {
                    ServiceBusRPCServiceAttribute serviceBusAttr = (ServiceBusRPCServiceAttribute)method.GetCustomAttribute(typeof(ServiceBusRPCServiceAttribute));
                    if(serviceBusAttr != null)
                    {
                        Task.Run(async () =>
                        {
                            await this.registerHandlerNew((string param) =>
                            {
                                return (string)method.Invoke(null, new[] { param });
                            }, serviceBusAttr.queueName);
                        });
                    }

                }
            }
        }

        protected static ServiceBusSender GetResponseQueueClientNew(string queueName, string serviceBusConnectionString)
        {
            var key = queueName + "_" + serviceBusConnectionString;
            if (ResponseQueueSenders.ContainsKey(key))
            {
                if (ResponseQueueSenders[key] != null && !ResponseQueueSenders[key].IsClosed)
                {
                    return ResponseQueueSenders[key];
                }
                else
                {
                    ResponseQueueSenders.Remove(key);
                }
            }
            var client = new ServiceBusClient(serviceBusConnectionString);
            ServiceBusSender sender = client.CreateSender(queueName);
            // create the sender
            ResponseQueueSenders.Add(key, sender);
            return ResponseQueueSenders[key];
        }

        protected static QueueClient GetResponseQueueClient(string queueName, string serviceBusConnectionString)
        {
            var key = queueName + "_" + serviceBusConnectionString;
            if (ResponseQueueClients.ContainsKey(key))
            {
                if (ResponseQueueClients[key] != null && !ResponseQueueClients[key].IsClosedOrClosing)
                {
                    return ResponseQueueClients[key];
                }
                else
                {
                    ResponseQueueClients.Remove(key);
                }
            }
            var queueClient = new QueueClient(serviceBusConnectionString, queueName);
            ResponseQueueClients.Add(key, queueClient);
            return ResponseQueueClients[key];
        }

        public async Task registerHandlerNew(Func<string, string> handler, string queueName)
        {
            if (!String.IsNullOrWhiteSpace(this.ServiceBusConnectionPrefix))
            {
                queueName = this.ServiceBusConnectionPrefix + "-" + queueName;
            }
            Task<bool> task = Task.Run(async () =>
            {
                return await CreateQueueIfNeeded(queueName, true);
            });
            var queuesExist = task.Result;

            if (!queuesExist)
            {
                throw new Azure.Messaging.ServiceBus.ServiceBusException(false, "Queues do not exist. Create manually");
            }
            // create the options to use for configuring the processor
            var options = new ServiceBusProcessorOptions
            {
                // By default or when AutoCompleteMessages is set to true, the processor will complete the message after executing the message handler
                // Set AutoCompleteMessages to false to [settle messages](https://docs.microsoft.com/en-us/azure/service-bus-messaging/message-transfers-locks-settlement#peeklock) on your own.
                // In both cases, if the message handler throws an exception without settling the message, the processor will abandon the message.
                AutoCompleteMessages = false,

                // I can also allow for multi-threading
                MaxConcurrentCalls = 5
            };

            var client = new ServiceBusClient(this.serviceBusConnectionString);
            ServiceBusProcessor processor = client.CreateProcessor(queueName, options);

            // configure the message and error handler to use
            processor.ProcessMessageAsync += MessageHandler;
            processor.ProcessErrorAsync += ErrorHandler;

            /* Register the function that will process messages */
            async Task MessageHandler(ProcessMessageEventArgs args) {

                DebugLog.WriteToLog($"Received message: SequenceNumber:{args.Message.SequenceNumber} Body:{args.Message.Body.ToString()} MessageID: {args.Message.MessageId}");


                /* Build response message with the Handler's response */
                string responseMessage = null;

                try
                {
                    responseMessage = handler(args.Message.Body.ToString());
                }
                catch (Exception e)
                {
                    DebugLog.WriteToLog(e.Message);
                    DebugLog.WriteToLog(e.StackTrace);
                }


                /* mark the current message as complete */
                await args.CompleteMessageAsync(args.Message);

                /* Create response client on "message.ReplyTo" queue. */
                if (!String.IsNullOrWhiteSpace(args.Message.ReplyTo) && !String.IsNullOrWhiteSpace(args.Message.ReplyToSessionId))
                {
                    /* create the response */
                    var response = new ServiceBusMessage(responseMessage);
                    response.MessageId = Guid.NewGuid().ToString();
                    response.SessionId = args.Message.ReplyToSessionId;

                    var responseClient = GetResponseQueueClientNew(args.Message.ReplyTo, this.serviceBusConnectionString);//new QueueClient(this.serviceBusConnectionString, message.ReplyTo);
                    /* send the response */
                    await responseClient.SendMessageAsync(response);
                }

            };

            Task ErrorHandler(ProcessErrorEventArgs args)
            {
                // the error source tells me at what point in the processing an error occurred
                Console.WriteLine(args.ErrorSource);
                // the fully qualified namespace is available
                Console.WriteLine(args.FullyQualifiedNamespace);
                // as well as the entity path
                Console.WriteLine(args.EntityPath);
                Console.WriteLine(args.Exception.ToString());
                return Task.CompletedTask;
            }

            // start processing
            await processor.StartProcessingAsync();
            DebugLog.WriteToLog("Handler Registered");
        }

        public void registerHandler(Func<string, string> handler, string queueName)
        {
            if(!String.IsNullOrWhiteSpace(this.ServiceBusConnectionPrefix))
            {
                queueName = this.ServiceBusConnectionPrefix + "-" + queueName;
            }
            Task<bool> task = Task.Run(async () =>
            {
                return await CreateQueueIfNeeded(queueName, true);
            });
            var queuesExist = task.Result;

            if(!queuesExist)
            {
                throw new Microsoft.Azure.ServiceBus.ServiceBusException(false, "Queues do not exist. Create manually");
            }

            var queueClient = new QueueClient(this.serviceBusConnectionString, queueName);

            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 5,
                AutoComplete = false
            };

            /* Register the function that will process messages */
            queueClient.RegisterMessageHandler(async (Message message, CancellationToken token) => {

                DebugLog.WriteToLog($"Received message: SequenceNumber:{message.SystemProperties.SequenceNumber} Body:{Encoding.UTF8.GetString(message.Body)} MessageID: {message.MessageId}");


                /* Build response message with the Handler's response */
                string responseMessage = null;

                try
                {
                    responseMessage = handler(Encoding.UTF8.GetString(message.Body));
                } catch (Exception e)
                {
                    DebugLog.WriteToLog(e.Message);
                    DebugLog.WriteToLog(e.StackTrace);
                }


                /* mark the current message as complete */
                await queueClient.CompleteAsync(message.SystemProperties.LockToken);

                /* Create response client on "message.ReplyTo" queue. */
                if (!String.IsNullOrWhiteSpace(message.ReplyTo) && !String.IsNullOrWhiteSpace(message.ReplyToSessionId))
                {
                    /* create the response */
                    var response = new Message(Encoding.UTF8.GetBytes(responseMessage));
                    response.MessageId = Guid.NewGuid().ToString();
                    response.SessionId = message.ReplyToSessionId;

                    var responseClient = GetResponseQueueClient(message.ReplyTo, this.serviceBusConnectionString);//new QueueClient(this.serviceBusConnectionString, message.ReplyTo);
                    /* send the response */
                    await responseClient.SendAsync(response);
                }

            }, messageHandlerOptions);

            DebugLog.WriteToLog("Handler Registered");
        }

        // Use this Handler to look at the exceptions received on the MessagePump
        static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            DebugLog.WriteToLog($"Message handler encountered an exception {exceptionReceivedEventArgs.Exception}.");
            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
            DebugLog.WriteToLog("Exception context for troubleshooting:");
            DebugLog.WriteToLog($"- Endpoint: {context.Endpoint}");
            DebugLog.WriteToLog($"- Entity Path: {context.EntityPath}");
            DebugLog.WriteToLog($"- Executing Action: {context.Action}");

            return Task.CompletedTask;
        }

        private async Task<QueueDescription> GetQueueAsync(string queueName)
        {
            try
            {
                QueueDescription getQueue = await managementClient.GetQueueAsync(queueName).ConfigureAwait(false);
                return getQueue;
            }
            catch (Microsoft.Azure.ServiceBus.ServiceBusException ex)
            {
                Console.WriteLine($"Encountered exception while retrieving Queue -\n{ex}");
                return null;
            }
        }

        private async Task<QueueDescription> CreateQueueAsync(string queueName, bool isResponseQueue)
        {
            var queueDescription = new QueueDescription(queueName)
            {
                LockDuration = TimeSpan.FromSeconds(30),
                MaxSizeInMB = 1024,
                RequiresDuplicateDetection = true,
                DuplicateDetectionHistoryTimeWindow = TimeSpan.FromSeconds(30),
                DefaultMessageTimeToLive = TimeSpan.FromMinutes(1),
                EnableDeadLetteringOnMessageExpiration = false,
                MaxDeliveryCount = 10,

                EnablePartitioning = !isResponseQueue,
                RequiresSession = isResponseQueue
            };

            try
            {
                QueueDescription createdQueue = await managementClient.CreateQueueAsync(queueDescription).ConfigureAwait(false);
                return createdQueue;
            }
            catch (Microsoft.Azure.ServiceBus.ServiceBusException ex)
            {
                Console.WriteLine($"Encountered exception while creating Queue -\n{ex}");
                return null;
            }
        }

        private async Task<bool> CreateQueueIfNeeded(string queueName, bool createResQueue, string resQueueName = null)
        {

            QueueDescription queue = await GetQueueAsync(queueName);

            if(queue == null)
            {
                queue = await CreateQueueAsync(queueName, false);
            }

            QueueDescription resQueue = null;

            if (createResQueue)
            {
                if (resQueueName == null)
                {
                    resQueueName = queueName + "-res";
                }

                resQueue = await GetQueueAsync(resQueueName);

                if (resQueue == null)
                {
                    resQueue = await CreateQueueAsync(resQueueName, true);
                }
            }

            if(queue == null || (createResQueue && resQueue == null))
            {
                return false;
            }

            return true;
        }
    }
}
