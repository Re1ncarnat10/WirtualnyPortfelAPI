using Microsoft.AspNetCore.Mvc;
using WirtualnyPortfelAPI.Models;

namespace WirtualnyPortfelAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private static readonly List<Category> _categories = new();

        [HttpGet]
        public IActionResult GetAll() => Ok(_categories);

        [HttpPost]
        public IActionResult Create([FromBody] Category c)
        {
            _categories.Add(c);
            return CreatedAtAction(nameof(GetById), new { id = c.Id }, c);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var c = _categories.FirstOrDefault(x => x.Id == id);
            if (c == null) return NotFound();
            return Ok(c);
        }
    }
}