using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RabbitMQ.PublisherHeaderExchange
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
            static void Main(string[] args)
            {
                var factory = new ConnectionFactory();

                factory.Uri = new Uri("amqps://tpmxkawx:tP_TZFioZbqGBarCUE3Lsl4l-C8jDaed@fish.rmq.cloudamqp.com/tpmxkawx");


                using var connection = factory.CreateConnection(); //Connection Gerçekleşir.


                var channel = connection.CreateModel(); //Kanal Oluşur 

                //channel.QueueDeclare("hello-queue", true, false, false); //Kuyruk oluşur..

                channel.ExchangeDeclare("header-exchange", durable: true, type: ExchangeType.Headers);

                Dictionary<string, object> headers = new Dictionary<string, object>();
                headers.Add("format", "pdf");
                headers.Add("shape", "a4");

                var properties = channel.CreateBasicProperties();
                properties.Headers = headers;

                channel.BasicPublish("header-exchange", string.Empty, properties,Encoding.UTF8.GetBytes("header mesajım"));

                Console.WriteLine("Mesaj Gönderildi..");

                Console.ReadLine();
            }
    }
}
