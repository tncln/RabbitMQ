using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace RabbitMQ.subscriber
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();

            factory.Uri = new Uri("amqps://tpmxkawx:tP_TZFioZbqGBarCUE3Lsl4l-C8jDaed@fish.rmq.cloudamqp.com/tpmxkawx");
             
            using var connection = factory.CreateConnection(); //Connection Gerçekleşir.
             
            var channel = connection.CreateModel(); //Kanal Oluşur
             
            //Kuyruktan veri okurken bu kısım silinebilir, eğer silinirse kuyruk okumaya çalıştığında eğer kuyruk
            //ismi yoksa yeni oluşturur, silinmez ise kuyruğu bulamadığı durumda hata verir,
            channel.QueueDeclare("hello-queue", true, false, false); //Kuyruk oluşur..

            var consumer = new EventingBasicConsumer(channel);

            //autoAck:true olursa doğruda işlense yanlışta işlense kuyruktan siler. false olursa,
            //kuyruktan silmez doğru işlerse silmek için haber verir
            channel.BasicConsume("hello-queue", true, consumer);

            consumer.Received += (object sender, BasicDeliverEventArgs e) =>
            {
                var message = Encoding.UTF8.GetString(e.Body.ToArray());
                Console.WriteLine("Gelen Mesaj:" + message);
            };

            Console.ReadLine();
        } 
    }
}
