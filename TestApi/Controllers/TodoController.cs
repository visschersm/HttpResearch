using System.Diagnostics.SymbolStore;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Mvc;
using MTech.HttpResearch.DataModel;
using MTech.HttpResearch.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.JsonPatch;
using AutoMapper.QueryableExtensions;


namespace MTech.HttpResearch.TestApi
{
    [Route("[controller]")]
    public class TodoController : ControllerBase
    {
        private readonly ITodoContext _context; 
        public TodoController(ITodoContext context)
        {
            _context = context;
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var toDelete = _context.TodoItems
                .SingleOrDefault(x => x.Id == id);
            
            if(toDelete == null)
                return NotFound($"TodoItem with id: {id} could not be found");

            _context.TodoItems.Remove(toDelete);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _context.TodoItems.AsNoTracking()
                .ToArrayAsync();
            
            return Ok(result);
        }

        [HttpHead]
        public async Task<IActionResult> Head()
        {
            var total = await _context.TodoItems.AsNoTracking()
                .CountAsync();

            Response.ContentLength = total;
            return Ok();
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _context.TodoItems.AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == id);

            if(result == null)
                return NotFound($"TodoItem with id: {id} could not be found.");
            
            return Ok(result);
        }

        // The HttpHead is used to see the data size before 
        // returning. This is usefull to provide information 
        // about data downloads before the bandwidth is used.
        // Check the get methods for more information.
        // [HttpHead]
        // public async Task<IActionResult> Head()
        // {
        //     throw new NotImplementedException();
        // }

        [HttpOptions]
        public IActionResult Options()
        {
            HttpContext.Response.Headers.Add("Allow", new string[] { "GET", "OPTIONS" });
            return Ok();
        }

        [HttpPatch]
        [Route("{id}")]
        public async Task<IActionResult> Patch(int id, [FromBody] JsonPatchDocument<TodoItem> patchDocument)
        {
            if(patchDocument == null)
                return BadRequest(ModelState);
            
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var entity = await _context.TodoItems.SingleOrDefaultAsync(x => x.Id == id);

            patchDocument.ApplyTo(entity);

            await _context.SaveChangesAsync();
            
            return Ok(entity);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]TodoItem toCreate)
        {
            var data = await _context.TodoItems.SingleOrDefaultAsync(x => x.Id == toCreate.Id);

            if(data != null)
                return BadRequest($"System already contains a TodoItem with id: {toCreate.Id}");
            
            var newTodo = _context.TodoItems.Add(toCreate).Entity;
            await _context.SaveChangesAsync();

            return Ok(newTodo);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]TodoItem toUpdate)
        {
            var todoItem = await _context.TodoItems.SingleOrDefaultAsync(x => x.Id == id);

            if(todoItem == null)
                return NotFound($"TodoItem with id: {id} could not be found");

            todoItem = toUpdate;

            await _context.SaveChangesAsync();

            return Ok(todoItem);
        }
    }
}