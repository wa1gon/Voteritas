namespace VoterRegistrarServer.Service;

using System;
using System.Text;
using NATS.Client;
using NATS.Client.JetStream;
using Newtonsoft.Json;

class VoterRegistrarService
{
    public static void SendSignedPayloadToElectionAuthority(string jsonPayload, string signedPayload)
    {
        var options = ConnectionFactory.GetDefaultOptions();
        options.Url = "nats://localhost:4222"; // NATS server URL

        using (IConnection connection = new ConnectionFactory().CreateConnection(options))
        {
            // Create JetStream context for publishing
            IJetStream js = connection.CreateJetStreamContext();

            // Create or use an existing stream for voter registration messages
            StreamConfiguration streamConfig = StreamConfiguration.Builder()
                .WithName("VoterRegistrationStream")
                .WithSubjects("election.voter.register")
                .Build();
            IJetStreamManagement jsm = connection.CreateJetStreamManagementContext();
            try
            {
                jsm.AddStream(streamConfig);
            }
            catch (NATSJetStreamException ex)
            {
                Console.WriteLine("Stream already exists.");
            }

            // Create the message with the signed payload
            string messageContent = JsonConvert.SerializeObject(new
            {
                Payload = jsonPayload,
                Signature = signedPayload
            });

            // Convert message content to bytes
            byte[] messageBytes = Encoding.UTF8.GetBytes(messageContent);

            // Publish the message to JetStream and wait for an acknowledgment
            var ack = js.Publish("election.voter.register", messageBytes);
            Console.WriteLine($"Message published to ElectionAuthority. ACK received: {ack}");
        }
    }
}
