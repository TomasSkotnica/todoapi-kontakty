using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using todoapi.Controllers;
using TodoApi.Models;

namespace todoapi.Helpers
{

    public class CsvSerializer : ICvsSerializer
    {
        private readonly char csvSeparator = ',';

        public delegate void Callback(TodoItem todoItem);

        public LoadResult LoadFromCsv(Callback callback, string filePath, ILogger<TodoItemsController> logger)
        {
            LoadResult result = new LoadResult();
            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                bool firstLineSkipped = false;

                // Read lines until the end of the file
                while ((line = reader.ReadLine()) != null)
                {
                    if (!firstLineSkipped)
                    {
                        // skip header
                        firstLineSkipped = true;
                    }
                    else
                    {
                        try
                        {
                            string[] fields = line.Split(csvSeparator);
                            TodoItem todoItem = new TodoItem();
                            todoItem.Id = Int32.Parse(fields[0]);
                            todoItem.Name = fields[1];
                            todoItem.Surname = fields[2];
                            todoItem.Email = fields[3];
                            todoItem.Phone = fields[4];
                            callback(todoItem);
                        }
                        catch (Exception ex)
                        {
                            logger.LogWarning($"Exception at line <{line}> {ex.ToString()}");
                            result.Errors.Add($"Exception at line <{line}>");
                        }
                    }
                }
            }
            return result;
        }

        public void SaveToCsv(List<TodoApi.Models.TodoItem> items, string filePath)
        {
            // Get the properties of the type T
            PropertyInfo[] properties = typeof(TodoApi.Models.TodoItem).GetProperties();

            StringBuilder csvData = new StringBuilder();

            csvData.AppendLine(string.Join(",", properties.Select(p => p.Name)));

            foreach (var item in items)
            {
                List<string> propertyValues = new List<string>();

                foreach (var property in properties)
                {
                    object? value = property.GetValue(item);
                    string valueString = (value != null) ? value.ToString() : "";

                    propertyValues.Add(valueString);
                }

                csvData.AppendLine(string.Join(csvSeparator, propertyValues));
            }

            File.WriteAllText(filePath, csvData.ToString());
        }
    }
}