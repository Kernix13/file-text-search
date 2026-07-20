# File Text Search API using C#

This project will enable the user to search a word or phrase and return the full path for any text file (.md, .txt, .html) that contains the search phrase. Implementation only for markdown files at this point.

The point of the project is to consolidate notes in various files, into a master file or folder. But you have to track down your notes by subject first. For example, I have multiple files and notes involving CSS resets. These notes and files are spread across many folders and I can't always remember where they are.

Here is the structure this project will have when complete:

1. FileTextSearch.Api/ (✅ Done)
2. FileTextSearch.Console/ (✅ Done)
   - Menu done ✅
   - Full CRUD done ✅
3. FileTextSearch.Tests/ using xUnit (✅ Done)
4. FileTextSearch.Web/ using React (📌 Next)

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

4. Run xUnit tests:

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

<!--
cd FileTextSearch.Api
dotnet add package Scalar.AspNetCore
 -->

Then go to `http://localhost:5042/scalar` to interact with the API. The API is at `http://localhost:5042/api/search`.

### Program.cs methods:

- `using Scalar.AspNetCore`:
- `app.MapScalarApiReference()`:
- `builder.Services.AddSingleton()`:

### Controllers methods:

- HttpGet -> GetAll -> ActionResult SearchService.GetAll -> return Ok()
- HttpGet -> GetById -> ActionResult SearchService.Get -> return Ok()
- HttpPost -> Create -> ActionResult SearchService.Add -> return Ok()
- HttpPut -> Update -> IActionResult SearchService.Get -> return NoContent()
- HttpDelete -> Delete -> IActionResult SearchService.Get -> return NoContent()

IActionResult:

- `IActionResult` lets the client know if the request succeeded and provides the ID of the newly created pizza
- `IActionResult` uses standard HTTP status codes, so it can easily integrate with clients regardless of the language or platform they're running on
- `IActionResult` use case: Complex logic with highly dynamic responses.
  - It gives you total freedom to return any status code helper method (`Ok`, `NotFound`, `BadRequest`).
  - However, to document the API for tools like Swagger, you have to manually decorate the method with attributes

### Services methods:

- `List<SearchResult> SearchResults`
- Delete
  - Get - shouldn't this use `FirstOrDefault` like `Update`, or should `Update` use `Get`?
  - SearchResults.Remove

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

### Methods & notes:

The property `Category` is meant to be the folder path AFTER `Documents` and without the filename and exension. Right now it is set to "General", but I would like the folder path so that when/if I get to React, I can create sections based on that value.

