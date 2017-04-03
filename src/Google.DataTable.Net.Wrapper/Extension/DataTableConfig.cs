/*
   Copyright 2012 Zoran Maksimovic (zoran.maksimovich@gmail.com 
   http://www.agile-code.com)
 
   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
 
Code contribution (and a very special thanks to:)
- Tom Ziesmer for the DataTableConfig<T> implementation.
- List to DataTable transformation taken from:
   http://www.chinhdo.com/20090402/convert-list-to-datatable/
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Google.DataTable.Net.Wrapper.Extension
{
    /// <summary>
    /// Helper class for the generic lists.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DataTableConfig<T>
    {
        IEnumerable<T> _source;
        DataTable _dataTable;
        List<Func<T, Cell>> _colFunc;

        internal DataTableConfig(IEnumerable<T> source)
        {
            _source = source;
            _dataTable = new DataTable();
            _colFunc = new List<Func<T, Cell>>();
        }

        /// <summary>
        /// Adds a new Column to a Google.DataTable.Net.Wrapper.DataTable
        /// </summary>
        /// <param name="col"></param>
        /// <param name="cellFunction"></param>
        /// <returns></returns>
        public DataTableConfig<T> AddColumn(Column col, Func<T, Cell> cellFunction)
        {
            _dataTable.AddColumn(col);
            _colFunc.Add(cellFunction);
            return this;
        }

        /// <summary>
        /// Build a new Google.DataTable.Net.Wrapper.DataTable()
        /// </summary>
        /// <returns></returns>
        public DataTable Build()
        {
            try
            {
                if (_dataTable.Columns.Any())
                {
                    //user has configured columns
                    foreach (var r in _source)
                    {
                        var row = _dataTable.AddRow(_dataTable.NewRow());
                        row.AddCellRange(_colFunc.Select(c => c(r)).ToArray());
                    }
                }
                else
                {

                    /*
                     * No columns configured. For the time being this is a handy
                     * way of transforming the object into a DataTable, by
                     * reusing the already available code.
                     * */
                    //TODO: Test with large amount of data.
                    System.Data.DataTable dt = ToDataTable(_source.ToList());
                    _dataTable = SystemDataTableConverter.Convert(dt);
                }
                return _dataTable;
            }
            finally
            {
                _dataTable = null;
                _source = null;
                _colFunc = null;
            }
        }

        /// <summary>
        /// Convert a List{T} to a DataTable.
        /// </summary>
        private System.Data.DataTable ToDataTable<T>(List<T> items)
        {
            var tb = new System.Data.DataTable(typeof (T).Name);

            PropertyInfo[] props = typeof (T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo prop in props)
            {
                Type t = GetCoreType(prop.PropertyType);
                tb.Columns.Add(prop.Name, t);
            }


            foreach (T item in items)
            {
                var values = new object[props.Length];

                for (int i = 0; i < props.Length; i++)
                {
                    values[i] = props[i].GetValue(item, null);
                }

                tb.Rows.Add(values);
            }

            return tb;
        }

        /// <summary>
        /// Determine of specified type is nullable
        /// </summary>
        private static bool IsNullable(Type t)
        {
            return !t.IsValueType || (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>));
        }

        /// <summary>
        /// Return underlying type if type is Nullable otherwise return the type
        /// </summary>
        private static Type GetCoreType(Type t)
        {
            if (t != null && IsNullable(t))
            {
                if (!t.IsValueType)
                {
                    return t;
                }
                else
                {
                    return Nullable.GetUnderlyingType(t);
                }
            }
            else
            {
                return t;
            }
        }
    }
}