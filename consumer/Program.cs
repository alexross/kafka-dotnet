using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Confluent.Kafka.Serialization;

namespace consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            // todo figure out why command line args aren't working for vs mac
            string brokerList = "123"; // args[0];
            var topics = new List<string>() { "abc", "def" }; // args.Skip(1).ToList();

            var config = new Dictionary<string, object>()
            {
                { "group-id", "consumer-dotnet" },
                { "bootstrap-servers", brokerList }
            };

            // todo can't load librdkafka.redist
            using (var consumer = new Consumer<Ignore, string>(config, null, new StringDeserializer(Encoding.UTF8)))
            {
                consumer.Assign(new List<TopicPartitionOffset>() { new TopicPartitionOffset(topics.First(), 0, 0) });

                consumer.OnLog += (_, e) => Console.WriteLine($"Info: {e.Message}");
                consumer.OnError += (_, e) => Console.WriteLine($"Critical Error: {e.Reason}");
                consumer.OnConsumeError += (_, e) => Console.WriteLine($"Consume Error: {e.Value}");
                consumer.OnMessage += (_, e) => Console.WriteLine($"{e.Topic}@{e.Timestamp.UtcDateTime}: {e.Value}");

                while (!Console.KeyAvailable) 
                {
                    Thread.Sleep(1000);
                }
                Console.WriteLine("\nShutting down...");
            }
        }
    }
}
