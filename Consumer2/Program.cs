using MassTransit;
using Shared;

namespace Consumer2
{
    public class Message : IMessage
    {
        public string Text { get; set; }
    }
    public class MessageConsumer : IConsumer<IMessage>
    {
        public async Task Consume(ConsumeContext<IMessage> context)
            => Console.WriteLine($"test-queue-2 Gelen mesaj : {context.Message.Text}");
    }
    class Program
    {
        static async Task Main(string[] args)
        {
            string rabbitMqUri = "amqps://aqmlxqtc:4vKegvEoaXUnz03Dr8R5Y3MiEkCUG7Ja@toad.rmq.cloudamqp.com/aqmlxqtc";
            string queue = "mass-transit-example-2";
            string userName = "aqmlxqtc";
            string password = "4vKegvEoaXUnz03Dr8R5Y3MiEkCUG7Ja";

            var bus = Bus.Factory.CreateUsingRabbitMq(factory =>
            {
                factory.Host(rabbitMqUri, configurator =>
                {
                    configurator.Username(userName);
                    configurator.Password(password);
                });

                factory.ReceiveEndpoint(queue, endpoint => endpoint.Consumer<MessageConsumer>());
            });
            await bus.StartAsync();
            Console.ReadLine();
            await bus.StopAsync();
        }
    }
}