# Search files for text using C#

This project will enable the user to search word or phrase and return the full path for any text file (.md, .txt, .html) that contains the search phrase.

The point of the project is to consolidate my various notes, which are mainly in markdown files, into a master file or folder. For example, I have multiple files and notes involving CSS resets. These notes and files are spread across many folders and I can't always remember where they are.

This repo only has the API project at this time. Here is the project structure I will have:

- FileTextSearch.Api/ (Done ✅)
- FileTextSearch.Console/ (Next 📌) - search logic done, but need to connect it to the API and to create a json file from the results.
- FileTextSearch.Tests/ (using xUnit)
- FileTextSearch.Web/ (using React)

<!--
  namespace: FileTextSearch
  About text: ?
  Topics: csharp, ???
 -->

<span aria-hidden="true"><br></span>

## Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/en-us/download) 8.0
- [Visual Studio Code](https://code.visualstudio.com/) with [C# Dev Kit](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csdevkit)

<span aria-hidden="true"><br></span>

## Installation & Usage

1. Clone this repository and switch into project folder

   ```sh
   git clone https://github.com/Kernix13/file-text-search.git
   cd file-text-search/FileTextSearch.Console
   ```

2. Run the application - not sure of the steps

   ```bash

   ```

...

I am not sure of all the steps I will need to add here, but it will most likely include:

- Run the Web API (FileTextSearch.Api)
- Run the Console app (FileTextSearch.Console)
- Run the React web app
- Run the xUnit tests

I will need cd commands depending on the project!

### <span aria-hidden="true">⚡</span> Quick Start

```sh
git clone https://github.com/Kernix13/file-text-search.git
cd file-text-search/FileTextSearch.Console
dotnet run
```

<span aria-hidden="true"><br></span>

### API Project

Run the API

```bash
# From project root
dotnet run --project FileTextSearch.Api # or
dotnet run -p FileTextSearch.Api
```

Methods & notes:

-

<span aria-hidden="true"><br></span>

### Console Project

Run the Console app

```bash
# From project root
dotnet run --project FileTextSearch.Console # or
dotnet run -p FileTextSearch.Console
```

Methods & notes:

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

### Testing Project

Run the xUnit tests

```bash
#
```

<span aria-hidden="true"><br></span>

### React Project (OPTIONAL)

Run the React UI app

```bash
cd FileTextSearch.Web
npm run dev
# open http://localhost:5195
```

<span aria-hidden="true"><br></span>

## Project structure

Remember to add a .github folder with templates for issues and pull requests.

```python
# Add later
```

<span aria-hidden="true"><br></span>

## Tech Stack

> Later...

<span aria-hidden="true"><br></span>

## Capstone Questions

> Later...

<span aria-hidden="true"><br></span>

## AI Usage

> Later...

<span aria-hidden="true"><br></span>

## Acknowledgments & Resources

> Later...

1.
2.
3.
4.
5.

<span aria-hidden="true"><br></span>

## Future Improvements

> Later...

1. Allow user to select a specificfolder to search. That means I need nother prompt. If thy choose to search a folder, then I need to print out all folders in Documents and make them selectable, then repeat. I could also allow the user to type in the specific folder name.
2. Change user prompt and allow multiple search phrases separated by a comman, then Split on the comma and trim whitespace
3. Search other file types. .json and .csv will be easy, .docx and .xls will require a Nuget package
4.
5.

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

> WebLabApi: use the Get and Get by id code in Console/Program.cs, and the Controller and Service code in .Api

- Use `ContosoPizza` project as an example of an API with controllers ✅
- Use `csharp-async-httpclient-example` project for using JsonSerializer.Deserialize and HttpClient in FileTextSearch.Console Program.cs file
- Use the last meet recording to connect the API to the console app
  - https://drive.google.com/file/d/1CLTLijgt45c5LN3LbBDp-ewox-6ultjM/view
- Use `csharp-JsonSerializer-example` to create a json file, JsonSerializer.Serialize + File.WriteAllText, File.ReadAllText + JsonSerializer.Deserialize
  - Where would this go?
  - This uses a users.json file I created - how do I generate a json file?

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
- Look into `AppContext.BaseDirectory()`
  - built-in C# tool that finds the path to the folder where your program is compiled and running (usually deep inside a hidden folder called bin/Debug/net10.0)
- The `ActionResult` type is the base class for all action results in ASP.NET Core.
  - It automatically returns data with a `Content-Type` value of `application/json`
  - what about IActionResult instead?

HttpClient

- GetAsync vs PostAsJsonAsync
- IsSuccessStatusCode vs EnsureSuccessStatusCode() - Lawrence used just StatusCode in a conditional but IsSuccessStatusCode is used in WebApiLAb - Lawrence also used EnsureSuccessStatusCode
- response.Content.ReadFromJsonAsync vs response.Content.ReadAsStringAsync

Look at `WebApiLab` for more API code that connects to the console project, and `ClassLibraryProjects` for an example of unit testing

http://localhost:5042/api/search

<!--
1.
 -->
