using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MyExtensions.Methods
{
    public class GenericMethods<T> where T : new()
    {


        public void SaveListToCSV(List<T> items, string filePath) 
        {
            List<string> rows = new List<string>();

            T entry = new T();
            var cols = entry.GetType().GetProperties();
            string headerRow = string.Empty;


            foreach (var col in cols)
            {
                headerRow += $",{col.Name}";
            }
            rows.Add(headerRow.Substring(1));


            foreach (var item in items)
            {
             

                string dataRow = string.Empty;
                foreach (var col in cols)
                {
                    string itemString = $",{col.GetValue(item) ?? "".ToString()}";
                    dataRow += itemString;
                }

                
                rows.Add(dataRow.Substring(1));
        
            }

            File.WriteAllLines(filePath, rows);

        }

    }
}
