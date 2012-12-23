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

namespace Google.DataTable.Net.Wrapper.Tests
{
    [TestFixture]
    public class RowTest
    {

        [Test]
        public void GetRowWithTwoCellsAndGetJson()
        {
            //Arrange ------------

            DataTable dt = new DataTable();
            dt.AddColumn(new Column(ColumnType.Number));

            var row1 = dt.NewRow();


            row1.AddCell(new Cell()
                {
                    Value = 100,
                    Formatted = "100",
                    ColumnType = ColumnType.Number
                });

            //Act -----------------
            var json = row1.GetJson();

            //Assert --------------
            Assert.That(json == "{\"c\": [{\"v\": \"100\", \"f\": \"100\"}]}");
        }
    }
}
