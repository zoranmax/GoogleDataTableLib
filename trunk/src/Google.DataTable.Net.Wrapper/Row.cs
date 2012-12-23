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

using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Google.DataTable.Net.Wrapper.Common;

namespace Google.DataTable.Net.Wrapper
{
    /// <summary>
    /// Each row object has one required property called c, 
    /// which is an array of cells in that row. 
    /// It also has an optional p property that defines a map of arbitrary 
    /// custom values to assign to the whole row. 
    /// If your visualization supports any row-level properties it will describe them; 
    /// otherwise, this property will be ignored.
    /// </summary>
    public class Row: ISerializable
    {
        /// <summary>
        /// Internal constructor as we don't allow the direct generation
        /// of the row due to the fact that the table Attribute is set
        /// internally
        /// </summary>
        internal Row()
        {
            Cells = new List<Cell>();
        }

        /// <summary>
        /// DataTable to which the Row belongs
        /// </summary>

        internal List<ColumnType> ColumnTypes { get; set; }       

        /// <summary>
        /// Adds a range of Cell objects
        /// </summary>
        /// <param name="cells"></param>
        public void AddCellRange(Cell[] cells)
        {
            foreach (var c in cells)
            {
                this.AddCell(c);
            }
        }

        /// <summary>
        /// Adds a single cell to the Row
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public Cell AddCell(Cell cell)
        {
            //find this cell ordinal number
            var cellNr = this.Cells.Count();

            //assign the 
            cell.ColumnType = ColumnTypes.ElementAt(cellNr);

            //let's do some check around the type

            Cells.Add(cell);
            return cell;
        }

        /// <summary>
        /// 
        /// </summary>
        public List<Cell> Cells { get; set; }

        public string GetJson()
        {
            var sb = new StringBuilder();

            foreach (Cell c in Cells)
            {
                sb.Append(c.GetJson());
                sb.Append(", ");
            }

            string json = Helper.DoubleQuoteString("c") + ": [" + sb.ToString().Trim().TrimEnd(',') + "]";

            return "{" + json.TrimEnd(',') + "}";
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("c", this.Cells);             
        }

        protected Row (SerializationInfo info, StreamingContext context)
        {
            // Reset the property value using the GetValue method.
            Cells = (List<Cell>)info.GetValue("c", typeof(List<Cell>));
        }
    }
}