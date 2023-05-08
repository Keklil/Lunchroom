namespace Contracts;

public interface IPushSender
{
    Task Push(string title, string message, IReadOnlyList<string> deviceTokens, IReadOnlyDictionary<string, string>? additionalData = null);
}