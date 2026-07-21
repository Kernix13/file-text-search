using System.Net.Http.Json;
using FileTextSearch.Console.Models;

namespace FileTextSearch.Console.Services;

public class FileSearchService
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

    // POST: Move WriteLine to the Program.cs file
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

    // GET: Move WriteLine to the Program.cs file
    public async Task GetAll(HttpClient client)
    {
        var results = await client.GetFromJsonAsync<List<SearchResult>>("/api/search");

        if (results is null)
        {
            System.Console.WriteLine("🚫 No results returned from API.");
            return;
        }

        foreach (var result in results)
        {
            System.Console.WriteLine($"{result.FileName}, {result.FullPath}, {result.Priority}, {result.Category}");
        }
    }

    // GET by Id: Move Writeline and ReadLine to the Program.cs file 
    public async Task GetById(HttpClient client)
    {
        System.Console.WriteLine("📌 Enter the Id of the result you want to view: ");
        string? id = System.Console.ReadLine();
        var result = await client.GetFromJsonAsync<SearchResult>($"/api/search/{id}");
        if (result != null)
        {
            System.Console.WriteLine(result.FileName);
            System.Console.WriteLine(result.FullPath);
        }
        else
        {
            System.Console.WriteLine($"🚫 No result found with ID: {id}");
        }
    }

    // DELETE: Move Writeline and ReadLine to the Program.cs file
    public async Task DeleteById(HttpClient client)
    {
        System.Console.WriteLine("📌 Enter the Id of the result you want to delete: ");
        string? id = System.Console.ReadLine();
        var response = await client.DeleteAsync($"/api/search/{id}");
        if (response.IsSuccessStatusCode)
        {
            System.Console.WriteLine("✅ Result deleted successfully.");
        }
        else
        {
            System.Console.WriteLine($"🚫 Failed to delete result with ID: {id}");
        }
    }

    // UPDATE: Move Writeline and ReadLine to the Program.cs file
    public async Task UpdateById(HttpClient client)
    {
        System.Console.WriteLine("📌 Enter the Id of the result you want to edit: ");
        string? id = System.Console.ReadLine();
        System.Console.WriteLine("📌 Enter the new priority value (High/Low): ");
        string? priority = System.Console.ReadLine();
        if (string.IsNullOrWhiteSpace(id))
        {
            System.Console.WriteLine("🚫 Invalid Id.");
            return;
        }

        if (!Guid.TryParse(id, out var guid))
        {
            System.Console.WriteLine("🚫 Invalid GUID format.");
            return;
        }

        var response = await client.PutAsJsonAsync<SearchResult>($"/api/search/{id}", new SearchResult { Id = guid, Priority = priority ?? "Normal" });
    }

    // Helper method to search files based on the search phrase and user-provided folder
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
                System.Console.WriteLine("🚫 Folder not found.");
                return new List<SearchResult>(); // Exits the method immediately and returns an empty list!
            }
        }

        if (fileType == "")
        {
            fileType = "md";
        }
        else if (!_allowedExtensions.Contains(fileType.ToLower()))
        {
            System.Console.WriteLine($"🚫 Invalid file type, {fileType} not supported.");
            return new List<SearchResult>(); // Stop search immediately for bad file types
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
            // Get the current folder to search and remove it from the list of folders to search 
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
                // Skip the folder
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
                    // Add the subdirectory to the list of folders to search
                    foldersToSearch.Add(directory);
                }

            }
            catch (UnauthorizedAccessException)
            {
                // Skip the folder
            }
        }

        return results;
    }
}

