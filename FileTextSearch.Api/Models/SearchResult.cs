namespace FileTextSearch.Api.Models;

public class SearchResult
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string FileName { get; set; } = string.Empty;
    public string FullPath { get; set; } = string.Empty;
    public string Category { get; set; } = "General"; // Maps to the subfolder path
    public long FileSize { get; set; }
    public string Priority { get; set; } = "Normal"; // Change priority in the UI to "High" or "Low" as needed
}