# Refactor Api project

```sh
dotnet run --project FileTextSearch.Api
dotnet run --project FileTextSearch.Console
npm --prefix FileTextSearch.Web run dev

dotnet test FileTextSearch.Tests
```

`C:\Users\pc\Documents\WebDev\CodeYou`

Notes on moving the Console Code into the Api project so that my React project can interact with the Api.

HOW THE DO I PASS THE FORM FIELD VALUES TO THE C# CODE?

1. Console Program.cs:

Where would this go in the Api project? Add it to FileService.cs?

```cs
using FileTextSearch.Console.Services;

using var client = new HttpClient();
client.BaseAddress = new Uri("http://localhost:5042");

var searchService = new FileSearchService();

// 1. Method for running the search with user input
static async Task RunSearch(HttpClient client, FileSearchService searchService)
{
    // How do I get the search phrase? This code is wrong
    string searchPhrase = Console.ReadLine() ?? "";

    // How do I get the file type? This code is wrong
    string fileType = Console.ReadLine() ?? "";

    // How do I get the folder? This code is wrong
    string userFolder = Console.ReadLine() ?? "";

    // Main search logic in FileSearchService.cs
    var results = await searchService.SearchFiles(searchPhrase, userFolder, fileType);

    // Then post the results found
    await searchService.Create(client, results);
}

// 2. Helper method for GetAll
static async Task RunGetAll(HttpClient client, FileSearchService searchService)
{
    var results = await searchService.GetAll(client);

    if (results is null || results.Count == 0)
    {
        // What do I do here?
        return;
    }
}

// 3. Helper method for GetById
static async Task RunGetById(HttpClient client, FileSearchService searchService)
{
    // How do I get the Id? This code is wrong
    string? id = Console.ReadLine();

    var response = await searchService.GetById(client, id);
}

// 4. Helper method for UpdateById
static async Task RunUpdateById(HttpClient client, FileSearchService searchService)
{
    // How do I get the Id? This code is wrong
    Console.WriteLine("📌 Enter the Id of the result you want to edit: ");
    string? id = Console.ReadLine();

    Console.WriteLine("📌 Enter the new priority value (High/Low): ");
    string? priority = Console.ReadLine();

    var response = await searchService.UpdateById(client, guid, priority ?? "Normal");
}

// 5. Helper method for DeleteById
static async Task RunDeleteById(HttpClient client, FileSearchService searchService)
{
    // How do I get the Id? This code is wrong
    Console.WriteLine("📌 Enter the Id of the result you want to delete: ");
    string? id = Console.ReadLine();

    var response = await searchService.DeleteById(client, id);
}
```

2. Console Services/FileSearchService.cs - I need the entire file but this code is the issue:

```cs
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
```

Can I add the code I need from Console Program.cs into a new API Services file named FileSearchService.cs and then maybe call the methods in SearchService.cs?

3. Web src/App.jsx

```jsx
const handleSubmit = (e) => {
  e.preventDefault();
  console.log(searchPhrase, fileType, folder);
  // call the API here

  // I need to clear the search form when done
};
```
