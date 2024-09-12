namespace RentalManager.WebApi.Settings;

public class MessageBrokerSettings
{
    public const string SettingsKey = "MessageBrokerSettings";
    public string Host { get; init; } = string.Empty;
    public TopicsConfig? Topics { get; init; }

    public class TopicsConfig
    {
        public string MotorCycleCreated { get; init; } = string.Empty;
        public string MotorCycleUpdated { get; init; } = string.Empty;
        public string MotorCycleDeleted { get; init; } = string.Empty;

    }
}
