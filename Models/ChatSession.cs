namespace DocumentAnalyzer.CLI.Models;

public class ChatSession
{
    private readonly List<(string Role, string Content)> _history = new();
    public IReadOnlyList<(string Role, string Content)> History => _history;

    public void AddSystem(string content)    => _history.Add(("system", content));
    public void AddUser(string content)      => _history.Add(("user", content));
    public void AddAssistant(string content) => _history.Add(("assistant", content));
}