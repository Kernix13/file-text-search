using System;
using System.IO;
using System.Text.Json;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
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
            // Console.WriteLine("All results");
            await GetAll(client);
            break;
        case "3":
            Console.WriteLine("");
            Console.WriteLine("Your result");
            // GetById();
            break;
        case "4":
            Console.WriteLine("");
            Console.WriteLine("Result to update");
            // UpdateById();
            break;
        case "5":
            Console.WriteLine("");
            Console.WriteLine("Result to delete");
            // DeleteById();
            break;
        case "6":
            return;
        default:
            Console.WriteLine("");
            Console.WriteLine("Invalid choice.");
            break;
    }
}


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
        Console.WriteLine($"{result.FileName}, {result.FullPath}");
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

    // GET all results: GetAsync

    // GET by Id: GetAsync

    // POST
    if (results.Count == 0)
    {
        Console.WriteLine($"No results found for '{searchPhrase}'.");
    }
    else
    {
        // 13. Serialize the results to JSON and save them to a file
        // See ConsoleNotes.md for code


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
// UPDATE: PutAsJsonAsync

// DELETE: 