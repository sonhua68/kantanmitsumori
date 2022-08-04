using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace KantanMitsumori.Helper.CommonFuncs
{

    public class DropDownList
    {
        public object? Value { set; get; }
        public string? Text { set; get; }
        public static List<DropDownList> ListMonth(bool isEmpty)
        {
            var ListMonth = new List<DropDownList>();
            if (isEmpty)
            {
                ListMonth.Add(new DropDownList { Value = "", Text = "" });

            }
            for (int i = 1; i <= 12; i++)
            {
                ListMonth.Add(new DropDownList { Value = i, Text = i.ToString() });
            }
            return ListMonth.ToList();
        }

        public static List<DropDownList> ListYear(bool isEmpty, int numberYear)
        {
            var ListYear = new List<DropDownList>();
            if (isEmpty)
            {
                ListYear.Add(new DropDownList { Value = "", Text = "" });

            }
            DateTime now = DateTime.Now;
            var year = now.Year;
            for (int i = 1; i <= numberYear; i++)
            {            
                ListYear.Add(new DropDownList { Value = year, Text = year.ToString()});
                year++;
            }
            return ListYear.ToList();
        }
        public static List<DropDownList> ListObjectData(bool isEmpty, object data)
        {
            var ListData = new List<DropDownList>();
            if (isEmpty)
            {
                ListData.Add(new DropDownList { Value = "", Text = "" });

            }
            foreach (PropertyInfo propertyInfo in data.GetType().GetProperties())
            {
                var Key = propertyInfo.Name;
                var Value = propertyInfo.GetValue(data, null);

                ListData.Add(new DropDownList { Value = Key, Text = Value?.ToString() });
            }
            return ListData.ToList();
        }
    }



}
