using System.Collections.Generic;
using System.Linq;
using Google.DataTable.Net.Wrapper.Extension;
using NUnit.Framework;

namespace Google.DataTable.Net.Wrapper.Tests.Extension
{
    [TestFixture]
    public class GenericListConverterTest
    {
        [Test]
        public void CanConvertAnonymousListToGoogleDataTable()
        {
            var list = new[]
                {
                    new {Name = "Dogs", Count = 5},
                    new {Name = "Cats", Count = 2}
                };

            var dataTable = list.ToGoogleDataTable()
                                .NewColumn(new Column(ColumnType.String, "Name"), x => x.Name)
                                .NewColumn(new Column(ColumnType.Number, "Count"), x => x.Count)
                                .Build();

            Assert.That(dataTable.Columns.Count() == 2);
            Assert.That(dataTable.Rows.Count() == 2);
            Assert.That(dataTable.Columns.First().Id == "Name");
            Assert.That(dataTable.Columns.Last().Id == "Count");
        }

        [Test]
        public void CanConvertAnonymousListToGoogleDataTableWithoutCreatingColumns()
        {
            var list = new[]
                {
                    new {Name = "Dogs", Count = 5},
                    new {Name = "Cats", Count = 2}
                };


            var dataTable = list.ToGoogleDataTable().Build();

            Assert.That(dataTable.Columns.Count() == 2);
            Assert.That(dataTable.Rows.Count() == 2);
            Assert.That(dataTable.Columns.First().Id == "Name");
            Assert.That(dataTable.Columns.Last().Id == "Count");
        }

        [Test]
        public void CanConvertStronglyTypedListToGoogleDataTableWithoutSpecifyingColumns()
        {
            var list = new List<PersonTest>
                {
                    new PersonTest
                        {
                            Name = "Jackie",
                            Surname = "Chen",
                            Person = new PersonTest {Name = "Bruce", Surname = "Lee"}
                        },
                    new PersonTest {Name = "James", Surname = "Bond", Person = null}
                };


            var dataTable = list
                .ToGoogleDataTable()
                .Build();

            Assert.That(dataTable.Columns.Count() == 3);
            Assert.That(dataTable.Rows.Count() == 2);
            Assert.That(dataTable.Columns.First().Id == "Name");
            Assert.That(dataTable.Columns.Last().Id == "Person");          
        }

        [Test]
        public void CanConvertStronglyTypedListToGoogleDataTable()
        {
            var list = new List<PersonTest>
                {
                    new PersonTest
                        {
                            Name = "Jackie",
                            Surname = "Chen",
                            Person = new PersonTest {Name = "Bruce", Surname = "Lee"}
                        },
                    new PersonTest {Name = "James", Surname = "Bond", Person = null}
                };


            var dataTable = list
                .ToGoogleDataTable()
                .NewColumn(new Column(ColumnType.String, "Name"), x => x.Name)
                .NewColumn(new Column(ColumnType.String, "Surname"), x => x.Surname)
                .Build();

            Assert.That(dataTable.Columns.Count() == 2);
            Assert.That(dataTable.Rows.Count() == 2);
            Assert.That(dataTable.Columns.First().Id == "Name");
            Assert.That(dataTable.Columns.Last().Id == "Surname");
        }

        //test class to be included in a list
        private class PersonTest
        {
            public string Name { get; set; }
            public string Surname { get; set; }

            //this shouldn't be converted.
            public PersonTest Person { get; set; }

            public override string ToString()
            {
                return "abc";
            }
        }
    }
}