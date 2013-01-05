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
*/

using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.IO;

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
    [Serializable]
    public class DataTable : ISerializable
    {
        private readonly List<Row> _rows;
        private readonly List<Column> _columns;
        //internal cache of available column types.
        private readonly List<ColumnType> _columnTypes;

        /// <summary>
        /// Default constructor
        /// </summary>
        public DataTable()
        {
            _columns = new List<Column>();
            _rows = new List<Row>();
            _columnTypes = new List<ColumnType>();
        }

        /// <summary>
        /// Returns a list of already assigned columns to the current DataTable
        /// </summary>
        public IEnumerable<Column> Columns
        {
            get { return _columns; }
        }

        /// <summary>
        /// Returns a list of already assigned rows to the current DataTable       
        /// </summary>
        public IEnumerable<Row> Rows
        {
            get { return _rows; }
        }

        /// <summary>
        /// Creates a new DataRow with the same schema as the table.
        /// </summary>
        /// <returns>A new row with the same schema as the table</returns>
        public Row NewRow()
        {
            return new Row
            {
                ColumnTypes = _columnTypes
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
            //TODO: Add a check so that is not possible to add rows that contains a wrong cell type value 
            _rows.Add(row);
            return row;
        }

        /// <summary>
        /// Adds a new column to the current DataTable
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        public Column AddColumn(Column column)
        {
            AssignDefaultColumnName(column);

            ThrowExceptionIfDuplicateId(column);

            _columns.Add(column);
            
            //adding to the column type list as internal cache
            _columnTypes.Add(column.ColumnType);
            return column;
        }

        private void ThrowExceptionIfDuplicateId(Column column)
        {
            var isExistingId = _columns.Any(x => x.Id == column.Id);

            if (isExistingId)
            {
                throw new Exception(string.Format("Column with the ID: '{0}' already exists", column.Id));
            }
        }

        private void AssignDefaultColumnName(Column column)
        {
            if (string.IsNullOrEmpty(column.Id))
            {
                column.Id = "Column " + (_columns.Count + 1);
            }
        }

        /// <summary>
        /// Returns a Json string compatible with the Google DataTable notation.
        /// </summary>
        /// <returns></returns>
        public string GetJson()
        {
            string rowsJson;
            using (var ms = new MemoryStream())
            {
                var sw = new StreamWriter(ms);

                JsonizeColumns(sw);

                sw.Write(",");

                JsonizeRows(sw);

                sw.Flush();
                ms.Position = 0;
                using (var sr = new StreamReader(ms))
                {
                    rowsJson = sr.ReadToEnd();                    
                };                
            }

            return "{" + rowsJson + "}";
        }

        /// <summary>
        /// Serializes the Rows. 
        /// <remarks>
        /// The choice to have inline both the row and the cell 
        /// serialization is purely for the performance reasons.
        /// </remarks>
        /// </summary>
        /// <param name="sw"></param>
        private void JsonizeRows(StreamWriter sw)
        {
            var rowCount = _rows.Count;
            var lastItem = rowCount - 1;

            sw.Write(" \"rows\" : [");

            //foreach row... 
            for (int i = 0; i < rowCount; i++)
            {
                var currentRow = _rows[i];

                sw.Write("{\"c\" : [");

                var rowCellCount = currentRow.Cells.Count();
                var lastCell = rowCellCount - 1;

                //for each cell
                for (int j = 0; j < rowCellCount; j++)
                {
                    var currentCell = currentRow.Cells.ElementAt(j);

                    //Cells Start
                    sw.Write("{");

                    var value = currentCell.GetFormattedValue(currentCell.ColumnType, currentCell.Value);

                    if (!string.IsNullOrEmpty(value))
                    {
                        sw.Write("\"v\": " + value + "");

                        //if the value is empty then the formatted value
                        //should be empty too.
                        var formatted = currentCell.Formatted;
                        if (formatted!=null && formatted.Length != 0)
                        {
                            sw.Write(", \"f\": \"" + formatted + "\"");
                        }
                    }

                    var p = currentCell.Properties;
                    if (value!=null && value.Length > 0 && !string.IsNullOrEmpty(p))
                    {
                        sw.Write(string.Format(", \"p\":{{{0}}}, ", p));
                    }

                    sw.Write("}");
                    //Cells End

                    if (j != lastCell)
                    {
                        sw.Write(", ");
                    }
                }
                sw.Write("]}");


                if (i != lastItem)
                {
                    sw.Write(", ");
                }
            }
            sw.Write("]");
        }

        /// <summary>
        /// Serializes the Columns into the Json format
        /// </summary>
        /// <returns></returns>
        private void JsonizeColumns(StreamWriter sw)
        {
            int max = _columns.Count;
            int last = max - 1;

            sw.Write("\"cols\"" + ": [");
            for (int i = 0; i < max; i++)
            {
                _columns[i].GetJson(sw);
                if (i != last)
                {
                    sw.Write(", ");
                }
            }
            sw.Write("]");
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
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