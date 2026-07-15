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
}