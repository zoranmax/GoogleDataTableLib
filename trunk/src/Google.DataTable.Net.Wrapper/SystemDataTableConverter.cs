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
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Google.DataTable.Net.Wrapper
{
    /// <summary>
    /// Responsible for converting the System.Data.DataTable into
    /// Google.DataTable.Net.Wrapper.DataTable.
    /// </summary>
    public class SystemDataTableConverter
    {
        /// <summary>
        /// Responsible for converting the System.Data.DataTable into
        /// Google.DataTable.Net.Wrapper.DataTable.        
        /// </summary>
        public static DataTable Convert(System.Data.DataTable dataTable)
        {
            if (dataTable == null)
            {
                throw new ArgumentNullException("dataTable");
            }

            DataTable dt = new DataTable();

            foreach (System.Data.DataColumn column in dataTable.Columns)
            {
                var columnType = GetColumnTypeFromType(column.DataType);
                var id = column.ColumnName;
                var label = column.Caption;
                
                dt.AddColumn(new Column(columnType, id, label));
            }

            int nrOfColumns = dt.Columns.Count();

            foreach (System.Data.DataRow systemRow in dataTable.Rows)
            {
                Row row = dt.NewRow();

                List<Cell> cell = new List<Cell>();
                for (int i = 0; i < nrOfColumns; i++)
                {
                    cell.Add(new Cell(systemRow[i]));
                }
                row.AddCellRange(cell.ToArray());
                dt.AddRow(row);
            }
            return dt;
        }

        /// <summary>
        /// For more info about System.Data.DataColumn Types 
        /// http://msdn.microsoft.com/en-us/library/system.data.datacolumn.datatype.aspx
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        private static ColumnType GetColumnTypeFromType(Type t)
        {
            switch (t.FullName.ToString(CultureInfo.InvariantCulture))
            {
                case "System.Decimal":
                case "System.Double":
                case "System.Single":
                case "System.Byte":
                case "System.Int16":
                case "System.Int32":
                case "System.Int64":
                case "System.SByte":
                case "System.UInt16":
                case "System.UInt32":
                case "System.UInt64":
                    return ColumnType.Number;
                case "System.Boolean":
                    return ColumnType.Boolean;
                case "System.Char":
                case "System.String":
                case "System.Guid":
                    return ColumnType.String;
                case "System.DateTime":
                    return ColumnType.Datetime;
                case "System.TimeSpan":
                    return ColumnType.Timeofday;
                default:
                    return ColumnType.String;
            }
        }
    }
}
