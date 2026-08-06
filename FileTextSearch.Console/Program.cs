using FileTextSearch.Console.Services;

using var client = new HttpClient();
client.BaseAddress = new Uri("http://localhost:5042");
Console.WriteLine("Client is configured");

var searchService = new FileSearchService();

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
            await RunSearch(client, searchService);
            break;
        case "2":
            Console.WriteLine("");
            await RunGetAll(client, searchService);
            break;
        case "3":
            Console.WriteLine("");
            await RunGetById(client, searchService);
            break;
        case "4":
            Console.WriteLine("");
            await RunUpdateById(client, searchService);
            break;
        case "5":
            Console.WriteLine("");
            await RunDeleteById(client, searchService);
            break;
        case "6":
            return;
        default:
            Console.WriteLine("");
            Console.WriteLine("Invalid choice.");
            break;
    }
}

// 1. Helper method for running the search with user input
static async Task RunSearch(HttpClient client, FileSearchService searchService)
{
    Console.Write("📌 Enter search phrase: ");
    string searchPhrase = Console.ReadLine() ?? "";

    Console.Write(
        @"📌 Enter text file type to search
        Options: md, txt, csv, css, js, json, html
        (Press Enter for md files): ");
    string fileType = Console.ReadLine() ?? "";

    Console.Write(
        @"📌 Enter complete folder path to search
        (Press Enter to use your Documents folder): ");
    string userFolder = Console.ReadLine() ?? "";

    var results = await searchService.SearchFiles(searchPhrase, userFolder, fileType);
    
    await searchService.Create(client, results);

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
}

// 2. Helper method for GetAll
static async Task RunGetAll(HttpClient client, FileSearchService searchService)
{
    var results = await searchService.GetAll(client);

    if (results is null || !results.Any())
    {
        Console.WriteLine("🚫 No results returned from API.");
        return;
    }

    foreach (var result in results)
    {
        Console.WriteLine($"{result.FileName}, {result.FullPath}, {result.Priority}, {result.Category}");
    }
}

// 3. Helper method for GetById
static async Task RunGetById(HttpClient client, FileSearchService searchService)
{
    Console.WriteLine("📌 Enter the Id of the result you want to view: ");
    string? id = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(id))
    {
        Console.WriteLine("🚫 Invalid Id.");
        return;
    }

    if (!Guid.TryParse(id, out var guid))
    {
        Console.WriteLine("🚫 Invalid GUID format.");
        return;
    }

    var response = await searchService.GetById(client, id);


    if (response != null)
    {
        Console.WriteLine(response.FileName);
        Console.WriteLine(response.FullPath);
    }
    else
    {
        Console.WriteLine($"🚫 No result found with ID: {id}");
    }
}

// 4. Helper method for UpdateById
static async Task RunUpdateById(HttpClient client, FileSearchService searchService)
{
    Console.WriteLine("📌 Enter the Id of the result you want to edit: ");
    string? id = Console.ReadLine();
    Console.WriteLine("📌 Enter the new priority value (High/Low): ");
    string? priority = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(id))
    {
        Console.WriteLine("🚫 Invalid Id.");
        return;
    }

    if (!Guid.TryParse(id, out var guid))
    {
        Console.WriteLine("🚫 Invalid GUID format.");
        return;
    }

    var response = await searchService.UpdateById(client, guid, priority ?? "Normal");
    if (response.IsSuccessStatusCode)
    {
        Console.WriteLine("✅ Result updated successfully.");
    }
    else
    {
        Console.WriteLine($"🚫 Failed to update result with ID: {id}");
    }
}


// 5. Helper method for DeleteById
static async Task RunDeleteById(HttpClient client, FileSearchService searchService)
{
    Console.WriteLine("📌 Enter the Id of the result you want to delete: ");
    string? id = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(id))
    {
        Console.WriteLine("🚫 Invalid Id.");
        return;
    }
    var response = await searchService.DeleteById(client, id);
    if (response.IsSuccessStatusCode)
    {
        Console.WriteLine("✅ Result deleted successfully.");
    }
    else
    {
        Console.WriteLine($"🚫 Failed to delete result with ID: {id}");
    }
}