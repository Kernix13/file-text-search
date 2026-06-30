namespace FileTextSearch.Models;

public class SearchResult
{
    // Generates a brand new, completely unique ID automatically whenever a result is created
    public Guid Id { get; set; } = Guid.NewGuid();
    public string FileName { get; set; } = string.Empty;
    public string FullPath { get; set; } = string.Empty;
    public string Category { get; set; } = "General"; // Maps to the subfolder path
    public string FileSize { get; set; } = "0 KB";
    public string Priority { get; set; } = "Normal"; // Change priority in the UI to "High" or "Low" as needed
}