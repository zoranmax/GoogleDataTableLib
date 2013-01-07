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

namespace Google.DataTable.Net.Wrapper
{
    /// <summary>
    ///A row is an array of cells, 
    ///plus an optional map of arbitrary name/value pairs that you can assign.    
    /// </summary>
    [Serializable]
    public class Row: ISerializable, IPropertyMap
    {
        private readonly List<Cell> _cellList;
        private readonly List<Property> _propertyMap;

        /// <summary>
        /// Internal constructor as we don't allow the direct generation
        /// of the row due to the fact that the table Attribute is set
        /// internally
        /// </summary>
        internal Row()
        {
            _cellList = new List<Cell>();
            _propertyMap = new List<Property>();
            ColumnTypes = new List<ColumnType>();
        }

        /// <summary>
        /// A reference to the available column types of the table. 
        /// </summary>
        internal IEnumerable<ColumnType> ColumnTypes { get; set; }       

        /// <summary>
        /// Adds a range of Cell objects
        /// </summary>
        /// <param name="cells"></param>
        public void AddCellRange(Cell[] cells)
        {
            if (cells == null)
                return;

            for (int i = 0; i < cells.Count(); i++)
            {
                this.AddCell(cells[i], i);
            }
        }

        private Cell AddCell(Cell cell, int index)
        {
            //assigning the type to the cell
            cell.ColumnType = ColumnTypes.ElementAt(index);

            //TODO: Throw an error if Cell Value Type doesn't match the specified Column Type

            _cellList.Add(cell);
            return cell;
        }

        /// <summary>
        /// Adds a single cell to the Row
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public Cell AddCell(Cell cell)
        {
            //find this cell ordinal number
            var index = this.Cells.Count();
            
            return AddCell(cell, index);
        }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<Cell> Cells
        {
            get
            {
                return _cellList;
            }
        }

        /// <summary>
        /// Returns a list of currently assigned properties to the Row
        /// </summary>
        public IEnumerable<Property> PropertyMap
        {
            get { return _propertyMap; }            
        }

        /// <summary>
        /// Adds a new property to the list of properties.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public Property AddProperty(Property p)
        {
            _propertyMap.Add(p);
            return p;
        }

        /// <summary>
        /// Removes a property from the Property Map
        /// </summary>
        /// <param name="p"></param>
        public void RemoveProperty(Property p)
        {
            _propertyMap.Remove(p);
        }

        /// <summary>
        /// Removes a property from the Property Map by an index.
        /// </summary>
        /// <param name="index"></param>
        public void RemoveProperty(int index)
        {
            _propertyMap.RemoveAt(index);
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("c", this.Cells);
            info.AddValue("p", this.PropertyMap);
        }

        protected Row(SerializationInfo info, StreamingContext context)
        {
            // Reset the property value using the GetValue method.
            _cellList = (List<Cell>) info.GetValue("c", typeof (List<Cell>));
            _propertyMap = (List<Property>) info.GetValue("p", typeof (List<Property>));         
        }
    }
}