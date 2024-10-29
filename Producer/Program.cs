using MassTransit;
using Shared;

namespace Producer
{
    public class Message : IMessage
    {
        public string Text { get; set; }
    }
    class Program
    {
        static async Task Main(string[] args)
        {
            string rabbitMqUri = "amqps://aqmlxqtc:4vKegvEoaXUnz03Dr8R5Y3MiEkCUG7Ja@toad.rmq.cloudamqp.com/aqmlxqtc";
            string queue = "mass-transit-example";
            string userName = "aqmlxqtc";
            string password = "4vKegvEoaXUnz03Dr8R5Y3MiEkCUG7Ja";

            var bus = Bus.Factory.CreateUsingRabbitMq(factory =>
            {
                factory.Host(rabbitMqUri, configurator =>
                {
                    configurator.Username(userName);
                    configurator.Password(password);
                });
            });

            var sendToUri = new Uri($"{rabbitMqUri}/{queue}");
            var endPoint = await bus.GetSendEndpoint(sendToUri);

            await Task.Run(async () =>
            {
                while (true)
                {
                    Console.Write("Mesaj yaz : ");
                    Message message = new Message
                    {
                        Text = Console.ReadLine()
                    };
                    if (message.Text.ToUpper() == "C")
                        break;
                    await endPoint.Send<IMessage>(message);
                    Console.WriteLine("");
                }
            });
        }
    }
}