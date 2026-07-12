using System.Text.Json;
using FileTextSearch.Api.Models;

namespace FileTextSearch.Services;

public static class SearchService
{
    // Add "= new List<SearchResult>();" to initialize an empty list right away
    // I think this should be private
    public static List<SearchResult> SearchResults { get; } = new List<SearchResult>();

    // AppContext.BaseDirectory finds the path to the folder where your program is compiled and running (bin/Debug/net10.0)
    private static readonly string _filePath = Path.Combine(AppContext.BaseDirectory, "Resources", "results.json");

    // GET: api/search
    public static List<SearchResult> GetAll() => SearchResults;

    // GET: api/search/{id}
    public static SearchResult? Get(Guid id) => SearchResults.FirstOrDefault(result => result.Id == id);

    // POST: api/search
    public static void Add(List<SearchResult> newResults)
    {
        // the service expects a list so use AddRange
        SearchResults.AddRange(newResults);

        string? directory = Path.GetDirectoryName(_filePath);
        if (directory != null && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        var options = new JsonSerializerOptions { WriteIndented = true };
        string jsonString = JsonSerializer.Serialize(SearchResults, options);
        File.WriteAllText(_filePath, jsonString);
    }

    // PUT: api/search/{id}
    public static void Update(SearchResult searchResult)
    {
        // Finds the existing item by matching its unique Guid ID
        var index = SearchResults.FindIndex(r => r.Id == searchResult.Id);
        if (index == -1)
            return;

        SearchResults[index] = searchResult;
    }

    // DELETE: api/search/{id}
    public static void Delete(Guid id)
    {
        var searchResult = Get(id);
        if (searchResult is null) return;
        SearchResults.Remove(searchResult);
    }
}