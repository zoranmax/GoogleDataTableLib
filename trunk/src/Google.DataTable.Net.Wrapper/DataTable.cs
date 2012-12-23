/*
   Copyright 2012 Zoran Maksimovic (zoran.maksimovich@gmail.com)

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/

using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Google.DataTable.Net.Wrapper.Common;

namespace Google.DataTable.Net.Wrapper
{
    /// <summary>
    ///     A DataTable represents a basic two-dimensional table. 
    ///     All data (cells) in each column must have the same data type. 
    ///     Each cell in the table holds a value. 
    /// <remarks>
    ///     For more information about the usage of the serialized DataTable please visit:
    ///     https://developers.google.com/chart/interactive/docs/reference#DataTable
    /// </remarks>
    /// </summary>
    
    public class DataTable: ISerializable
    {         
        private readonly List<Row> _rows;         
        private readonly List<Column> _columns;

        /// <summary>
        /// Default constructor
        /// </summary>
        public DataTable()
        {
            _columns = new List<Column>();
            _rows = new List<Row>();
        }

        /// <summary>
        /// Returns a list of already assigned columns to the current DataTable
        /// </summary>
        public IList<Column> Columns
        {
            get { return _columns; }
        }

        /// <summary>
        /// Returns a list of already assigned rows to the current DataTable       
        /// </summary>
        public IList<Row> Rows
        {
            get { return _rows; }            
        }

        /// <summary>
        /// Creates a new DataRow with the same schema as the table.
        /// </summary>
        /// <returns>A new row with the same schema as the table</returns>
        public Row NewRow()
        {
            return new Row()
                {
                    ColumnTypes = Columns.Select(x => x.ColumnType).ToList()
                };
        }

        /// <summary>
        /// Adds a row to the list of rows
        /// attached to the current DataTable
        /// </summary>
        /// <param name="row">a row created with NewRow() method</param>
        /// <returns></returns>
        public Row AddRow(Row row)
        {
            //TODO: Add a check so that is not possible to add rows with a different column types.

            Rows.Add(row);
            return row;
        }

        /// <summary>
        /// Adds a new column to the current DataTable
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        public Column AddColumn(Column column)
        {
            
            var isExistingId = _columns.Any(x => x.Id == column.Id);
            if (isExistingId)
            {
                throw new Exception(string.Format("Column with the ID: '{0}' already exists", column.Id));
            }
            _columns.Add(column);
            return column;
        }

        /// <summary>
        /// Returns a Json string compatible with the Google DataTable notation.
        /// </summary>
        /// <returns></returns>
        public string GetJson()
        {
            string columnsJson = JsonizeColumns();
            string rowsJson = JsonizeRows();

            var finalJson = columnsJson + ", " + rowsJson;

            return "{" + finalJson.TrimEnd(',') + "}";
        }

        /// <summary>
        /// Serializes the Rows into the Json format
        /// </summary>
        /// <returns></returns>
        private string JsonizeRows()
        {
            var sb = new StringBuilder();

            foreach (Row r in Rows)
            {
                sb.Append(r.GetJson());
                sb.Append(", ");
            }
            
            var intermediateJson = sb.ToString().Trim().TrimEnd(',');
            var json = Helper.DoubleQuoteString("rows") + ": [" + intermediateJson + "]";
            
            return json;
        }

        /// <summary>
        /// Serializes the Columns into the Json format
        /// </summary>
        /// <returns></returns>
        private string JsonizeColumns()
        {
            var sb = new StringBuilder();

            foreach (Column c in Columns)
            {
                sb.Append(c.GetJson());
                sb.Append(", ");
            }

            var intermediateJson = sb.ToString().Trim().TrimEnd(',');
            var json = Helper.DoubleQuoteString("cols") + ": [" + intermediateJson + "]";

            return json;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("cols", this._columns);
            info.AddValue("rows", this._rows);
        }

        protected DataTable(SerializationInfo info, StreamingContext context)
        {
            // Reset the property value using the GetValue method.
            _columns = (List<Column>)info.GetValue("cols", typeof(List<Column>));
            _rows = (List<Row>)info.GetValue("rows", typeof(List<Row>));
        }
    }
}
