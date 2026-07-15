using Microsoft.AspNetCore.Mvc;
using FileTextSearch.Api.Models;
using FileTextSearch.Api.Services;

namespace FileTextSearch.Api.Controllers;

[ApiController]
[Route("api/[controller]")] // This makes the URL: api/search
public class SearchController : ControllerBase
{
    // This is from WebApiLab, but I don't think it's needed here. I think the service is enough.
    private List<SearchResult> results = new List<SearchResult>();

    public SearchController()
    {
        // No code here
    }

    // GET: api/search
    [HttpGet]
    public ActionResult<List<SearchResult>> GetAll() => SearchService.GetAll();

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
        // return result;
    }

    // POST: api/search
    [HttpPost]
    public ActionResult Create(List<SearchResult> newResults)
    {
        // Pass the list to your service
        SearchService.Add(newResults);

        // Return a status 200 OK along with the data
        return Ok(newResults);
    }

    /*
    public IActionResult Create(List<SearchResult> newResults)
    {
        SearchService.Add(newResults);
        
        // Ths is from a project that was only creating a single object, but we are creating a list. So we need to adjust the return statement.
        return CreatedAtAction(nameof(GetById), new { id = newResults[0].Id }, newResults[0]);
    }
    */

    // PUT: api/search/{id}
    [HttpPut("{id}")]
    public IActionResult Update(Guid id, SearchResult updatedResult)
    {
        if (id != updatedResult.Id)
        {
            return BadRequest("ID in URL path does not match ID in request body.");
        }

        var itemToDelete = SearchService.Get(id);
        if (itemToDelete == null)
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
        var itemToDelete = SearchService.Get(id);
        if (itemToDelete == null)
        {
            return NotFound($"No result found with ID: {id}");
        }

        SearchService.Delete(id);
        return NoContent(); // Returns a 204 No Content 
    }
}