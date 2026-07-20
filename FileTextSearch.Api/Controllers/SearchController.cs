using Microsoft.AspNetCore.Mvc;
using FileTextSearch.Api.Models;
using FileTextSearch.Api.Services;

namespace FileTextSearch.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SearchController : ControllerBase
{
    private readonly SearchService _searchService;

    public SearchController(SearchService searchService)
    {
        _searchService = searchService;
    }

    // GET: api/search
    [HttpGet]
    public ActionResult<List<SearchResult>> GetAll() => _searchService.GetAll();

    // GET: api/search/{id}
    [HttpGet("{id}")]
    public ActionResult<SearchResult> GetById(Guid id)
    {
        var result = _searchService.Get(id);
        if (result == null)
        {
            return NotFound($"No file found with ID: {id}");
        }

        return Ok(result);
        // return result;
    }

    // POST: api/search
    [HttpPost]
    public ActionResult Create(List<SearchResult> newResults)
    {
        // Pass the list to your service
        _searchService.Add(newResults);

        // Return a status 200 OK along with the data
        return Ok(newResults);
    }

    // PUT: api/search/{id}
    [HttpPut("{id}")]
    public IActionResult Update(Guid id, SearchResult updatedResult)
    {
        if (id != updatedResult.Id)
        {
            return BadRequest("ID in URL path does not match ID in request body.");
        }

        var itemToDelete = _searchService.Get(id);
        if (itemToDelete == null)
        {
            return NotFound($"No result found with ID: {id}");
        }

        _searchService.Update(updatedResult);
        return NoContent(); // Returns a 204 No Content 
    }

    // DELETE: api/search/a1b2c3d4...
    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        var itemToDelete = _searchService.Get(id);
        if (itemToDelete == null)
        {
            return NotFound($"No result found with ID: {id}");
        }

        _searchService.Delete(id);
        return NoContent(); // Returns a 204 No Content 
    }
}