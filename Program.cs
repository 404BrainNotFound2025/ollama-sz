using DocumentAnalyzer.CLI.Commands;
using DocumentAnalyzer.CLI.Providers;
using DocumentAnalyzer.CLI.Services;
using Microsoft.Extensions.Configuration;

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: true)
    .AddUserSecrets<Program>()
    .AddEnvironmentVariables()
    .Build();

if (args.Length == 0)
{
    Console.WriteLine("Usage: DocumentAnalyzer <file-path> [--provider openai|ollama]");
    return;
}

string filePath = args[0];
string providerName = args.Contains("--provider") ? args[Array.IndexOf(args, "--provider") + 1].ToLower() : "ollama";
//string providerName = args.Contains("--provider") ? args[Array.IndexOf(args, "--provider") + 1].ToLower() : "openai";

IDocumentChatProvider provider = providerName switch
{
    "ollama"  => new OllamaChatProvider(),
    "openai"  => new OpenAIChatProvider(
                    config["OpenAI:ApiKey"] 
                    ?? throw new InvalidOperationException(
                        "OpenAI:ApiKey not set. Run: dotnet user-secrets set \"OpenAI:ApiKey\" \"sk-...\"")
                 ),
    _ => throw new ArgumentException($"Unknown provider: {providerName}")
};

var service = new DocumentChatService(provider);
var command = new AnalyzeCommand(service);

Console.WriteLine($"Using provider: {provider.ProviderName}\n");

await command.RunAsync(filePath);