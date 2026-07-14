using System.Net.Http.Json;
using FileTextSearch.Console.Models;
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
            // await SearchFiles(client);
            // await searchService.SearchFiles(client);
            await RunSearch(client, searchService);
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

// Helper method for running the search with user input
static async Task RunSearch(HttpClient client, FileSearchService searchService)
{
    Console.Write("Enter search phrase: ");
    string searchPhrase = Console.ReadLine() ?? "";

    Console.Write(
        @"Enter complete folder path to search
        (Press Enter to use your Documents folder): ");
    string userFolder = Console.ReadLine() ?? "";

    await searchService.SearchFiles(client, searchPhrase, userFolder);
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


