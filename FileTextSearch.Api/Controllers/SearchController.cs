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
        return Ok(results); // Returns your mock list as JSON with a 200 OK status
    }

    // GET: api/search/3f2504e0-4f89-11d3-9a0c-0305e82c3301
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
    public ActionResult<SearchResult> Create(SearchResult newResult)
    {
        // The model automatically generates a new Guid on creation, 
        // but you could also explicitly enforce it: newResult.Id = Guid.NewGuid();

        SearchService.Add(newResult);

        // Standard REST practice: returns a 201 Created status, 
        // pointing to the URL where the new item can be fetched
        return CreatedAtAction(nameof(GetById), new { id = newResult.Id }, newResult);
    }

    // PUT: api/search/a1b2c3d4...
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