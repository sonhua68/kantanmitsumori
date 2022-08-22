using Microsoft.VisualBasic;
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
                ListYear.Add(new DropDownList { Value = year, Text = year.ToString() });
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
      
        #region inputCar
        public static List<DropDownList> ListDropDownFirstYM(bool isEmpty)
        {
            var ddlY = new List<DropDownList>();
            if (isEmpty)
            {
                ddlY.Add(new DropDownList { Value = "", Text = "" });
            }
            int cnt = 0;
            int c = 1;
            while (cnt > -31)
            {
                var y = DateTime.Now.AddYears(cnt).Year;
                var value = GetWareki(y.ToString()) + " (" + DateTime.Now.AddYears(cnt).Year + ")";
                ddlY.Add(new DropDownList { Value = value, Text = value });
                cnt -= 1;
                c += 1;
            }
            return ddlY.ToList();
        }
        public static List<DropDownList> ListDropDownSyakenYM(bool isEmpty)
        {
            var ddlY = new List<DropDownList>();
            if (isEmpty)
            {
                ddlY.Add(new DropDownList { Value = "", Text = "" });
            }
            int cnt = 0;
            int c = 1;
            while (cnt < 5)
            {
                var y = DateTime.Now.AddYears(cnt).Year;
                var value = GetWareki(y.ToString()) + " (" + DateTime.Now.AddYears(cnt).Year + ")";              
                ddlY.Add(new DropDownList { Value = value, Text = value });
                cnt += 1;
                c += 1;
            }
            return ddlY.ToList();
        }
        /// <summary>
        /// getWareki
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public static string GetWareki(string year)
        {
            int intYear = Convert.ToInt32(year);
            string retNengo = "";
            if (1926 <= intYear & intYear <= 1988)
                retNengo = "S" + Convert.ToString(intYear - 1925);
            else if (intYear <= 2018)
                retNengo = "H" + Convert.ToString(intYear - 1988);
            else if (2019 <= intYear)
                retNengo = "R" + Convert.ToString(intYear - 2018);
            return retNengo;
        }
        #endregion inputCar
    }



}
