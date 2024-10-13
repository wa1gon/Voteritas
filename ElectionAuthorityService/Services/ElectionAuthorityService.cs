using Serilog;
using TrustedVoteLibrary.Utils;

namespace ElectionAuthorityService.Services;

public class ElectionAuthorityService : IHostedService, IDisposable
{
    private readonly ILogger<ElectionAuthorityService> _logger;
    private IConnection connection;
    private IJetStreamPullSubscription subscription;
    private Timer timer;
    
    public ElectionAuthorityService(ILogger<ElectionAuthorityService> logger)
    {
        _logger = logger;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("ElectionAuthorityService is starting.");

        PullSubscribeOptions pullOptions = PullSubscribeOptions.Builder()
            .WithDurable("durable-election-authority")
            .Build();
        // Initialize NATS connection and JetStream subscription
        var options = ConnectionFactory.GetDefaultOptions();
        options.Url = "nats://localhost:4222"; // NATS server URL

        connection = new ConnectionFactory().CreateConnection(options);
        JetStreamUtils.EnsureStreamExists(connection, "VoterRegistrationStream", "election.voter.register", _logger);
        IJetStream js = connection.CreateJetStreamContext();

        // Subscribe to the JetStream subject with a durable subscription
        subscription = js.PullSubscribe("election.voter.register", pullOptions);

        // Start processing messages periodically
        timer = new Timer(ProcessMessages, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));

        return Task.CompletedTask;
    }

    private void ProcessMessages(object state)
    {
        _logger.LogInformation("Checking for messages...");

        try
        {
            var messages = subscription.Fetch(10, 1000); // Fetch 10 messages or wait for max 1000 ms

            foreach (var msg in messages)
            {
                string messageContent = Encoding.UTF8.GetString(msg.Data);
                dynamic receivedData = JsonConvert.DeserializeObject(messageContent);

                string jsonPayload = receivedData.Payload;
                string signature = receivedData.Signature;

                using (RSA rsa = RSA.Create())
                {
                    rsa.ImportFromPem(File.ReadAllText("public_key.pem"));

                    if (VerifySignature(jsonPayload, signature, rsa))
                    {
                        _logger.LogInformation("Signature verified! Valid payload.");
                        _logger.LogInformation($"Payload: {jsonPayload}");
                    }
                    else
                    {
                        _logger.LogWarning("Signature verification failed.");
                    }
                }

                // Acknowledge the message after processing
                msg.Ack();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while processing messages.");
        }
    }

    private bool VerifySignature(string jsonPayload, string signature, RSA publicKey)
    {
        byte[] payloadBytes = Encoding.UTF8.GetBytes(jsonPayload);
        byte[] signatureBytes = Convert.FromBase64String(signature);

        return publicKey.VerifyData(payloadBytes, signatureBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("ElectionAuthorityService is stopping.");
        timer?.Change(Timeout.Infinite, 0);
        connection?.Dispose();
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        timer?.Dispose();
        connection?.Dispose();
    }
}
