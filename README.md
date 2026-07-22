# File Text Search API using C#

This project will enable the user to search a word or phrase and return the full path for any text file (.md, .txt, .html) that contains the search phrase. Implementation only for markdown files at this point.

The point of the project is to consolidate notes in various files, into a master file or folder. But you have to track down your notes by subject first. For example, I have multiple files and notes involving CSS resets. These notes and files are spread across many folders and I can't always remember where they are.

<!-- Here is the structure this project will have when complete:

1. FileTextSearch.Api/ (✅ Done)
2. FileTextSearch.Console/ (✅ Done)
   - Menu done ✅
   - Full CRUD done ✅
3. FileTextSearch.Tests/ using xUnit (✅ Done)
4. FileTextSearch.Web/ using React (📌 Next) -->

## Table of Contents

1. [Prerequisites](#prerequisites)
1. [API Project](#api-project)
1. [Console Project](#console-project)
1. [Testing Project](#testing-project)
1. [React Project](#react-project)
1. [Project Structure](#project-structure)
1. [Tech Stack](#tech-stack)
1. [Capstone Requirements](#capstone-requirements)
1. [Capstone Questions](#capstone-questions)
1. [AI Usage](#ai-usage)
1. [Acknowledgments & Resources](#acknowledgments--resources)
1. [Future Improvements](#future-improvements)
1. [Contributing](#contributing)
1. [License](#license)

<span aria-hidden="true"><br></span>

## Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/en-us/download) 10.0
- [Visual Studio Code](https://code.visualstudio.com/) with [C# Dev Kit](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csdevkit)

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

4. Run xUnit tests from project root:

   ```sh
   dotnet test
   ```

5. View React front end UI (not added yet):

   ```sh
   cd FileTextSearch.Web
   npm install
   npm run dev
   ```

<span aria-hidden="true"><br></span>

## API Project

### Run the API

```bash
# From project root
dotnet run --project FileTextSearch.Api # or
dotnet run -p FileTextSearch.Api
```

Then go to `http://localhost:5042/scalar` to interact with the in-memory API. The API is at `http://localhost:5042/api/search`.

<span aria-hidden="true"><br></span>

## Console Project

Run the Console app

```bash
# From project root
dotnet run --project FileTextSearch.Console # or
dotnet run -p FileTextSearch.Console
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

I still need to add error handling for edge cases and bad input.

<span aria-hidden="true"><br></span>

## Testing Project

```bash
# Run the tests from root
dotnet test
```

I have 3 tests

1. POST: adding a search result
2. GET: Get all results
3. DELETE: Delete a result given its Id

<span aria-hidden="true"><br></span>

## React Project

Run the React UI app (not added to project yet)

```bash
cd FileTextSearch.Web
npm install
npm run dev
# open http://localhost:5195
```

I intend to use a form in place of teh Console menu and then display the search results in the UI.

<span aria-hidden="true"><br></span>

## Project Structure

Remember to add a `.github` folder with templates for issues and pull requests.

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
├── .gitignore
├── FileTextSearch.slnx
└── README.md
```

If I add React, add this to above

```python
├── FileTextSearch.Web/
│   ├── src/
│   ├── public/
│   ├── package.json
│   └── vite.config.js
```

<span aria-hidden="true"><br></span>

## Tech Stack

1. [.NET SDK 10.0](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)
2. [C# Dev Kit VS Code extension](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csdevkit)
3. [xUnit testing framework](https://xunit.net/?tabs=cs)
4. [Node.js](https://nodejs.org/en) (not added yet)
5. [React](https://react.dev/) (not added yet)

<span aria-hidden="true"><br></span>

## Capstone Requirements

> Due by Noon on August 14th, 2026

Project must contain three different projects:

1. ✅ A C# web API using controllers or minimal API -> [FileTextSearch.Api](https://github.com/Kernix13/file-text-search/tree/main/FileTextSearch.Api)
2. ✅ A project that consumes that API, console application or a web site -> [FileTextSearch.Console](https://github.com/Kernix13/file-text-search/tree/main/FileTextSearch.Console)
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
   - I have a better understanding of Object OrientedProgramming and insight into how to build and implement an API.
3. What did I learn from the Code:You C# Software Development pathway?
   - I found C# difficult but understanding the importance of data types was important to learn. I hope to convert my JavaScript projectsto TypeScript in the near future.
4. What would I have done differently for this project?
   - I wish that I couldh ave used a database and create aJSON file rather than use in memory for the data.

<span aria-hidden="true"><br></span>

## AI Usage

1. I originally wanted to create a JSON file for my search results (POST) whichI did, but I was not able to to get GET working so I asked ChatGPT what the problem was. It showed me code to fix it, but since it was not what was covered in any of the lessons, I abandoned that approach and changed to "in memory" for the API.
2. My tests were failing in random order so I asked GPT why after showing the errors. It was because of my `static` class and methods in `SearchService.cs`. I removed the `static` and made changes in SearchController.cs, Program.cs, and SearchServiceTests.cs

<span aria-hidden="true"><br></span>

## Acknowledgments & Resources

1. [Dependency injection for .NET APIs](https://youtu.be/LpBdpoHD50I): This was helpful for how to inject my services into Program.cs for use there.
2.

<span aria-hidden="true"><br></span>

## Future Improvements

1. I already allow the user to select a specific folder to search, but you have to type out the full folder path. It would be better to somehow allow the user to "browse" their system folders.
   - ✅ I have added this but you need the full path like `C:/Users/pc/Documents/WebDev/CodeYou`. That is cumbersome
2. Change user prompt and allow multiple search phrases separated by a comman, then `Split` on the comma and `Trim` whitespace
3. Search other file types: _.json_ and _.csv_ will be easy, _.docx_ and _.xls_ will require a Nuget package
4. I want to also be able to search for filename + extension like `reset.css`
5. ...

<span aria-hidden="true"><br></span>

## Contributing

Contributions are welcome! If you'd like to help improve this project, please read our _contribution guidelines_ on how to get started, our workflow, and code style expectations.

> Add a link for "contribution guidelines" as soon as I add that file. Also add a code of conduct file.

<span aria-hidden="true"><br></span>

## License

This project is licensed under the MIT License.

> Add a link for "MIT License" as soon as I add it.
