using System.Reflection;

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

        #region InputLoan
        public static List<DropDownList> DropDownFirstMonth(bool isEmpty)
        {
            var ListMonth = new List<DropDownList>();
            var dtNow = DateTime.Now;
            var CurrentYear = dtNow.Year;
            var CurrentMonth = dtNow.Month;
            var setMonth = CurrentMonth;
            if (isEmpty)
            {
                ListMonth.Add(new DropDownList { Value = "", Text = "" });
            }
            for (int i = 0; i <= 3; i++)
            {
                if (CurrentMonth == 13)
                {
                    setMonth = 1;
                }
                else
                {
                    ListMonth.Add(new DropDownList { Value = setMonth, Text = setMonth.ToString() });
                    setMonth = setMonth + 1;
                }
            }
            return ListMonth.ToList();
        }
        public static List<DropDownList> DropDownBonusMonth(bool isEmpty)
        {
            var ListMonth = new List<DropDownList>();
            if (isEmpty)
            {
                ListMonth.Add(new DropDownList { Value = "0", Text = "" });
            }
            for (int i = 6; i <= 12; i++)
            {
                if (i != 10 & i != 11)
                {
                    ListMonth.Add(new DropDownList { Value = i, Text = i.ToString() });
                }
            }
            for (int i = 1; i <= 2; i++)
            {
                ListMonth.Add(new DropDownList { Value = i, Text = i.ToString() });
            }
            return ListMonth.ToList();
        }
        public static List<DropDownList> DropDownBonusSecond(bool isEmpty, string month)
        {
            var ListMonth = new List<DropDownList>();
            if (!string.IsNullOrEmpty(month))
            {
                int m = int.Parse(month);

                var isCheck = (m == 1 || m == 2 || m == 12);
                if ((isEmpty) & isCheck)
                {
                    ListMonth.Add(new DropDownList { Value = "0", Text = "" });
                }
                if (m == 6 || m == 7 || m == 8 || m == 9)
                {
                    ListMonth.Add(new DropDownList { Value = 12, Text = "12" });
                    ListMonth.Add(new DropDownList { Value = 1, Text = "1" });
                    ListMonth.Add(new DropDownList { Value = 2, Text = "2" });
                }
                else if (isCheck)
                {
                    for (int i = 6; i <= 9; i++)
                    {
                        ListMonth.Add(new DropDownList { Value = i, Text = i.ToString() });
                    }
                }
            }


            return ListMonth.ToList();
        }
        #endregion InputLoan

        #region InpZeiHoken 
        public static List<DropDownList> DropDownJibaiHokenMonth(bool isEmpty)
        {
            var ListMonth = new List<DropDownList>();
            if (isEmpty)
            {
                ListMonth.Add(new DropDownList { Value = "", Text = "" });

            }
            for (int i = 1; i <= 37; i++)
            {
                ListMonth.Add(new DropDownList { Value = i, Text = i.ToString() });
            }
            return ListMonth.ToList();
        }
        #endregion InpZeiHoken
    }



}
