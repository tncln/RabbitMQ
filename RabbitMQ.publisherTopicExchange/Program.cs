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

            channel.ExchangeDeclare("logs-direct", durable: true, type: ExchangeType.Direct);

            Enum.GetNames(typeof(LogNames)).ToList().ForEach(x => {
                var routeKey = $"route-{x}";
                var queueName = $"direct-queue-{x}";
                channel.QueueDeclare(queueName, true, false, false);
                channel.QueueBind(queueName, "logs-direct", routeKey);
            });

            Enumerable.Range(1, 50).ToList().ForEach(x =>
            {

                LogNames log = (LogNames)new Random().Next(1, 5);

                string mesaj = $"log-type : {log}";


                //Kuyruğa mesajlar, byte[] olarak gönderilir. 
                var messageBody = Encoding.UTF8.GetBytes(mesaj);

                var routeKey = $"route-{log}";

                channel.BasicPublish("logs-direct", routeKey, null, messageBody); //mesaj gönderilir. 

                Console.WriteLine($"{mesaj}. Log Gönderilmiştir.");
            });



            Console.ReadLine();
        }
    }
}
