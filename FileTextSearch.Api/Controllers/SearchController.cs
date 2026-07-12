using Microsoft.AspNetCore.Mvc;
using FileTextSearch.Api.Models;
using FileTextSearch.Services;

namespace FileTextSearch.Controllers;

[ApiController]
[Route("api/[controller]")] // This makes the URL: api/search
public class SearchController : ControllerBase
{
    // GET: api/search
    [HttpGet]
    public ActionResult<List<SearchResult>> GetAll()
    {
        var results = SearchService.GetAll();
        return Ok(results);
    }

    // GET: api/search/{id}
    [HttpGet("{id}")]
    public ActionResult<SearchResult> GetById(Guid id)
    {
        var result = SearchService.Get(id);
        if (result == null)
        {
            return NotFound($"No file found with ID: {id}");
        }

        return Ok(result);
    }

    // POST: api/search
    [HttpPost]
    public ActionResult Create(List<SearchResult> newResults) // <-- Change this to List
    {
        // Pass the list to your service
        SearchService.Add(newResults);

        // Return a status 200 OK along with the data
        return Ok(newResults);
        // return Ok(new { message = $"Successfully added {newResults.Count} results." });
    }

    // PUT: api/search/{id}
    [HttpPut("{id}")]
    public IActionResult Update(Guid id, SearchResult updatedResult)
    {
        if (id != updatedResult.Id)
        {
            return BadRequest("ID in URL path does not match ID in request body.");
        }

        var existing = SearchService.Get(id);
        if (existing == null)
        {
            return NotFound($"No result found with ID: {id}");
        }

        SearchService.Update(updatedResult);
        return NoContent(); // Returns a 204 No Content 
    }

    // DELETE: api/search/a1b2c3d4...
    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        var existing = SearchService.Get(id);
        if (existing == null)
        {
            return NotFound($"No result found with ID: {id}");
        }

        SearchService.Delete(id);
        return NoContent(); // Returns a 204 No Content 
    }
}