- `StringComparison.OrdinalIgnoreCase`: case-insensitive string comparison used with the user search phrase to find marrkdown files with that phrase
- `UnauthorizedAccessException`: occurs when enumerating the files in a directory which I ran into when searching the Documents folder
- `HashSet<string>` used to define folders that may include markdown files like `node_modules` and to skip those folders.
- `IsSuccessStatusCode`: property on `HttpResponseMessage`, holds a boolean for if the request was successful (200 OK) or not. See [WebApiLab](https://github.com/Kernix13/WebApiLab)
  - I used that as opposed to `response.StatusCode`
- `PostAsJsonAsync`: serializes a given object into a JSON payload and transmits it as the body of an HTTP POST request
  - vs `PostAsync`: you are responsible for serializing the object, creating the HTTP content, & setting the content type
  - `PostAsJsonAsync` does all of that for you
- `PutAsJsonAsync`: serializes a C# object to JSON and sends an HTTP PUT request
  - `PutAsync`: Exactly the same idea as above
- `GetFromJsonAsync`: I think I will be using this for both GET requests
  - Get abd Get By Id both use `GetAsync` in other projects

<span aria-hidden="true"><br></span>

## Testing Project

Run the xUnit tests but they need to be run individually or they fail. I am working on fixing my code so that does not happen.

```bash
# Run the tests from root
dotnet test
```

<span aria-hidden="true"><br></span>

## React Project (not included yet)

Run the React UI app

```bash
cd FileTextSearch.Web
npm install
npm run dev
# open http://localhost:5195
```

<span aria-hidden="true"><br></span>

## Project structure

Remember to add a `.github` folder with templates for issues and pull requests.

<!-- ```python
file-text-search/
├── FileTextSearch.Api/
│   ├── Controllers/
│   ├── Models/
│   ├── Services/
│   └── Program.cs
├── FileTextSearch.Console/
│   ├── Models/
│   ├── Services/
│   └── Program.cs
├── FileTextSearch.Tests/
├── .gitignore
├── FileTextSearch.slnx
└── README.md
```

Or maybe this: -->

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

1. .NET SDK 10.0
2. C# Dex Kit VS Code extension
3. xUnit testing library
4. Node.js (not added yet)
5. React (not added yet)

<span aria-hidden="true"><br></span>

## Capstone Questions

1. Why this project?
   - The search results in Windows File Explorer returns results that Ido not want, so I wanted to create a custom search to organize all my notes on web development.
2. What did I learn from this project?
   - I have a better understanding of Object OrientedProgramming and insight into how to build and implement an API.
3. What did I learn from the Code:You C# Software Development pathway?
   - I found C# difficult but understanding the importance of data types was important to learn. I hope to convert my JavaScript projectsto TypeScript in the near future.
4. What would I have done differently for this project?
   - I wish that Icouldhave used a database and create aJSON file rather than use in memory for the data.

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

> Later...

1. I already allow the user to select a specific folder to search, but you have to type out the full folder path. It would be better to somehow allow the user to "browse" their system folders.
   - ✅ I have added this but you need the full path like `C:/Users/pc/Documents/WebDev/CodeYou`. That is cumbersome
2. Change user prompt and allow multiple search phrases separated by a comman, then `Split` on the comma and `Trim` whitespace
3. Search other file types: _.json_ and _.csv_ will be easy, _.docx_ and _.xls_ will require a Nuget package
4. I want to also be able to search for filename + extension like `reset.css`

<span aria-hidden="true"><br></span>

## Contributing

Contributions are welcome! If you'd like to help improve this project, please read our _contribution guidelines_ on how to get started, our workflow, and code style expectations.

> Add a link for "contribution guidelines" as soon as I add that file.

<span aria-hidden="true"><br></span>

## License

This project is licensed under the MIT License.

> Add a link for "MIT License" as soon as I add it.

<span aria-hidden="true"><br></span>

## 🚫 My temporary notes (remove later)

- Use `WebLabApi`, the Get and Get by id code in Console/Program.cs, and the Controller and Service code in .Api
- Use `ContosoPizza` project as an example of an API with controllers ✅
- Use `csharp-async-httpclient-example` project for using JsonSerializer.Deserialize and HttpClient in FileTextSearch.Console Program.cs file
- Use the last meet recording to connect the API to the console app
  - https://drive.google.com/file/d/1CLTLijgt45c5LN3LbBDp-ewox-6ultjM/view
  <!-- - Use `csharp-JsonSerializer-example` to create a json file, JsonSerializer.Serialize + File.WriteAllText, File.ReadAllText + JsonSerializer.Deserialize
  - Where would this go?
  - This uses a users.json file I created - how do I generate a json file? -->

### Code notes

Setting every markdown file to upper or lower case is extreme for a string comparison - use this to compare strings:

- `StringComparison.OrdinalIgnoreCase`: ignores the case of both strings during the comparison
  - Highly efficient, non-linguistic, case-insensitive comparison
  - It doesn't create new strings where `.ToLower` & `.ToUpper` does
  - `string.Equals`: this would not work because each markdown file is much larger than a search string so it would never return `true`
  - Docs: [StringComparison Enum](https://learn.microsoft.com/en-us/dotnet/api/system.stringcomparison?view=net-10.0)
- Look into `AddRange()` - C# lists have a built-in method called AddRange() which inserts the entire list into your collection
  - .Add(item) — Takes one single item and puts it at the end of the list
  - .AddRange(collection) — Takes a list/collection of items and adds all of them into the list
- The `ActionResult` type is the base class for all action results in ASP.NET Core.
  - It automatically returns data with a `Content-Type` value of `application/json`
  - what about IActionResult instead?
  <!-- - Look into `AppContext.BaseDirectory()`
  - built-in C# tool that finds the path to the folder where your program is compiled and running (usually deep inside a hidden folder called bin/Debug/net10.0) -->

HttpClient

- GetAsync vs PostAsJsonAsync
- IsSuccessStatusCode vs EnsureSuccessStatusCode() - Lawrence used just StatusCode in a conditional but IsSuccessStatusCode is used in WebApiLAb - Lawrence also used EnsureSuccessStatusCode
- response.Content.ReadFromJsonAsync vs response.Content.ReadAsStringAsync

Look at `WebApiLab` for more API code that connects to the console project, and `ClassLibraryProjects` for an example of unit testing

<!--
http://localhost:5042/api/search
 -->
