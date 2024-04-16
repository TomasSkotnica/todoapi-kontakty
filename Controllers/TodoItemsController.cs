using Microsoft.AspNetCore.Diagnostics;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Docs.Samples;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;
using System.Text.RegularExpressions;
using todoapi.Helpers;
using Microsoft.AspNetCore.HttpLogging;
using System.IO;

namespace todoapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly TodoContext _context;
        private readonly CsvSerializer _csvSerializer;
        private readonly string _myCsvFile = @".\MojeKontakty.csv";

        public TodoItemsController(TodoContext context, CsvSerializer csvSerializer)
        {
            _context = context;
            _csvSerializer = csvSerializer;
        }

        // GET: api/TodoItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems()
        {
            return await _context.TodoItems.ToListAsync();
        }

        private void AddItemToContext(TodoItem todoItem)
        {
            _context.TodoItems.Add(todoItem);
        }

        [HttpGet("load")]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetOnLoad()
        {

            _csvSerializer.LoadFromCsv(AddItemToContext, _myCsvFile);
            await _context.SaveChangesAsync();

            return await _context.TodoItems.ToListAsync();
        }

        // GET: api/TodoItems
        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetFilter()
        {
            string name = HttpContext.Request.Query["name"];
            if (string.IsNullOrEmpty(name)) { name = string.Empty; }
            return await _context.TodoItems.Where(t => t.Name != null && t.Name.Contains(name)).ToListAsync();
        }

        // GET: api/TodoItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> GetTodoItem(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }

        // PUT: api/TodoItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(long id, TodoItem todoItem)
        {
            if (id != todoItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(todoItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();

                _csvSerializer.SaveToCsv(_context.TodoItems.ToList<TodoApi.Models.TodoItem>(), _myCsvFile);

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/TodoItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem todoItem)
        {
            if (!this.TodoItemIsValid(todoItem))
                return BadRequest("Item to insert is not valid. Item was not added.");

            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();

            _csvSerializer.SaveToCsv(_context.TodoItems.ToList<TodoApi.Models.TodoItem>(), _myCsvFile);

            return CreatedAtAction("PostTodoItem", new { id = todoItem.Id }, todoItem);
        }

        private bool TodoItemIsValid(TodoItem item)
        {
            bool result = true;
            if (!string.IsNullOrEmpty(item.Email))
            {
                Match m = Regex.Match(item.Email, @"^[!#$%&'*+\-\/=?^_`{|}~a-zA-Z0-9\.]+@[a-zA-Z0-9-\.]+[a-zA-Z]{2,}$", RegexOptions.IgnoreCase);
                if (!m.Success)
                    result = false;
            }

            return result;
        }

        // DELETE: api/TodoItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TodoItemExists(long id)
        {
            return _context.TodoItems.Any(e => e.Id == id);
        }

        [HttpGet("order")]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetOrder()
        {
            string name = HttpContext.Request.Query["orderby"];
            string dir = HttpContext.Request.Query["direction"];

            if (string.IsNullOrEmpty(name)) { name = string.Empty; }
            if (string.IsNullOrEmpty(dir)) { dir = string.Empty; }
            if (dir.ToUpper().Equals("DESC")) {
                return await _context.TodoItems.OrderByDescending(t => t.Surname).ToListAsync();
            }
            var res = await _context.TodoItems.OrderBy(t => t.Surname).ToListAsync();
            return res;
        }

    }
}
