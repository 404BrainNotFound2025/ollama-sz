# AI Document Analyzer

A command-line tool to have a multi-turn conversation with any document,
powered by OpenAI or a local LLM via Ollama. Built with C# and .NET 8.

---

## What This Demonstrates

- **Prompt Engineering** — system prompt instructs the model to answer only
  from the provided document and admit when information isn't present
- **Streaming Responses** — output is streamed token-by-token in real time
  using `IAsyncEnumerable<string>`
- **Multi-Turn Conversation** — full conversation history is maintained and
  replayed on every API call since LLMs are stateless
- **Provider Abstraction** — `IDocumentChatProvider` interface allows
  switching between OpenAI and Ollama without changing business logic
- **Local LLM Inference** — runs fully offline via Ollama with llama3.1:8b

---

## Project Structure

```
DocumentAnalyzer.OpenAI/
├── Program.cs                   ← Composition root / DI wiring
├── Commands/
│   └── AnalyzeCommand.cs        ← User interaction loop
├── Services/
│   └── DocumentChatService.cs   ← Business logic, prompt engineering, RAG
├── Providers/
│   ├── IDocumentChatProvider.cs ← Provider abstraction interface
│   ├── OpenAIChatProvider.cs    ← OpenAI cloud implementation
│   └── OllamaChatProvider.cs    ← Local LLM implementation
├── Models/
│   └── ChatSession.cs           ← Conversation history / context manager
└── appsettings.json
```

### How a conversation flows:

```
User types question
      ↓
AnalyzeCommand
      ↓
DocumentChatService.AskAsync()
  → adds question to ChatSession history
  → sends full history to provider
      ↓
IDocumentChatProvider.StreamResponseAsync()
  → OpenAIChatProvider  (api.openai.com)
  → OllamaChatProvider  (localhost:11434)
      ↓
Streamed chunks yielded back in real time
      ↓
Full response saved to ChatSession history
      ↓
Next question has full context
```

---

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Ollama](https://ollama.com) _(for local inference)_
- OpenAI API key _(for cloud inference)_

### Install Dependencies

```bash
dotnet add package OpenAI
dotnet add package Microsoft.Extensions.Configuration
dotnet add package Microsoft.Extensions.Configuration.Json
dotnet add package Microsoft.Extensions.Configuration.UserSecrets
dotnet add package Microsoft.Extensions.Configuration.EnvironmentVariables
```

### Configure OpenAI API Key

```bash
dotnet user-secrets init
dotnet user-secrets set "OpenAI:ApiKey" "sk-your-key-here"
```

### Set Up Ollama (Local Inference)

```bash
# Pull the model (~4.7GB)
ollama pull llama3.1:8b

# Verify it works
ollama run llama3.1:8b

# Check downloaded models
ollama list
```

---

## ▶️ Running the App

```bash
# Default provider (Ollama)
dotnet run -- your-document.txt

# Explicitly choose provider
dotnet run -- your-document.txt --provider openai
dotnet run -- your-document.txt --provider ollama
```

### Example Session

```
Loaded: report.txt (2,847 chars)
Type your questions. Enter 'exit' to quit.

You: What were the main revenue drivers?
Assistant: Based on the document, the main revenue drivers were enterprise
subscriptions which grew 34%, and expansion into the APAC region...

You: What was the headcount change?
Assistant: The document states headcount grew from 45 to 67 employees...

You: exit
```

---

## VSCode Extensions

| Extension              | Purpose                                     |
| ---------------------- | ------------------------------------------- |
| C# Dev Kit             | IntelliSense, debugging, project management |
| REST Client            | Test HTTP endpoints directly in VSCode      |
| .gitignore Generator   | Generate .NET-appropriate .gitignore files  |
| .NET Core User Secrets | Manage secrets without leaving VSCode       |

---

## Tech Stack

| Technology                         | Role                        |
| ---------------------------------- | --------------------------- |
| .NET 8 / C#                        | Application framework       |
| OpenAI .NET SDK                    | OpenAI + Ollama API client  |
| Ollama                             | Local LLM runtime           |
| llama3.1:8b                        | Local language model        |
| gpt-4o-mini                        | Cloud language model        |
| Microsoft.Extensions.Configuration | Config + secrets management |
