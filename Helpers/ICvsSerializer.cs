namespace todoapi.Helpers
{
    public interface ICvsSerializer
    {
        public void SaveToCsv(List<TodoApi.Models.TodoItem> items, string filePath);
    }
}
