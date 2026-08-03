using FileTextSearch.Api.Models;
using FileTextSearch.Api.Services;

namespace FileTextSearch.Tests;

public class SearchServiceTests
{
    [Fact]
    public void Add_WithResults_AddsResultsToList()
    {
        // Arrange
        var service = new SearchService();
        var newResult = new SearchResult { Id = Guid.NewGuid(), FileName = "test.txt" };

        // Act
        service.Add(new List<SearchResult> { newResult });

        // Assert
        Assert.Single(service.SearchResults);
        Assert.Equal("test.txt", service.SearchResults[0].FileName);
    }

    [Fact]
    public void GetAll_ReturnsAllSearchResults()
    {
        // Arrange
        var service = new SearchService();
        var results = new List<SearchResult>
        {
            new SearchResult { FileName = "test1.txt" },
            new SearchResult { FileName = "test2.txt" }
        };

        service.Add(results);

        // Act
        var returnedResults = service.GetAll();

        // Assert
        Assert.Equal(2, returnedResults.Count);
    }

    [Fact]
    public void Delete_WithExistingId_RemovesSearchResult()
    {
        // Arrange
        var service = new SearchService();
        var result = new SearchResult { Id = Guid.NewGuid(), FileName = "delete.txt" };
        service.Add(new List<SearchResult> { result });

        // Act
        service.Delete(result.Id);

        // Assert
        Assert.Empty(service.SearchResults);
    }
}