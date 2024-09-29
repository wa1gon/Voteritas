using NATS.Client;
using NATS.Client.JetStream;
using System.Text;
using TrustedVotingNatsLibrary.Abstract;

namespace ElectionAuthorityService.Services;
public class JetSteamService : IMessagingService, IDisposable
{
    private readonly IConnection _connection;
    private readonly IJetStream _jetStream;
    private readonly IJetStreamManagement _jetStreamManagement;

    public JetSteamService()
    {
        var options = ConnectionFactory.GetDefaultOptions();
        options.Url = "nats://localhost:4222";
        _connection = new ConnectionFactory().CreateConnection(options);
        _jetStream = _connection.CreateJetStreamContext();
        _jetStreamManagement = _connection.CreateJetStreamManagementContext();
    }

    public void CreateStream(string streamName, string subject)
    {
        try
        {
            // Check if the stream already exists
            _jetStreamManagement.GetStreamInfo(streamName);
        }
        catch (NATSJetStreamException ex) when (ex.ErrorCode == 10059) // Stream not found
        {
            // Stream does not exist, create it
            var streamConfig = StreamConfiguration.Builder()
                .WithName(streamName)
                .WithSubjects(subject)
                .WithStorageType(StorageType.File) // Set storage type to File for persistence
                .Build();
            _jetStreamManagement.AddStream(streamConfig);
        }
    }

    public async Task PublishAsync(string subject, string message)
    {
        var data = Encoding.UTF8.GetBytes(message);
        await _jetStream.PublishAsync(subject, data);
    }

    public void Subscribe(string subject, Action<string> messageHandler)
    {
        var consumer = _jetStream.PushSubscribeAsync(subject, (sender, args) =>
        {
            var message = Encoding.UTF8.GetString(args.Message.Data);
            messageHandler(message);
            args.Message.Ack();
        }, true); // Added the 'autoAck' parameter
    }

    public void Dispose()
    {
        _connection?.Dispose();
    }
}
