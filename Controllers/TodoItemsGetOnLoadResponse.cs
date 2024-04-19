using TodoApi.Models;
namespace todoapi.Controllers
{
    public class TodoItemsGetOnLoadResponse
    {
        public IEnumerable<TodoItem> Items { get; set; }
        public List<string> Message { get; set; }
}
}
