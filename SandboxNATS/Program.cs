// See https://aka.ms/new-console-template for more information
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using NATS.Client.Core;
Console.WriteLine("Hello, World!");



var stopwatch = Stopwatch.StartNew();


var url = Environment.GetEnvironmentVariable("NATS_URL") ?? "127.0.0.1:4222";
Console.WriteLine($"[CON] Connecting to {url}...");


var opts = NatsOpts.Default with { Url = url };
await using var nats = new NatsConnection(opts);


await using var sub = await nats.SubscribeCoreAsync<int>("greet.*");


var reader = sub.Msgs;
var responder = Task.Run(async () =>
{
    await foreach (var msg in reader.ReadAllAsync())
    {
        var name = msg.Subject.Split('.')[1];
        Console.WriteLine($"[REP] Received {msg.Subject}");
        await Task.Delay(500);
        await msg.ReplyAsync($"Hello {name}!");
    }
});

var replyOpts = new NatsSubOpts { Timeout = TimeSpan.FromSeconds(2) };

Console.WriteLine("[REQ] From joe");
var reply = await nats.RequestAsync<int, string>("greet.joe", 0, replyOpts: replyOpts);
Console.WriteLine($"[REQ] {reply.Data}");


Console.WriteLine("[REQ] From sue");
reply = await nats.RequestAsync<int, string>("greet.sue", 0, replyOpts: replyOpts);
Console.WriteLine($"[REQ] {reply.Data}");


Console.WriteLine("[REQ] From bob");
reply = await nats.RequestAsync<int, string>("greet.bob", 0, replyOpts: replyOpts);
Console.WriteLine($"[REQ] {reply.Data}");


await sub.UnsubscribeAsync();

await responder;
try
{
    reply = await nats.RequestAsync<int, string>("greet.joe", 0, replyOpts: replyOpts);
    Console.WriteLine($"[REQ] {reply.Data} - This will timeout. We should not see this message.");
}
catch (NatsNoReplyException)
{
    Console.WriteLine($"Timed out as expected.");
    Console.WriteLine("[REQ] timed out!");
}


Console.WriteLine("Bye!");


return;


//void Log(string log) => Console.WriteLine($"{stopwatch.Elapsed} {log}");
