namespace Store.Infraestructure.Messaging.Configuration;

public record RabbitMqOptions
{
    public const string SectionName = "RabbitMq";
    public string Host { get; init; } = string.Empty;
    public string Username { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}