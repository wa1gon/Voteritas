namespace TrustedVoteLibrary.Utils;

public  static class JetStreamUtils
{
    public static void EnsureStreamExists(IConnection connection, string streamName, string subject, ILogger logger)
    {
        try
        {
            IJetStreamManagement jsm = connection.CreateJetStreamManagementContext();
            StreamInfo streamInfo = jsm.GetStreamInfo(streamName);
            // logger.LogInformation().LogInformation($"Stream '{streamName}' already exists.");
        }
        catch (NATSJetStreamException ex)
        {
            if (ex.ErrorCode == 10059) // Stream not found
            {
                // logger.LogInformation($"Stream '{streamName}' does not exist. Creating it now...");

                StreamConfiguration streamConfig = StreamConfiguration.Builder()
                    .WithName(streamName)
                    .WithSubjects(subject)
                    .Build();

                IJetStreamManagement jsm = connection.CreateJetStreamManagementContext();
                jsm.AddStream(streamConfig);

                // logger.LogInformation($"Stream '{streamName}' created successfully.");
            }
            else
            {
                // logger.LogError(ex, $"Error checking or creating stream '{streamName}'");
            }
        }
    }
}
