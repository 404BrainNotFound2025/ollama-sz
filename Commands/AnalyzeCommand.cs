using DocumentAnalyzer.CLI.Services;

namespace DocumentAnalyzer.CLI.Commands;

public class AnalyzeCommand
{
    private readonly DocumentChatService _chatService;

    public AnalyzeCommand(DocumentChatService chatService)
    {
        _chatService = chatService;
    }

    public async Task RunAsync(string filePath, CancellationToken cancellationToken = default)
    {
        if (!File.Exists(filePath))
        {
            Console.WriteLine($"File not found: {filePath}");
            return;
        }

        var content = await File.ReadAllTextAsync(filePath, cancellationToken);
        var session = _chatService.CreateSession(content);

        Console.WriteLine($"Loaded: {filePath} ({content.Length:N0} chars)\n");
        Console.WriteLine("Type your questions. Enter 'exit' to quit.\n");

        while (!cancellationToken.IsCancellationRequested)
        {
            Console.Write("You: ");
            var question = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(question) || 
                question.Equals("exit", StringComparison.OrdinalIgnoreCase))
                break;

            Console.Write("Assistant: ");

            await foreach (var chunk in _chatService.AskAsync(session, question, cancellationToken))
            {
                Console.Write(chunk);
            }

            Console.WriteLine("\n");
        }
    }
}