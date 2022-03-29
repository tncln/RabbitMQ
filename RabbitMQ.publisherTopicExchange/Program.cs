using RabbitMQ.Client;
using System;
using System.Linq;
using System.Text;

namespace RabbitMQ.publisherTopicExchange
{
    public enum LogNames
    {
        Critical = 1,
        Error = 2,
        Warning = 3,
        Info = 4
    }
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();

            factory.Uri = new Uri("amqps://tpmxkawx:tP_TZFioZbqGBarCUE3Lsl4l-C8jDaed@fish.rmq.cloudamqp.com/tpmxkawx");


            using var connection = factory.CreateConnection(); //Connection Gerçekleşir.


            var channel = connection.CreateModel(); //Kanal Oluşur 

            //channel.QueueDeclare("hello-queue", true, false, false); //Kuyruk oluşur..

            channel.ExchangeDeclare("logs-topic", durable: true, type: ExchangeType.Topic);


            Random rnd = new Random();
            Enumerable.Range(1, 50).ToList().ForEach(x =>
            {   
                LogNames log1 = (LogNames)rnd.Next(1, 5);
                LogNames log2 = (LogNames)rnd.Next(1, 5);
                LogNames log3 = (LogNames)rnd.Next(1, 5);

                var routeKey = $"{log1}.{log2}.{log3}";
                string mesaj = $"log-type : {log1}-{log2}-{log3}";
                var messageBody = Encoding.UTF8.GetBytes(mesaj);

                channel.BasicPublish("logs-topic", routeKey, null, messageBody); //mesaj gönderilir. 

                Console.WriteLine($"{mesaj}. Log Gönderilmiştir.");
            });



            Console.ReadLine();
        }
    }
}
