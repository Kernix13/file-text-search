using System.Net.Http.Json;
using FileTextSearch.Console.Models;

namespace FileTextSearch.Console.Services;

public class FileSearchService
{
    // Add or remove your preferred plain-text file type extensions here:
    // Currently not being implemented
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

    /* Helper method to search files based on the search phrase, and user provider file type and folder. */
    public async Task<List<SearchResult>> SearchFiles(string searchPhrase, string userFolder, string fileType)
    {

        string rootFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        // Validate the user-provided folder path, if any, and set it as the root folder
        if (userFolder != "")
        {
            if (Directory.Exists(userFolder))
            {
                rootFolder = userFolder;
            }
            else
            {
                System.Console.WriteLine($"🚫 Folder '/{userFolder}' not found.");
                return new List<SearchResult>();
            }
        }

        if (fileType == "")
        {
            fileType = "md";
        }
        else if (!_allowedExtensions.Contains(fileType.ToLower()))
        {
            System.Console.WriteLine($"🚫 Invalid file type, {fileType} not supported.");
            return new List<SearchResult>(); 
        }

        System.Console.WriteLine();
        System.Console.WriteLine($"✅ {rootFolder} will be searched for '{searchPhrase}'");
        System.Console.WriteLine();

        // Initialize a counter for skipped folders
        skippedFoldersCount = 0;

        // Initialize a list to hold the folders to search, starting with the root folder
        var foldersToSearch = new List<string> { rootFolder };

        // Initialize a list to hold the search results
        List<SearchResult> results = new();

        // Start the search loop, which continues until there are no more folders to search
        while (foldersToSearch.Count > 0)
        {
            // Get the current folder to search and remove it from the list of folders to search - AI Usage #5
            var currentFolder = foldersToSearch[0];
            foldersToSearch.RemoveAt(0);

            try
            {
                // Enumerate through all .md files in the current folder
                foreach (var file in Directory.EnumerateFiles(currentFolder, $"*.{fileType}"))
                {
                    try
                    {
                        string content = File.ReadAllText(file);

                        if (content.Contains(searchPhrase, StringComparison.OrdinalIgnoreCase))
                        {
                            var info = new FileInfo(file);
                            var result = new SearchResult();
                            // Folder path after 'Documents' or user folder
                            var relativePath = file
                                .Replace(rootFolder, "")
                                .Trim('\\');
                            // Add each md file to the results List
                            results.Add(new SearchResult
                            {
                                FileName = Path.GetFileName(file),
                                FullPath = file,
                                Category = Path.GetDirectoryName(relativePath) ?? "General",
                                FileSize = info.Length,
                                Priority = "Normal"
                            });
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Console.WriteLine($"Skipping {file}: {ex.Message}");
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {
                // Skip the folder if access is denied and continue the search
            }

            try
            {
                // Enumerate through all subdirectories in the current folder
                foreach (var directory in Directory.EnumerateDirectories(currentFolder))
                {
                    string folderName = Path.GetFileName(directory);

                    if (_ignoredFolders.Contains(folderName))
                    {
                        skippedFoldersCount++;
                        continue;
                    }
                    // Add each subdirectory to the list of folders to search
                    foldersToSearch.Add(directory);
                }

            }
            catch (UnauthorizedAccessException)
            {
                // Skip the folder if access is denied and continue the search
            }
        }

        return results;
    }
}