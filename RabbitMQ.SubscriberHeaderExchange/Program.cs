using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.IO;
using System.Text;
using System.Threading;

namespace RabbitMQ.SubscriberHeaderExchange
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();

            factory.Uri = new Uri("amqps://tpmxkawx:tP_TZFioZbqGBarCUE3Lsl4l-C8jDaed@fish.rmq.cloudamqp.com/tpmxkawx");

            using var connection = factory.CreateConnection(); //Connection Gerçekleşir. 

            var channel = connection.CreateModel(); //Kanal Oluşur 


            channel.BasicQos(0, 1, false);//false, Her subscriber a 1 1 gönderir Değer kaç ise, true; 10 adet ise,
                                          //subscriber 5 ise 2şer şeklinde gönderir. 

            //Kuyruktan veri okurken bu kısım silinebilir, eğer silinirse kuyruk okumaya çalıştığında eğer kuyruk
            //ismi yoksa yeni oluşturur, silinmez ise kuyruğu bulamadığı durumda hata verir,
            channel.QueueDeclare("hello-queue", true, false, false); //Kuyruk oluşur..

            var consumer = new EventingBasicConsumer(channel);

            var queueName = channel.QueueDeclare().QueueName;

            var routekey = "*.Error.*";

            channel.QueueBind(queueName, "logs-topic", routekey);

            //autoAck:true olursa doğruda işlense yanlışta işlense kuyruktan siler. false olursa,
            //kuyruktan silmez doğru işlerse silmek için haber verir
            channel.BasicConsume(queueName, false, consumer);

            Console.WriteLine("Logları dinleniyor..");

            consumer.Received += (object sender, BasicDeliverEventArgs e) =>
            {
                var message = Encoding.UTF8.GetString(e.Body.ToArray());
                Thread.Sleep(1500);
                Console.WriteLine("Gelen Mesaj:" + message);
                File.AppendAllText("log-critical.txt", message + "\n");
                channel.BasicAck(e.DeliveryTag, false);// kuyruktan okunan mesajın silinmesini sağlar. Başarılı şekilde işlendi artık kuyruktan sil denilir. 
            };

            Console.ReadLine();
        }
    }
}
