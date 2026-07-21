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
            await searchService.GetAll(client);
            break;
        case "3":
            Console.WriteLine("");
            await searchService.GetById(client);
            break;
        case "4":
            Console.WriteLine("");
            await searchService.UpdateById(client);
            break;
        case "5":
            Console.WriteLine("");
            await searchService.DeleteById(client);
            break;
        case "6":
            return;
        default:
            Console.WriteLine("");
            Console.WriteLine("Invalid choice.");
            break;
    }
}

// Helper method for running the search with user input
static async Task RunSearch(HttpClient client, FileSearchService searchService)
{
    Console.Write("Enter search phrase: ");
    string searchPhrase = Console.ReadLine() ?? "";

    Console.Write(
        @"Enter text file type to search
        Options: md, txt, csv, css, js, json, html
        (Press Enter for md files): ");
    string fileType = Console.ReadLine() ?? "";

    Console.Write(
        @"Enter complete folder path to search
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

// Should I create helper methods for the other options? I don't think I should have WriteLine or ReadLine statements in the service class.


