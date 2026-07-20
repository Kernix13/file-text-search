using FileTextSearch.Api.Models;

namespace FileTextSearch.Api.Services;

public class SearchService
{

    public List<SearchResult> SearchResults { get; } = new List<SearchResult>();

    // Do I need a constructor here?

    // GET: api/search
    public List<SearchResult> GetAll() => SearchResults;

    // GET: api/search/{id}
    public SearchResult? Get(Guid id) => SearchResults.FirstOrDefault(result => result.Id == id);

    // POST: api/search
    public void Add(List<SearchResult> newResults)
    {
        // SearchResults.Clear();
        SearchResults.AddRange(newResults);
    }

    // PUT: api/search/{id}
    public void Update(SearchResult searchResult)
    {
        var result = SearchResults.FirstOrDefault(x => x.Id == searchResult.Id);
        if (result is null)
            return;

        result.Priority = searchResult.Priority;
    }

    // DELETE: api/search/{id}
    public void Delete(Guid id)
    {
        var searchResult = Get(id);
        if (searchResult is null) return;
        SearchResults.Remove(searchResult);
    }
}