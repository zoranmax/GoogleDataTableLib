using System.Globalization;
using System.Linq;
using Google.DataTable.Net.Wrapper.Extension;
using NUnit.Framework;

namespace Google.DataTable.Net.Wrapper.Tests.Extension
{
    [TestFixture]
    public class SystemDataTableExtensionTest
    {
        [Test]
        public void CanTransformASystemDataTableToGoogleDataTable()
        {
            using (var sysDt = new System.Data.DataTable())
            {
                sysDt.Columns.Add("firstcolumn", typeof(string));
                sysDt.Columns.Add("secondcolumn", typeof(int));
                sysDt.Columns.Add("thirdcolumn", typeof(decimal));
                sysDt.Locale = CultureInfo.InvariantCulture;

                var row1 = sysDt.NewRow();
                row1[0] = "Ciao";
                row1[1] = 10;
                row1[2] = 2.2;
                sysDt.Rows.Add(row1);

                var dataTable = sysDt.ToGoogleDataTable();

                Assert.That(dataTable != null);
                Assert.That(dataTable.Rows.Count() == 1);
                Assert.That((string)dataTable.Rows.ElementAt(0).Cells.ElementAt(0).Value == "Ciao");
                Assert.That((int)dataTable.Rows.ElementAt(0).Cells.ElementAt(1).Value == 10);
                Assert.That((decimal)dataTable.Rows.ElementAt(0).Cells.ElementAt(2).Value == 2.2m);
            }
        }

        [Test]
        public void CanTransformANullSystemDataTableToGoogleDataTable()
        {

            System.Data.DataTable dataTable = null;
            var res = dataTable.ToGoogleDataTable();

            Assert.That(res == null);
        }
    }
}