using RabbitMQ.Client;
using System;
using System.Linq;
using System.Text;

namespace RabbitMQ.publisher
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();

            factory.Uri = new Uri("amqps://tpmxkawx:tP_TZFioZbqGBarCUE3Lsl4l-C8jDaed@fish.rmq.cloudamqp.com/tpmxkawx");


            using var connection = factory.CreateConnection(); //Connection Gerçekleşir.


            var channel = connection.CreateModel(); //Kanal Oluşur


            //durable: true olursa fiziksel tutulur restart edilirse sorun olmaz, false olursa bellekte tutulur
            //exclusive:true olursa kuyruğa sadece bu kanal üzerinden bağlanabilir başka bağlantı kabul etmez 
            //autoDelete: Kuyruğa bağlı olan son subscribe bağlantısını koparırsa kuyruğu otomatik siler.
            channel.QueueDeclare("hello-queue", true, false, false); //Kuyruk oluşur..

            Enumerable.Range(1, 50).ToList().ForEach (x=>
            {
                string mesaj = $"message {x}";


                //Kuyruğa mesajlar, byte[] olarak gönderilir. 
                var messageBody = Encoding.UTF8.GetBytes(mesaj);


                channel.BasicPublish(string.Empty, "hello-queue", null, messageBody); //mesaj gönderilir. 

                Console.WriteLine($"{x}. Mesaj Gönderilmiştir.");
            });

            

            Console.ReadLine();
        }
    }
}
