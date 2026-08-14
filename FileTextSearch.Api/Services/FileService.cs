using System.Net.Http.Json;
using FileTextSearch.Api.Models;

namespace FileTextSearch.Api.Services;

public class FileService
{
    // Add or remove your preferred plain-text file type extensions here:
    private readonly string[] _allowedExtensions = new[] {
        "md", "mdx",
        "txt", "csv",
        "css",
        "html",
        "cs",
        "py",
        "js", "ts", "jsx", "tsx",
        "json"
    };

    // Define a list of folders to ignore during the search
    private readonly HashSet<string> _ignoredFolders =
    [
        "bin",
        "obj",
        ".git",
        "node_modules"
    ];

    int skippedFoldersCount;

    // POST
    public async Task Create(HttpClient client, List<SearchResult> results)
    {
        if (results.Count == 0)
        {
            System.Console.WriteLine($"🚫 No results found for search phrase.");
        }
        else
        {
            HttpResponseMessage response = await client.PostAsJsonAsync("/api/search", results);

            if (response.IsSuccessStatusCode)
            {
                System.Console.WriteLine("✅ Successfully uploaded search results to the API!");
            }
            else
            {
                string errorReason = await response.Content.ReadAsStringAsync();
                System.Console.WriteLine($"Failed to send data. API responded with: {response.StatusCode} - {errorReason}");
            }

            // Does the user need this information?
            System.Console.WriteLine($"💡 Skipped {skippedFoldersCount} folders.");
            System.Console.WriteLine();
        }
    }

    // GET
    public async Task<List<SearchResult>?> GetAll(HttpClient client)
    {
        return await client.GetFromJsonAsync<List<SearchResult>>("/api/search");
    }

    // GET
    public async Task<SearchResult?> GetById(HttpClient client, string id)
    {
        var response = await client.GetAsync($"/api/search/{id}");

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<SearchResult>();
        }

        return null;
    }

    // DELETE
    public async Task<HttpResponseMessage> DeleteById(HttpClient client, string id)
    {
        return await client.DeleteAsync($"/api/search/{id}");
    }

    // PUT
    public async Task<HttpResponseMessage> UpdateById(HttpClient client, Guid id, string priority)
    {
        return await client.PutAsJsonAsync<SearchResult>($"/api/search/{id}", new SearchResult { Id = id, Priority = priority });
    }

    /* Helper method to search files based on the search phrase, and user provider file type and folder. This is where the difficulty starts because I need searchPhrase, userFolder, and fileType
    1. searchPhrase: The text that the user wants to search for within the files.
    2. userFolder: The folder path where the user wants to perform the search.
    3. fileType: The type of files the user wants to search within (e.g., .txt, .md, .cs, etc.).
    The method should return a list of SearchResult objects that match the search criteria. 
    */
    // public async Task<List<SearchResult>> SearchFiles(string searchPhrase, string userFolder, string fileType) {...}

}