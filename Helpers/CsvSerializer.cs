using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using TodoApi.Models;

namespace todoapi.Helpers
{

    public class CsvSerializer
    {
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