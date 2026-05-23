using DocumentAnalyzer.CLI.Providers;
using OpenAI;
using OpenAI.Chat;
using System.ClientModel;

namespace DocumentAnalyzer.CLI.Providers;

public class OpenAIChatProvider : IDocumentChatProvider
{
    private readonly ChatClient _client;
    public string ProviderName => "OpenAI (gpt-4o-mini)";

    public OpenAIChatProvider(string apiKey)
    {
        // _client = new OpenAIClient(apiKey)
        //     .GetChatClient("gpt-4o-mini");

        _client = new OpenAIClient(new ApiKeyCredential(apiKey))
            .GetChatClient("gpt-4o-mini");

    }

    public async IAsyncEnumerable<string> StreamResponseAsync(
        IReadOnlyList<(string Role, string Content)> history,
        [System.Runtime.CompilerServices.EnumeratorCancellation]
        CancellationToken cancellationToken = default)
    {
        var messages = history.Select(MapMessage).ToList();

        await foreach (var update in _client
            .CompleteChatStreamingAsync(messages, cancellationToken: cancellationToken))
        {
            foreach (var part in update.ContentUpdate)
                yield return part.Text;
        }
    }

    private static ChatMessage MapMessage((string Role, string Content) msg) =>
        msg.Role switch
        {
            "system"    => new SystemChatMessage(msg.Content),
            "user"      => new UserChatMessage(msg.Content),
            "assistant" => new AssistantChatMessage(msg.Content),
            _           => throw new ArgumentOutOfRangeException(nameof(msg.Role))
        };
}