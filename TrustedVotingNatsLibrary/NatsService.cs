

using NATS.Client.Core;

namespace TrustedVotingNatsLibrary;
public class NatsService : IDisposable
{

    public NatsService()
    {

    }
    public async Task Sub()
    {
        await using var nats = new NatsConnection();
        var cts = new CancellationTokenSource();

        
        var subscription = Task.Run(async () =>
        {
            await foreach (var msg in nats.SubscribeAsync<string>(subject: "foo").WithCancellation(cts.Token))
            {
                Console.WriteLine($"Received: {msg.Data}");
            }
        });

        // Give subscription time to start
        await Task.Delay(1000);

        for (var i = 0; i < 10; i++)
        {
            await nats.PublishAsync(subject: "foo", data: $"Hello, World! {i}");
        }

        // Give subscription time to receive messages
        await Task.Delay(1000);

        // Unsubscribe
        cts.Cancel();

        await subscription;
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}
//using System;
//using System.Text;
//using System.Threading;
//using NATS.Net;

//public class NatsService : IDisposable
//{
//    private readonly IConnection _natsConnection;
//    public event Action<string> OnMessageReceived;

//    public NatsService()
//    {
//        var cf = new ConnectionFactory();
//        _natsConnection = cf.CreateConnection("nats://localhost:4222");
//    }

//    // Start NATS listener on a separate thread
//    public void SubscribeOnNewThread(string subject)
//    {
//        Thread natsListenerThread = new Thread(() =>
//        {
//            _natsConnection.SubscribeAsync(subject, (sender, args) =>
//            {
//                string message = Encoding.UTF8.GetString(args.Message.Data);
//                // Trigger the event
//                OnMessageReceived?.Invoke(message);
//            });
//        });

//        natsListenerThread.IsBackground = true;
//        natsListenerThread.Start();
//    }

//    public void Dispose()
//    {
//        _natsConnection?.Dispose();
//    }
//}
