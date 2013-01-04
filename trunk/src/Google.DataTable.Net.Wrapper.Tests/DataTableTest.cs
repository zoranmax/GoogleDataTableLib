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
using System.Runtime.Serialization;
using NUnit.Framework;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Linq;

namespace Google.DataTable.Net.Wrapper.Tests
{
    [TestFixture]
    public class DataTableTest
    {
        [Test(Description="Creates a new DataTable instance by using the default constructor")]
        public void CanCreateDataTableWithEmptyConstructor()
        {
            //Arrange ------------
            DataTable dt = null; 
            
            //Act -----------------
            dt = new DataTable();

            //Assert --------------
            Assert.IsTrue(dt!=null);
        }

        [Test]
        public void CanAddNewRowToTheDataTable()
        {
            //Arrange ------------
            DataTable dt = GetNewDataTableInstance();
            Row row = dt.NewRow();

            //Act -----------------
            dt.AddRow(row);
            
            //Assert --------------
            Assert.IsTrue(dt.Rows.Count() == 1);
        }

        [Test]
        public void CanAddColumnToTheDataTable()
        {
            //Arrange ------------
            DataTable dt = GetNewDataTableInstance();
            Column col = new Column(ColumnType.String);
            
            //Act -----------------
            dt.AddColumn(col);
           
            //Assert --------------
            Assert.IsTrue(dt.Columns.Count() == 1);
        }

        [Test]
        public void CanListColumns()
        {
            //Arrange ------------
            DataTable dt = GetNewDataTableInstance();
            Column col = new Column(ColumnType.String, "column1");
            Column col2 = new Column(ColumnType.Number, "column2");

            dt.AddColumn(col);
            dt.AddColumn(col2);

            //Act -----------------
            var columns = dt.Columns;

            //Assert --------------
            Assert.IsTrue(columns.Count() == 2);
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public void ColumnsWithTheSameIdAreNotAccepted()
        {
            const string COLUMN_NAME = "whatever_column_name";

            //Arrange ------------
            DataTable dt = GetNewDataTableInstance();
            Column col = new Column(ColumnType.String, COLUMN_NAME, "");
            Column col2 = new Column(ColumnType.String, COLUMN_NAME, "");

            //Act -----------------
            dt.AddColumn(col);
            dt.AddColumn(col2);

            //Assert --------------
            //mentioned in the header as the attribute.
        }

        [Test(Description = "Unfortunately, is a known issue that the deserialization for a custom generated class doesn't work")]
        [ExpectedException(typeof(SerializationException))]
        public void CanDeserializeDataTableJsonWithJSonNet()
        {
            //Arrange ------------
            DataTable dt = GetExampleDataTable();

            //Act ----------------
            var json = dt.GetJson();
            var des = DeserializeFromJson(json);

            //Assert --------------
            Assert.IsTrue(json != null);
            Assert.IsTrue(des != null);
        }

        private DataTable DeserializeFromJson(string json)
        {
            var dataTable = JsonConvert.DeserializeObject<DataTable>(json);
            return dataTable;
        }

        [Test(Description="Test that the serialization/deserialization by using Json.NET is working")]
        public void CanSerializeAndDeserializeWithJsonNet()
        {
            //Arrange ------------
            var dt = GetExampleDataTable();
            
            //Act ----------------
            var dtSerialized = JsonConvert.SerializeObject(dt);
            var dtDeserialized = JsonConvert.DeserializeObject<DataTable>(dtSerialized);

            //Assert--------------
            Assert.IsTrue(dtSerialized != null);
            Assert.IsTrue(dtDeserialized != null);
        }

        /// <summary>
        /// Get some data to work on...
        /// </summary>
        /// <returns></returns>
        private DataTable GetExampleDataTable()
        {
            DataTable dt = GetNewDataTableInstance();
            var columnYear = new Column(ColumnType.Number, "Year");
            var columnCount = new Column(ColumnType.String, "Count");

            //Act -----------------
            dt.AddColumn(columnYear);
            dt.AddColumn(columnCount);

            var row1 = dt.NewRow();
            var row2 = dt.NewRow();
            var row3 = dt.NewRow();

            row1.AddCellRange(new Cell[]
                {
                    new Cell() {Value = 2012, Formatted = "2012"},
                    new Cell() {Value = 1, Formatted = "1"}
                });

            row2.AddCellRange(new Cell[]
                {
                    new Cell() {Value = 2013, Formatted = "2013"},
                    new Cell() {Value = 100, Formatted = "100"}
                });

            row3.AddCellRange(new Cell[]
                {
                    new Cell() {Value = 2014, Formatted = "2014"},
                    new Cell() {Value = 50, Formatted = "50"}
                });

            dt.AddRow(row1);
            dt.AddRow(row2);
            dt.AddRow(row3);
            return dt;
        }

        /// <summary>
        /// Returns a new instance of the DataTable.
        /// </summary>
        /// <returns></returns>
        public DataTable GetNewDataTableInstance()
        {
          return new DataTable();
        }

        [Test]
        [Explicit]
        public void RunPerformanceTest()
        {
            const int TOTAL_NUM_OF_ROWS = 500;

            DataTable dt = GetNewDataTableInstance();
            var columnYear = new Column(ColumnType.Number, "Year");
            var columnCount = new Column(ColumnType.String, "Count");

            //Act -----------------
            dt.AddColumn(columnYear);
            dt.AddColumn(columnCount);
            
            var sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < TOTAL_NUM_OF_ROWS; i++)
            {
                var row = dt.NewRow();
                row.AddCellRange(new Cell[]
                {
                    new Cell() {Value = 2012, Formatted = "2012"},
                    new Cell() {Value = 1, Formatted = "1"}
                });
                dt.AddRow(row);
            }
            sw.Stop();
            Debug.WriteLine("Adding rows: " + sw.ElapsedMilliseconds + " ms");
            sw.Reset();
            
            sw.Start();
            var json = dt.GetJson();
            sw.Stop();
            Debug.WriteLine("Serialization takes: " + sw.ElapsedMilliseconds + " ms");
            Assert.That(json != null);
            Assert.That(json.Length > 0);
            Assert.True(true);
        }
    }
}
