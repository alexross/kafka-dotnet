using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Confluent.Kafka;

namespace consumer
{
    class Program
    {
        static void Main(string[] args)
        {

        }

        public async Task Consume()
        {
            var config = new Dictionary<string, object>();

            using (var consumer = new Consumer<Null, string>(config, null, null)) 
            {
                consumer.OnLog += (sender, e) => Console.WriteLine($"Info: {e.Message}");
                consumer.OnError += (sender, e) => Console.WriteLine($"Error: {e.Reason}");
                consumer.OnMessage += (sender, e) => {
                    // Message received
                    Console.WriteLine($"{e.Topic}@{e.Timestamp.UtcDateTime}: {e.Value}");
                };

            }
        }
    }
}
