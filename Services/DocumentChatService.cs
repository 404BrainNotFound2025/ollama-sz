using DocumentAnalyzer.CLI.Models;
using DocumentAnalyzer.CLI.Providers;

namespace DocumentAnalyzer.CLI.Services;

public class DocumentChatService
{
    private readonly IDocumentChatProvider _provider;

    public DocumentChatService(IDocumentChatProvider provider)
    {
        _provider = provider;
    }

    public ChatSession CreateSession(string documentContent)
    {
        var session = new ChatSession();
        session.AddSystem($"""
            You are a document analyst. Answer questions based only on the document below.
            Be concise and cite specific parts when relevant.
            If the answer is not in the document, say so explicitly.

            DOCUMENT:
            ---
            {documentContent}
            ---
            """);
        return session;
    }

    public async IAsyncEnumerable<string> AskAsync(
        ChatSession session,
        string question,
        [System.Runtime.CompilerServices.EnumeratorCancellation]
        CancellationToken cancellationToken = default)
    {
        session.AddUser(question);
        var fullResponse = new System.Text.StringBuilder();

        await foreach (var chunk in _provider.StreamResponseAsync(session.History, cancellationToken))
        {
            fullResponse.Append(chunk);
            yield return chunk;
        }

        session.AddAssistant(fullResponse.ToString());
    }
}