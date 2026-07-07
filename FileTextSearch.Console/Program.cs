using System.Text.Json;
using FileTextSearch.Console.Models;

Console.Write("Enter search phrase: ");
string searchPhrase = Console.ReadLine() ?? "";

/* Manually set specific folder to search
   Give the user the option to choose or default to Documents
   Here is an example to search WebDev/CodeYou for my laptop: */
// string rootFolder = Path.Combine(
//     Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
//     "WebDev",
//     "CodeYou"
// );

string rootFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

// string searchPhrase = "zazaza";

HashSet<string> ignoredFolders = new()
{
    "bin",
    "obj",
    ".git",
    "node_modules"
};

int skippedFoldersCount = 0;

var foldersToSearch = new List<string> { rootFolder };

// The code only searches for .md files but I will also include .txt and .html files in the future, and maybe .csv and .json. A stretch goal will be to search .docx and .xls files.
string[] allowedExtensions = ["*.md", "*.txt", "*.html"];

while (foldersToSearch.Count > 0)
{
    var currentFolder = foldersToSearch[0];
    foldersToSearch.RemoveAt(0);

    try
    {
        foreach (var file in Directory.EnumerateFiles(currentFolder, "*.md"))
        {
            try
            {
                string content = File.ReadAllText(file);

                if (content.Contains(searchPhrase, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine(Path.GetFileName(file));
                    Console.WriteLine(file);
                    Console.WriteLine("-------------------------------");
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
        foreach (var directory in Directory.EnumerateDirectories(currentFolder))
        {
            string folderName = Path.GetFileName(directory);

            if (ignoredFolders.Contains(folderName))
            {
                skippedFoldersCount++;
                continue;
            }

            foldersToSearch.Add(directory);
        }

    }
    catch (UnauthorizedAccessException)
    {
        // Log it or just silently skip the folder
        // Console.WriteLine($"Skipped (Access Denied): {currentFolder}");
    }
}

Console.WriteLine($"Skipped {skippedFoldersCount} folders.");