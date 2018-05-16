using System;
using System.Collections.Generic;

namespace Google.DataTable.Net.Wrapper.Extension
{
    /// <summary>
    /// Extension class for lists.
    /// </summary>
    public static class EnumerableExtension
    {
        /// <summary>
        /// Entry point extension method for IEnumerable types.
        /// Returns a new DataTableConfig.
        /// <param name="source"></param>
        /// <returns></returns>
        /// </summary>    
        public static DataTableConfig<T> ToGoogleDataTable<T>(this IEnumerable<T> source) where T : class
        {
            return new DataTableConfig<T>(source);
        }

        /// <summary>
        /// Creates a new column
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="col"></param>
        /// <param name="rowValue"></param>
        /// <param name="rowFormat"></param>
        /// <returns></returns>
        public static DataTableConfig<T> NewColumn<T>(this DataTableConfig<T> source, Column col, Func<T, object> rowValue, Func<T, string> rowFormat = null)
        {
            if (rowFormat != null)
            {
                source.AddColumn(col, r => new Cell(rowValue(r), rowFormat(r)));
            }
            else
            {
                source.AddColumn(col, r => new Cell(rowValue(r)));
            }
            return source;
        }
    }
}