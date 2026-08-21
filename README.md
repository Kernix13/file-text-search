# File Text Search API using C#

This project will enable the user to search a word or phrase and return the full path for any text file (.md, .txt, .html) that contains the search phrase. Implementation only for markdown files at this point. The point of the project is to consolidate notes in various files into a master file or folder by subject.

## Table of Contents

1. [Prerequisites](#prerequisites)
1. [API Project](#api-project)
1. [Console Project](#console-project)
1. [Testing Project](#testing-project)
1. [React Project](#react-project)
1. [Project Structure](#project-structure)
1. [Tech Stack](#tech-stack)
1. [Acknowledgments & Resources](#acknowledgments--resources)
1. [Future Improvements](#future-improvements)
<!-- 1. [Capstone Requirements](#capstone-requirements)
1. [Capstone Questions](#capstone-questions)
1. [AI Usage](#ai-usage) -->
<!-- 1. [Contributing](#contributing)
1. [License](#license) -->

<span aria-hidden="true"><br></span>

## Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/en-us/download) 10.0
- [Visual Studio Code](https://code.visualstudio.com/) with [C# Dev Kit](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csdevkit)
- [Node.js](https://nodejs.org/en)
- [React](https://react.dev/)

<span aria-hidden="true"><br></span>

## Installation & Usage

1. Clone this repository and switch into project folder

   ```sh
   git clone https://github.com/Kernix13/file-text-search.git
   cd file-text-search
   ```

2. Start the API then visit `http://localhost:5042/scalar/`:

   ```bash
   dotnet run --project FileTextSearch.Api
   ```

3. Open 2nd terminal and run the Console app

   ```sh
   dotnet run --project FileTextSearch.Console
   ```

4. Run xUnit unit tests:

   ```sh
   dotnet test FileTextSearch.Tests
   ```

5. View React front end UI:

   ```sh
   cd FileTextSearch.Web
   npm install
   npm run dev
   ```

<span aria-hidden="true"><br></span>

## API Project

### Run the API

```bash
# From project root:
dotnet run --project FileTextSearch.Api
```

Then go to `http://localhost:5042/scalar` to interact with the in-memory API. The API is at `http://localhost:5042/api/search`.

### Files

- [Controllers/SearchController.cs](./FileTextSearch.Api/Controllers/SearchController.cs)
- [Models/Searchresult.cs](./FileTextSearch.Api/Models/SearchResult.cs)
- [Services/SearchService.cs](./FileTextSearch.Api/Services/SearchService.cs)
- [Program.cs](./FileTextSearch.Api/Program.cs)
<!-- - FileTextSearch.Api.http -->

<span aria-hidden="true"><br></span>

## Console Project

### Run the Console app

```bash
# From project root:
dotnet run --project FileTextSearch.Console
```

### Console menu

```
1. Search Files
2. View All Search Results
3. View Search Result by Id
4. Update Search Result
5. Delete Search Result
6. Exit
```

### How to use

1. Choose menu item 1 to "Search Files"
2. Enter a search phrase that you know exists in a text file of some sort (md, txt, html, etc.)
3. You can enter the specific text file extension or hit <kbd>ENTER</kbd> to accept default of `md` for markdown files.
4. I would suggest accepting the default folder of `Documents` or whatever `Environment.SpecialFolder.MyDocuments` returns for your machine.

You should then see output for the files found that have the search phrase in the file(s).

The problem with entering a specific folder to search is because you need the entire filepath, e.g.: `C:\Users\pc\Documents\WebDev\CodeYou\`.

<!--
Once the React UI is handling the API, the Console project will change to something similar to this:

```
========== File Text Search ==========

Search complete!

Search phrase: "css reset"

Files scanned: 482
Matching files: 17
Folders searched: 26
Elapsed time: 0.43 seconds

Matches by folder
-----------------
CodeYou/module-2     8
CSS/css-essentials   5
markdown-repos       1
Traversy/React       3

Results successfully posted to the API.
```
-->

### Files

- [Models/SearchResult.cs](./FileTextSearch.Console/Models/SearchResult.cs) (same as Api Models file)
- [Services/FileSearchService.cs](./FileTextSearch.Console/Services/FileSearchService.cs)
- [Program.cs](./FileTextSearch.Console/Program.cs)

<span aria-hidden="true"><br></span>

## Testing Project

```bash
# Run the tests from root
dotnet test # or
dotnet test FileTextSearch.Tests
```

I have 3 tests in [SearchServiceTests.cs](./FileTextSearch.Tests/SearchServiceTests.cs):

1. POST: adding a search result
2. GET: Get all results
3. DELETE: Delete a result given its Id

<span aria-hidden="true"><br></span>

## React Project

### Run the React UI app

```bash
cd FileTextSearch.Web
# Install dependencies
npm install
# Start the development server
npm run dev

# Or run from the root:
npm --prefix FileTextSearch.Web run dev
```

Then open `http://localhost:5173/`

A form in the UI will replace the Console menu and then display the search results. A temporary form is in place but it is not functional - I am unsure how to connect to the API from React. I am writing the search results to the UI that are generated by the Console project.

<span aria-hidden="true"><br></span>

## Project Structure

<!-- Remember to add a `.github` folder with templates for issues and pull requests. -->

```python
file-text-search/
├── FileTextSearch.Api/
│   ├── Controllers/
│   │   └── SearchController.cs
│   ├── Models/
│   │   └── SearchResult.cs
│   ├── Services/
│   │   └── SearchService.cs
│   └── Program.cs
├── FileTextSearch.Console/
│   ├── Models/
│   │   └── SearchResult.cs
│   ├── Services/
│   │   └── FileSearchService.cs
│   └── Program.cs
├── FileTextSearch.Tests/
│   └── SearchServiceTests.cs
├── FileTextSearch.Web/
│   ├── public/
│   ├── src/
│   ├── package.json
│   └── vite.config.js
├── .gitignore
├── FileTextSearch.slnx
└── README.md
```

<span aria-hidden="true"><br></span>

## Tech Stack

1. [.NET SDK 10.0](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)
2. [C# Dev Kit VS Code extension](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csdevkit)
3. [xUnit testing framework](https://xunit.net/?tabs=cs)
4. [Node.js](https://nodejs.org/en)
5. [React](https://react.dev/)

<span aria-hidden="true"><br></span>

<span aria-hidden="true"><br></span>

## Acknowledgments & Resources

1. [Dependency injection for .NET APIs](https://youtu.be/LpBdpoHD50I): This was helpful for how to inject my services into Program.cs for use there.
2. [Full API Pattern with .NET 9](https://youtu.be/W_1eW_hBlmw): Various API features including depending injection and using Scalar.
3. [xUnit advanced Assert methods: Throws, IsType + more](https://youtu.be/Z7-3MV-7fGk): covers `Assert.Single` & `Assert.Empty` xUnit methods which I used in 2 of my test methods.

<span aria-hidden="true"><br></span>

## Future Improvements

1. Fix the code that uses `AllowAny...` in the API project involving CORS (see codeblock below)
2. I already allow the user to select a specific folder to search, but you have to type out the full folder path. It would be better to somehow allow the user to "browse" their system folders.
   - ✅ I have added this but you need the full path like `C:/Users/pc/Documents/WebDev/CodeYou/`. That is cumbersome
   - Another option is to allow the user to just add the path _AFTER_ `Documents/`, such as `WebDev/CodeYou/`
3. Change user prompt and allow multiple search phrases separated by a comma, then `Split` on the comma and `Trim` whitespace
4. Search other file types: _.json_ and _.csv_ will be easy, _.docx_ and _.xls_ will require a Nuget package
5. I want to also be able to search for filename + extension like `reset.css`

```cs
// I need to edit this code in Program.cs for the API project:
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
```

<!--
## Capstone Requirements

> Due by Noon on August 14th, 2026

Project must contain three different projects:

1. ✅ A C# web API using controllers or minimal API -> [FileTextSearch.Api](https://github.com/Kernix13/file-text-search/tree/main/FileTextSearch.Api)
2. ✅ A project that consumes that API, console application or a web site -> [FileTextSearch.Console](https://github.com/Kernix13/file-text-search/tree/main/FileTextSearch.Console) and [FileTextSearch.Web](https://github.com/Kernix13/file-text-search/tree/main/FileTextSearch.Web)
3. ✅ A test project to prove that your code works -> [FileTextSearch.Tests](https://github.com/Kernix13/file-text-search/tree/main/FileTextSearch.Tests)

Basic requirements:

- ✅ Submitted as a single GitHub repository
- ✅ The solution builds successfully
- ✅ Uses ASP.NET and C# to expose a web API
- ✅ A CRUD project that uses those web APIs for data stored in-memory
- ✅ Automated tests written, pass, and cover a significant portion of execution paths
- Your project is documented in a README.md file that contains
  - ✅ App's name and intended purpose
  - ✅ How to build and run your application
  - ✅ What you learned from this project/course
  - ✅ What would you have done differently
  - ✅ What additional features would you have added

<span aria-hidden="true"><br></span>

## Capstone Questions

1. Why this project?
   - The search results in Windows File Explorer returns results that Ido not want, so I wanted to create a custom search to organize all my notes on web development.
2. What did I learn from this project?
   - I have a better understanding of Object Oriented Programming and insight into how to build and implement an API.
3. What did I learn from the Code:You C# Software Development pathway?
   - I found C# difficult but understanding the importance of data types was important to learn. I hope to convert my JavaScript projectsto TypeScript in the near future.
4. What would I have done differently for this project?
   - I wish that I couldh ave used a database and create aJSON file rather than use in memory for the data.

## AI Usage

1. I originally wanted to create a JSON file for my search results (POST) whichI did, but I was not able to to get GET working so I asked ChatGPT what the problem was. It showed me code to fix it, but since it was not what was covered in any of the lessons, I abandoned that approach and changed to "in memory" for the API.
2. My tests were failing in random order so I asked GPT why after showing the errors. It was because of my `static` class and methods in `SearchService.cs`. I removed the `static` keyword and made changes in `SearchController.cs`, `Program.cs`, and `SearchServiceTests.cs`.
3. I asked ChatGPT how to run the React project from the root and it told me about the `--prefix` option: `npm --prefix FileTextSearch.Web run dev`. That makes things a little easier.
4. I used ChatGPT to explain the many errors I got in my console when trying to run my project. They were not always easy to interpret.
5. ChatGPT helped me with the process of searching folders with the code on ~ line #'s 137-138 in `Console/FileSearchService.cs`
   - I was having difficulty with searching folders and files during the early stages of the project (see code block below):

```cs
// Get the current folder to search and remove it from the list of folders to search - AI Usage #5
var currentFolder = foldersToSearch[0];
foldersToSearch.RemoveAt(0);
```
-->
