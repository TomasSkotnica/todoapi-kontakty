using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using TodoApi.Models;

namespace todoapi.Helpers
{

    public class CsvSerializer : ICvsSerializer
    {
        private readonly char csvSeparator = ',';

        public delegate void Callback(TodoItem todoItem);

        public void LoadFromCsv(Callback callback, string filePath)
        {
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
                            int intValue;
                            string[] fields = line.Split(csvSeparator);
                            TodoItem todoItem = new TodoItem();
                            if (int.TryParse(fields[0], out intValue))
                            {
                                todoItem.Id = intValue;
                                todoItem.Name = fields[1];
                                todoItem.Surname = fields[2];
                                todoItem.Email = fields[3];
                                todoItem.Phone = fields[4];
                                callback(todoItem);
                            }
                            else
                            {
                                Console.WriteLine($"Tomas message to Debug console: Error in line <{line}>");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Exception in line <{line}> {ex.ToString()}");
                        }
                    }
                }
            }
        }

        public void SaveToCsv(List<TodoApi.Models.TodoItem> items, string filePath)
        {
            // Get the properties of the type T
            PropertyInfo[] properties = typeof(TodoApi.Models.TodoItem).GetProperties();

            // Create a StringBuilder to store the CSV data
            StringBuilder csvData = new StringBuilder();

            // Write the header row
            csvData.AppendLine(string.Join(",", properties.Select(p => p.Name)));

            // Write the data rows
            foreach (var item in items)
            {
                List<string> propertyValues = new List<string>();

                foreach (var property in properties)
                {
                    // Get the value of each property and convert it to a string
                    object value = property.GetValue(item);
                    string valueString = (value != null) ? EscapeCsvField(value.ToString()) : "";

                    propertyValues.Add(valueString);
                }

                csvData.AppendLine(string.Join(",", propertyValues));
            }

            // Write the CSV data to the file
            File.WriteAllText(filePath, csvData.ToString());
        }

        private static string EscapeCsvField(string field)
        {
            // If the field contains double quotes, escape them by doubling
            if (field.Contains("\""))
            {
                field = field.Replace("\"", "\"\"");
            }

            // If the field contains commas, wrap it in double quotes
            if (field.Contains(","))
            {
                field = $"\"{field}\"";
            }

            return field;
        }
    }
}