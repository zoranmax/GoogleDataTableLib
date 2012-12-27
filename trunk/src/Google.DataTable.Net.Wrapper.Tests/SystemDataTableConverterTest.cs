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
            System.Data.DataTable sysDt = new System.Data.DataTable();
            sysDt.Columns.Add("firstcolumn", typeof(string));
            sysDt.Columns.Add("secondcolumn", typeof(int));
            sysDt.Columns.Add("thirdcolumn", typeof(decimal));

            var dataTable = SystemDataTableConverter.Convert(sysDt);

            Assert.That(dataTable != null);
            Assert.That(dataTable.Columns.Count() == 3);
            Assert.That(dataTable.Columns.ElementAt(0).ColumnType == ColumnType.String);
            Assert.That(dataTable.Columns.ElementAt(1).ColumnType == ColumnType.Number);
            Assert.That(dataTable.Columns.ElementAt(2).ColumnType == ColumnType.Number);
        }

        [Test]
        public void ConverterReturnsColumnsAndRows()
        {
            System.Data.DataTable sysDt = new System.Data.DataTable();
            sysDt.Columns.Add("firstcolumn", typeof(string));
            sysDt.Columns.Add("secondcolumn", typeof(int));
            sysDt.Columns.Add("thirdcolumn", typeof(decimal));

            var row1 = sysDt.NewRow();
            row1[0] = "Ciao";
            row1[1] = 10;
            row1[2] = 2.2;
            sysDt.Rows.Add(row1);

            var dataTable = SystemDataTableConverter.Convert(sysDt);

            Assert.That(dataTable != null);
            Assert.That(dataTable.Rows.Count() == 1);
            Assert.That((string)dataTable.Rows.ElementAt(0).Cells.ElementAt(0).Value == "Ciao");
            Assert.That((int)dataTable.Rows.ElementAt(0).Cells.ElementAt(1).Value == 10);
            Assert.That((decimal)dataTable.Rows.ElementAt(0).Cells.ElementAt(2).Value == 2.2m);
        }
    }
}
