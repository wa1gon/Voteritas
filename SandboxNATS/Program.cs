// Install NuGet packages `NATS.Net`, `NATS.Client.Serializers.Json` and `Microsoft.Extensions.Logging.Console`.
using  SandboxNATS;
var i = 0;

var simplePubSub = new Samples();
await simplePubSub.PubSub();
Console.WriteLine("*****************************************************************************");
await simplePubSub.ReqReply();
