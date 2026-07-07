using FileTextSearch.Api.Models;

namespace FileTextSearch.Services;

public static class SearchService
{
    static List<SearchResult> SearchResults { get; }

    static SearchService()
    {
        // Your mock data will now automatically get assigned a random Guid upon creation!
        SearchResults = new List<SearchResult>
        {
            new SearchResult { FileName = "General.md", FullPath = "C:\\Users\\pc\\Documents\\WebDev\\CodeYou\\CSharp\\General.md", Category = "General", FileSize = "12 KB", Priority = "Normal" },
            new SearchResult { FileName = "md1.md", FullPath = "C:\\Users\\pc\\Documents\\WebDev\\CodeYou\\testsearch\\md1.md", Category = "WebDev/CodeYou/CSharp", FileSize = "10 KB", Priority = "Normal" },
            new SearchResult { FileName = "pseudocode.md", FullPath = "C:\\Users\\pc\\Documents\\WebDev\\CodeYou\\CSharp\\Capstone\\pseudocode.md", Category = "General", FileSize = "1 KB", Priority = "Normal" },
            new SearchResult { FileName = "starter-code.md", FullPath = "C:\\Users\\pc\\Documents\\WebDev\\CodeYou\\CSharp\\Capstone\\starter-code.md", Category = "General", FileSize = "0 KB", Priority = "Normal" }
        };
    }

    public static List<SearchResult> GetAll() => SearchResults;

    // Change parameter type to Guid
    public static SearchResult? Get(Guid id) => SearchResults.FirstOrDefault(result => result.Id == id);

    public static void Add(SearchResult searchResult)
    {
        SearchResults.Add(searchResult);
    }

    public static void Delete(Guid id)
    {
        var searchResult = Get(id);
        if (searchResult is null) return;
        SearchResults.Remove(searchResult);
    }

    public static void Update(SearchResult searchResult)
    {
        // Finds the existing item by matching its unique Guid ID
        var index = SearchResults.FindIndex(r => r.Id == searchResult.Id);
        if (index == -1)
            return;

        SearchResults[index] = searchResult;
    }


}