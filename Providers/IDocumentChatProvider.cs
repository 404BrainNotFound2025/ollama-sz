namespace DocumentAnalyzer.CLI.Providers;

public interface IDocumentChatProvider
{
    string ProviderName { get; }
    IAsyncEnumerable<string> StreamResponseAsync(
        IReadOnlyList<(string Role, string Content)> history,
        CancellationToken cancellationToken = default);
}