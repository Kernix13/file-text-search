using System.Net.Http.Json;
using FileTextSearch.Console.Models;

namespace FileTextSearch.Console.Services;

public class FileSearchService
{
    // Not currently used, but will be used for future enhancements
    private readonly string[] _allowedExtensions = new[] { "*.md", "*.txt", "*.html" };

    // Define a list of folders to ignore during the search
    private readonly HashSet<string> _ignoredFolders =
    [
        "bin",
        "obj",
        ".git",
        "node_modules"
    ];

    // POST
    public async Task SearchFiles(HttpClient client, string searchPhrase, string userFolder)
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
                System.Console.WriteLine("Folder not found.");
                return;
            }
        }

        System.Console.WriteLine();
        System.Console.WriteLine($"{rootFolder} will be searched for '{searchPhrase}'");
        System.Console.WriteLine();

        // Initialize a counter for skipped folders
        int skippedFoldersCount = 0;

        // 6. Initialize a list to hold the folders to search, starting with the root folder
        var foldersToSearch = new List<string> { rootFolder };

        // 7. Initialize a list to hold the search results
        List<SearchResult> results = new();

        // 8. Start the search loop, which continues until there are no more folders to search
        while (foldersToSearch.Count > 0)
        {
            // 9. Get the current folder to search and remove it from the list of folders to search 
            var currentFolder = foldersToSearch[0];
            foldersToSearch.RemoveAt(0);

            try
            {
                // 10. Enumerate through all .md files in the current folder
                foreach (var file in Directory.EnumerateFiles(currentFolder, "*.md"))
                {
                    try
                    {
                        string content = File.ReadAllText(file);

                        if (content.Contains(searchPhrase, StringComparison.OrdinalIgnoreCase))
                        {
                            var info = new FileInfo(file);
                            var result = new SearchResult();
                            // Add each md file to the results List
                            results.Add(new SearchResult
                            {
                                FileName = Path.GetFileName(file),
                                FullPath = file,
                                Category = "General",
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
                // 11. Enumerate through all subdirectories in the current folder
                foreach (var directory in Directory.EnumerateDirectories(currentFolder))
                {
                    string folderName = Path.GetFileName(directory);

                    if (_ignoredFolders.Contains(folderName))
                    {
                        skippedFoldersCount++;
                        continue;
                    }
                    // 12. Add the subdirectory to the list of folders to search
                    foldersToSearch.Add(directory);
                }

            }
            catch (UnauthorizedAccessException)
            {
                // Skip the folder
            }
        }

        // POST
        if (results.Count == 0)
        {
            System.Console.WriteLine($"No results found for '{searchPhrase}'.");
        }
        else
        {
            HttpResponseMessage response = await client.PostAsJsonAsync("/api/search", results);

            if (response.IsSuccessStatusCode)
            {
                System.Console.WriteLine("Successfully uploaded search results to the API!");
            }
            else
            {
                string errorReason = await response.Content.ReadAsStringAsync();
                System.Console.WriteLine($"Failed to send data. API responded with: {response.StatusCode} - {errorReason}");
            }

            foreach (var result in results)
            {
                System.Console.WriteLine(result.Id);
                System.Console.WriteLine(result.FileName);
                System.Console.WriteLine(result.FullPath);
                System.Console.WriteLine(result.Category);
                System.Console.WriteLine(result.FileSize);
                System.Console.WriteLine(result.Priority);
                System.Console.WriteLine();
            }

            System.Console.WriteLine($"Skipped {skippedFoldersCount} folders.");
        }
    }

    // GET: Move WriteLine to the Program.cs file
    public async Task GetAll(HttpClient client)
    {
        var results = await client.GetFromJsonAsync<List<SearchResult>>("/api/search");

        if (results is null)
        {
            System.Console.WriteLine("No results returned from API.");
            return;
        }

        foreach (var result in results)
        {
            System.Console.WriteLine($"{result.FileName}, {result.FullPath}, {result.Priority}");
        }
    }

    // GET by Id: Move Writeline and ReadLine to the Program.cs file 
    public async Task GetById(HttpClient client)
    {
        System.Console.WriteLine("Enter the Id of the result you want to view: ");
        string? id = System.Console.ReadLine();
        var result = await client.GetFromJsonAsync<SearchResult>($"/api/search/{id}");
        if (result != null)
        {
            System.Console.WriteLine(result.FileName);
            System.Console.WriteLine(result.FullPath);
        }
        else
        {
            System.Console.WriteLine($"No result found with ID: {id}");
        }
    }

    // DELETE: Move Writeline and ReadLine to the Program.cs file
    public async Task DeleteById(HttpClient client)
    {
        System.Console.WriteLine("Enter the Id of the result you want to delete: ");
        string? id = System.Console.ReadLine();
        var response = await client.DeleteAsync($"/api/search/{id}");
        if (response.IsSuccessStatusCode)
        {
            System.Console.WriteLine("Result deleted successfully.");
        }
        else
        {
            System.Console.WriteLine($"Failed to delete result with ID: {id}");
        }
    }
}

