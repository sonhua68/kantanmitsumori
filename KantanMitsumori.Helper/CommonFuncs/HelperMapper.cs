using AutoMapper;
using Microsoft.Extensions.Configuration;
using System.ComponentModel;
using System.Data;
using System.Reflection;

namespace KantanMitsumori.Helper.CommonFuncs
{
    public class HelperMapper
    {
        public static IMapper _mapper;


        public HelperMapper(IMapper mapper)
        {
            _mapper = mapper;
        }
        public TResult MergeInto<TResult>(object item1, object item2)
        {
            return _mapper!.Map(item2, _mapper.Map<TResult>(item1));
        }

        public TResult MergeInto<TResult>(params object[] objects)
        {
            var res = _mapper!.Map<TResult>(objects.First());
            return objects.Skip(1).Aggregate(res, (r, obj) => _mapper.Map(obj, r));
        }
        #region Data Process
        public DataTable ToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection props =
                TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(
            prop.PropertyType) ?? prop.PropertyType);
            }
            object[] values = new object[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }
                table.Rows.Add(values);
            }
            return table;
        }
        public DataTable JoinDataTable(DataTable dataTable1, DataTable dataTable2, string joinField)
        {
            var dt = new DataTable();
            var joinTable = from t1 in dataTable1.AsEnumerable()
                            join t2 in dataTable2.AsEnumerable()
                                on t1[joinField] equals t2[joinField]
                            select new { t1, t2 };

            foreach (DataColumn col in dataTable1.Columns)
                dt.Columns.Add(col.ColumnName, typeof(string));

            dt.Columns.Remove(joinField);

            foreach (DataColumn col in dataTable2.Columns)
                dt.Columns.Add(col.ColumnName, typeof(string));

            foreach (var row in joinTable)
            {
                var newRow = dt.NewRow();
                newRow.ItemArray = row.t1.ItemArray.Union(row.t2.ItemArray).ToArray();
                dt.Rows.Add(newRow);
            }
            return dt;
        }
        public DataTable JoinDataTables(DataTable t1, DataTable t2, params Func<DataRow, DataRow, bool>[] joinOn)
        {
            DataTable result = new DataTable();
            foreach (DataColumn col in t1.Columns)
            {
                if (result.Columns[col.ColumnName] == null)
                    result.Columns.Add(col.ColumnName, col.DataType);
            }

            foreach (DataColumn col in t2.Columns)
            {
                if (result.Columns[col.ColumnName] == null)
                    result.Columns.Add(col.ColumnName, col.DataType);
            }
            foreach (DataRow row1 in t1.Rows)
            {
                var joinRows = t2.AsEnumerable().Where(row2 =>
                {
                    foreach (var parameter in joinOn)
                    {
                        if (!parameter(row1, row2)) return false;
                    }
                    return true;
                });
                foreach (DataRow fromRow in joinRows)
                {
                    DataRow insertRow = result.NewRow();
                    foreach (DataColumn col1 in t1.Columns)
                    {
                        insertRow[col1.ColumnName] = row1[col1.ColumnName];
                    }
                    foreach (DataColumn col2 in t2.Columns)
                    {
                        insertRow[col2.ColumnName] = fromRow[col2.ColumnName];
                    }
                    result.Rows.Add(insertRow);
                }
            }
            return result;
        }

        public List<T> ConvertToList<T>(DataTable dt)
        {
            var columnNames = dt.Columns.Cast<DataColumn>()
                    .Select(c => c.ColumnName)
                    .ToList();
            var properties = typeof(T).GetProperties();
            return dt.AsEnumerable().Select(row =>
            {
                var objT = Activator.CreateInstance<T>();
                foreach (var pro in properties)
                {
                    if (columnNames.Contains(pro.Name))
                    {
                        PropertyInfo pI = objT.GetType().GetProperty(pro.Name);

                        pro.SetValue(objT, row[pro.Name] == DBNull.Value ? null :
                            Convert.ChangeType(row[pro.Name], Nullable.GetUnderlyingType(
            pI.PropertyType) ?? pI.PropertyType));
                    }
                }
                return objT;
            }).ToList();
        }

        #endregion
    }
}
