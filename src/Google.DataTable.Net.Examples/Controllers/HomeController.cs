using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Google.DataTable.Net.Wrapper;

namespace Google.DataTable.Net.Examples.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Example1()
        {
            return View();
        }

        public ActionResult Example2()
        {
            //passing the ViewBag.JSON to the view.
            ViewBag.JSON = Example2Data();
            return View();
        }

        public string Example1Data()
        {
            var dt = new Wrapper.DataTable();

            //Act -----------------
            dt.AddColumn(new Column(ColumnType.String, "Tiers"));
            dt.AddColumn(new Column(ColumnType.Number, "Apps"));

            var row1 = dt.NewRow();
            var row2 = dt.NewRow();
            var row3 = dt.NewRow();


            row1.AddCellRange(new[] {new Cell("Tier 1"), new Cell(20)});
            row2.AddCellRange(new[] {new Cell("Tier 1.5"), new Cell(13)});
            row3.AddCellRange(new[] {new Cell("Tier 2"), new Cell(34)});


            dt.AddRow(row1);
            dt.AddRow(row2);
            dt.AddRow(row3);

            return dt.GetJson();
        }

        public string Example2Data()
        {
            var dt = new Wrapper.DataTable();

            //Act -----------------
            dt.AddColumn(new Column(ColumnType.String, "Year"));
            dt.AddColumn(new Column(ColumnType.Number, "Sales", "Sales - Legend"));
            dt.AddColumn(new Column(ColumnType.Number, "Expenses"));

            var row1 = dt.NewRow();
            var row2 = dt.NewRow();
            var row3 = dt.NewRow();
            var row4 = dt.NewRow();
            var row5 = dt.NewRow();
            var row6 = dt.NewRow();

            row1.AddCellRange(new[] {new Cell("2009"), new Cell(10), new Cell(20)});
            row2.AddCellRange(new[] {new Cell("2010"), new Cell(10), new Cell(50)});
            row3.AddCellRange(new[] {new Cell("2011"), new Cell(20), new Cell(10)});
            row4.AddCellRange(new[] {new Cell("2012"), new Cell(11), new Cell(20)});
            row5.AddCellRange(new[] {new Cell("2013"), new Cell(18), new Cell(18)});
            row6.AddCellRange(new[] {new Cell("2014"), new Cell(30), new Cell(19)});


            dt.AddRow(row1);
            dt.AddRow(row2);
            dt.AddRow(row3);
            dt.AddRow(row4);
            dt.AddRow(row5);
            dt.AddRow(row6);

            return dt.GetJson();
        }

    }
}
