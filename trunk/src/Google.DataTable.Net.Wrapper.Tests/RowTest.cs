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

using NUnit.Framework;
using System.Linq;
using System.Collections.Generic;

namespace Google.DataTable.Net.Wrapper.Tests
{
    [TestFixture]
    public class RowTest
    {
        [Test]
        public void Row_RowWithDefaultValues()
        {
            //Arrange ------------
            Row r = new Row(); //this is an internal constructor!

            //Act -----------------

            //Assert --------------
            Assert.IsNotNull(r);
            Assert.That(r.Cells.Count() == 0);
            Assert.That(r.ColumnTypes.Count() == 0);
        }

        [Test]
        public void Row_CanAddACell()
        {
            //Arrange ------------
            Row r = new Row();
            r.ColumnTypes = new List<ColumnType>() { ColumnType.Number };
            Cell c = new Cell(100, "100");

            //Act -----------------
            r.AddCell(c);

            //Assert --------------
            Assert.That(r.Cells.Count() == 1);
        }

        [Test]
        public void Row_CanAddARangeOfCells()
        {
            //Arrange ------------
            Row r = new Row();
            r.ColumnTypes = new List<ColumnType>() { ColumnType.Number, ColumnType.Number };

            Cell c = new Cell(100);
            Cell c2 = new Cell(200);
            //Act -----------------
            r.AddCellRange(new[] { c, c2 });

            //Assert --------------
            Assert.That(r.Cells.Count() == 2);
        }

        [Test]
        [ExpectedException(typeof(System.ArgumentOutOfRangeException))]
        public void Row_AddARangeOfCellsThatHaveMoreItemsThanColumnsRaisesException()
        {
            //Arrange ------------
            Row r = new Row();
            r.ColumnTypes = new List<ColumnType>() { ColumnType.Number };
            Cell c = new Cell(100);
            Cell c2 = new Cell(200);
            //Act -----------------
            r.AddCellRange(new[] { c, c2 });

            //Assert --------------

        }
    }
}