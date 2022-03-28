using RabbitMQ.Client;
using System;
using System.Linq;
using System.Text;

namespace RabbitMQ.FanoutExchange
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();

            factory.Uri = new Uri("amqps://tpmxkawx:tP_TZFioZbqGBarCUE3Lsl4l-C8jDaed@fish.rmq.cloudamqp.com/tpmxkawx");


            using var connection = factory.CreateConnection(); //Connection Gerçekleşir.


            var channel = connection.CreateModel(); //Kanal Oluşur 

            //channel.QueueDeclare("hello-queue", true, false, false); //Kuyruk oluşur..

            channel.ExchangeDeclare("logs-fanout", durable: true, type: ExchangeType.Fanout);

            Enumerable.Range(1, 50).ToList().ForEach(x =>
            {
                string mesaj = $"log {x}";


                //Kuyruğa mesajlar, byte[] olarak gönderilir. 
                var messageBody = Encoding.UTF8.GetBytes(mesaj);
                 

                channel.BasicPublish("logs-fanout","", null, messageBody); //mesaj gönderilir. 

                Console.WriteLine($"{x}. Mesaj Gönderilmiştir.");
            });



            Console.ReadLine();
        }
    }
}
