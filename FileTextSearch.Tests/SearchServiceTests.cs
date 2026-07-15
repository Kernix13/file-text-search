using FileTextSearch.Api.Models;
using FileTextSearch.Api.Services;

namespace FileTextSearch.Tests;

public class SearchServiceTests
{
    [Fact]
    public void Add_WithResults_AddsResultsToList()
    {
        // NOTE: Having SearchResults in SearchService as a static property makes it difficult to test, as the state persists across tests. Consider refactoring to use dependency injection or a non-static approach for better testability.

        // Arrange
        SearchService.SearchResults.Clear();
        var newResult = new SearchResult { Id = Guid.NewGuid(), FileName = "test.txt" };

        // Act
        SearchService.Add(new List<SearchResult> { newResult });

        // Assert
        Assert.Single(SearchService.SearchResults);
        Assert.Equal("test.txt", SearchService.SearchResults[0].FileName);
    }

    // They failed, I had to run them individually to get them to pass
    [Fact]
    public void GetAll_ReturnsAllSearchResults()
    {

        // Arrange
        SearchService.SearchResults.Clear();
        var results = new List<SearchResult>
        {
            new SearchResult { FileName = "test1.txt" },
            new SearchResult { FileName = "test2.txt" }
        };

        SearchService.Add(results);

        // Act
        var returnedResults = SearchService.GetAll();

        // Assert
        Assert.Equal(2, returnedResults.Count);
    }

    [Fact]
    public void Delete_WithExistingId_RemovesSearchResult()
    {
        // Arrange
        SearchService.SearchResults.Clear();
        var result = new SearchResult { Id = Guid.NewGuid(), FileName = "delete.txt" };
        SearchService.Add(new List<SearchResult> { result });

        // Act
        SearchService.Delete(result.Id);

        // Assert
        Assert.Empty(SearchService.SearchResults);
    }
}