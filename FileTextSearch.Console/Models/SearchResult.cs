namespace FileTextSearch.Console.Models;

public class SearchResult
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string FileName { get; set; } = string.Empty;
    public string FullPath { get; set; } = string.Empty;
    public string Category { get; set; } = "General";
    public long FileSize { get; set; } = 0;
    public string Priority { get; set; } = "Normal";
}