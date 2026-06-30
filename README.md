# Search files for text using C#

This project will enable the user to search word or phrase and return the full path for any text file (.md, .txt, .html) that contains the search phrase.

The point of the project is to consolidate my various notes, which are mainly in markdown files, into a master file or folder. For example, I have multiple files and notes involving CSS resets. These notes and files are spread across many folders and I can't always remember where they are.

This repo only has the API project at this time. Here is the project structure I will have:

- FileTextSearch.Api/ (Done ✅)
- FileTextSearch.Console/ (Next 📌)
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
- Run the console app (FileTextSearch.Console)
- Run the React Web App
- Run the xUnit tests

I will need cd commands depending on the project!

### <span aria-hidden="true">⚡</span> Quick Start

```sh
git clone https://github.com/Kernix13/file-text-search.git
cd file-text-search/FileTextSearch.Console
dotnet run
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

- Use `ContosoPizza` project as an example of an API with controllers ✅
- Use `csharp-async-httpclient-example` project for using JsonSerializer.Deserialize and HttpClient in FileTextSearch.Console Program.cs file
- Use `csharp-JsonSerializer-example` to create a json file, JsonSerializer.Serialize + File.WriteAllText, File.ReadAllText + JsonSerializer.Deserialize
  - Where would this go?

### Code notes

Setting every markdown file to upper or lower case is extreme for a string comparison - use this to compare strings:

- `StringComparison.OrdinalIgnoreCase`: ignores the case of both strings during the comparison
  - Highly efficient, non-linguistic, case-insensitive comparison
  - It doesn't create new strings where `.ToLower` & `.ToUpper` does
  - `string.Equals`: this would not work because each markdown file is much larger than a search string so it would never return `true`
  - Docs: [StringComparison Enum](https://learn.microsoft.com/en-us/dotnet/api/system.stringcomparison?view=net-10.0)
