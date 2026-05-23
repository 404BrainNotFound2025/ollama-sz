# Instructions

## Packages

- dotnet add package OpenAI
- dotnet add package Microsoft.Extensions.Configuration
- dotnet add package Microsoft.Extensions.Configuration.Json
- dotnet add package Microsoft.Extensions.Configuration.UserSecrets
- dotnet add package Microsoft.Extensions.Configuration.EnvironmentVariables

## Extensions

- C# Dev Kit
- REST Client
- .gitignore Generator
- .NET Core User Secrets


## Ollama
### Download and install Ollama for Windows

- ollama pull llama3.1:8b
- ollama run llama3.1:8b
- ollama list

# Run

- dotnet run -- test-doc.txt
    - dotnet run -- test-doc.txt --provider openai
    - dotnet run -- test-doc.txt --provider ollama

- dotnet run -- test-doc-small.txt

