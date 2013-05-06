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

using System.Globalization;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Google.DataTable.Net.Wrapper.Tests
{
    [TestFixture]
    public class SystemDataTableConverterTest
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConverterThrowsErrorIfDataTableParameterIsNull()
        {
            SystemDataTableConverter.Convert(null);
        }

        [Test]
        public void ConverterReturnsColumns()
        {
            using (var sysDt = new System.Data.DataTable())
            {
                sysDt.Columns.Add("firstcolumn", typeof (string));
                sysDt.Columns.Add("secondcolumn", typeof (int));
                sysDt.Columns.Add("thirdcolumn", typeof (decimal));
                sysDt.Locale = CultureInfo.InvariantCulture;

                var dataTable = SystemDataTableConverter.Convert(sysDt);

                Assert.That(dataTable != null);
                Assert.That(dataTable.Columns.Count() == 3);
                Assert.That(dataTable.Columns.ElementAt(0).ColumnType == ColumnType.String);
                Assert.That(dataTable.Columns.ElementAt(1).ColumnType == ColumnType.Number);
                Assert.That(dataTable.Columns.ElementAt(2).ColumnType == ColumnType.Number);
            }
        }

        [Test]
        public void ConverterReturnsColumnsAndRows()
        {
            using (var sysDt = new System.Data.DataTable())
            {
                sysDt.Columns.Add("firstcolumn", typeof (string));
                sysDt.Columns.Add("secondcolumn", typeof (int));
                sysDt.Columns.Add("thirdcolumn", typeof (decimal));
                sysDt.Locale = CultureInfo.InvariantCulture;

                var row1 = sysDt.NewRow();
                row1[0] = "Ciao";
                row1[1] = 10;
                row1[2] = 2.2;
                sysDt.Rows.Add(row1);

                var dataTable = SystemDataTableConverter.Convert(sysDt);

                Assert.That(dataTable != null);
                Assert.That(dataTable.Rows.Count() == 1);
                Assert.That((string) dataTable.Rows.ElementAt(0).Cells.ElementAt(0).Value == "Ciao");
                Assert.That((int) dataTable.Rows.ElementAt(0).Cells.ElementAt(1).Value == 10);
                Assert.That((decimal) dataTable.Rows.ElementAt(0).Cells.ElementAt(2).Value == 2.2m);
            }
        }
    }
}
