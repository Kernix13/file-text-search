using System.Net.Http.Json;
using FileTextSearch.Console.Models;

using var client = new HttpClient();
client.BaseAddress = new Uri("http://localhost:5042");
Console.WriteLine("Client is configured");

while (true)
{
    Console.WriteLine("");
    Console.WriteLine("1. Search Files");
    Console.WriteLine("2. View All Search Results");
    Console.WriteLine("3. View Search Result by Id");
    Console.WriteLine("4. Update Search Result");
    Console.WriteLine("5. Delete Search Result");
    Console.WriteLine("6. Exit");

    string choice = Console.ReadLine() ?? "";
    switch (choice)
    {
        case "1":
            await SearchFiles(client);
            break;
        case "2":
            Console.WriteLine("");
            await GetAll(client);
            break;
        case "3":
            Console.WriteLine("");
            await GetById(client);
            break;
        case "4":
            Console.WriteLine("");
            await UpdateById(client);
            break;
        case "5":
            Console.WriteLine("");
            await DeleteById(client);
            break;
        case "6":
            return;
        default:
            Console.WriteLine("");
            Console.WriteLine("Invalid choice.");
            break;
    }
}

static async Task SearchFiles(HttpClient client)
{

    // 1. Get the search phrase from the user
    Console.Write("Enter search phrase: ");
    string searchPhrase = Console.ReadLine() ?? "";

    // 2. Get the root folder to search from the user, or default to Documents
    Console.Write(@"Enter complete folder path to search 
    (Press Enter to use your Documents folder): ");
    string userFolder = Console.ReadLine() ?? "";

    string rootFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

    // 3. Validate the user-provided folder path, if any, and set it as the root folder
    if (userFolder != "")
    {
        if (Directory.Exists(userFolder))
        {
            rootFolder = userFolder;
        }
        else
        {
            Console.WriteLine("Folder not found.");
            return;
        }
    }

    Console.WriteLine();
    Console.WriteLine($"{rootFolder} will be searched for '{searchPhrase}'");
    Console.WriteLine();

    // 4. Define a list of folders to ignore during the search
    HashSet<string> ignoredFolders = new()
    {
        "bin",
        "obj",
        ".git",
        "node_modules"
    };

    // 5. Initialize a counter for skipped folders and a list to hold the folders to search
    int skippedFoldersCount = 0;

    /*  📌 The code only searches for .md files  right now but I will also 
        include .txt and .html files in the future, and maybe .csv and .json. 
        A stretch goal will be to search .docx and .xls files. */
    string[] allowedExtensions = ["*.md", "*.txt", "*.html"];

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
                    Console.WriteLine($"Skipping {file}: {ex.Message}");
                }
            }
        }
        catch (UnauthorizedAccessException)
        {
            // Log it or just silently skip the folder
            // Console.WriteLine($"Skipped (Access Denied): {currentFolder}");
        }

        try
        {
            // 11. Enumerate through all subdirectories in the current folder
            foreach (var directory in Directory.EnumerateDirectories(currentFolder))
            {
                string folderName = Path.GetFileName(directory);

                if (ignoredFolders.Contains(folderName))
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
            // Log it or just silently skip the folder
            // Console.WriteLine($"Skipped (Access Denied): {currentFolder}");
        }
    }

    // POST
    if (results.Count == 0)
    {
        Console.WriteLine($"No results found for '{searchPhrase}'.");
    }
    else
    {
        HttpResponseMessage response = await client.PostAsJsonAsync("/api/search", results);

        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine("Successfully uploaded search results to the API!");
        }
        else
        {
            string errorReason = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Failed to send data. API responded with: {response.StatusCode} - {errorReason}");
        }

        foreach (var result in results)
        {
            Console.WriteLine(result.Id);
            Console.WriteLine(result.FileName);
            Console.WriteLine(result.FullPath);
            Console.WriteLine(result.Category);
            Console.WriteLine(result.FileSize);
            Console.WriteLine(result.Priority);
            Console.WriteLine();
        }

        Console.WriteLine($"Skipped {skippedFoldersCount} folders.");
    }
}

// GET
static async Task GetAll(HttpClient client)
{
    var results = await client.GetFromJsonAsync<List<SearchResult>>("/api/search");

    if (results is null)
    {
        Console.WriteLine("No results returned from API.");
        return;
    }

    foreach (var result in results)
    {
        Console.WriteLine($"{result.FileName}, {result.FullPath}, {result.Priority}");
    }
}

// GET by Id: GetAsync or GetFromJsonAsync 
static async Task GetById(HttpClient client)
{
    Console.WriteLine("Enter the Id of the result you want to view: ");
    string? id = Console.ReadLine();
    var result = await client.GetFromJsonAsync<SearchResult>($"/api/search/{id}");
    if (result != null)
    {
        Console.WriteLine(result.FileName);
        Console.WriteLine(result.FullPath);
    }
    else
    {
        Console.WriteLine($"No result found with ID: {id}");
    }
}

// DELETE: 
static async Task DeleteById(HttpClient client)
{
    Console.WriteLine("Enter the Id of the result you want to delete: ");
    string? id = Console.ReadLine();
    var response = await client.DeleteAsync($"/api/search/{id}");
    if (response.IsSuccessStatusCode)
    {
        Console.WriteLine("Result deleted successfully.");
    }
    else
    {
        Console.WriteLine($"Failed to delete result with ID: {id}");
    }
}

// UPDATE:  
static async Task UpdateById(HttpClient client)
{
    Console.WriteLine("Enter the Id of the result you want to edit: ");
    string? id = Console.ReadLine();
    Console.WriteLine("Enter the new priority value (High/Low): ");
    string? priority = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(id))
    {
        Console.WriteLine("Invalid Id.");
        return;
    }

    if (!Guid.TryParse(id, out var guid))
    {
        Console.WriteLine("Invalid GUID format.");
        return;
    }

    var response = await client.PutAsJsonAsync<SearchResult>($"/api/search/{id}", new SearchResult { Id = guid, Priority = priority ?? "Normal" });
}


