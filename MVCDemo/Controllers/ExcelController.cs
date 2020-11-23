using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;

namespace MVCDemo.Controllers
{
    [AllowAnonymous]
    public class ExcelController : Controller
    {
        // GET: Excel
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(bool flag = false)
        {
            var _workbook = new XSSFWorkbook();
            var _sheet = _workbook.CreateSheet("Sheet1");

            var headerStyle = _workbook.CreateCellStyle(); //Formatting
            var headerFont = _workbook.CreateFont();
            headerFont.IsBold = true;
            headerStyle.SetFont(headerFont);

            var _headers = new List<string>() { "Column A", "Column B", "Column C", "Column D"
                                            ,"Column E", "Column F", "Column G", "Column H"
                                            ,"Column I", "Column J", "Column K", "Column L"};

            var header = _sheet.CreateRow(0);
            for (int i = 0; i < _headers.Count; i++)
            {
                var cell = header.CreateCell(i);
                cell.SetCellValue(_headers[i]);
                cell.CellStyle = headerStyle;
            }

            for (int i = 0; i < 100; i++)
            {
                var row = _sheet.CreateRow(i + 1);
                for (int j = 0; j < 12; j++)
                {
                    var cell = row.CreateCell(j);
                    cell.SetCellValue(string.Format("{0} * {1} = {2}", i + 1, j + 1, (i + 1) * (j + 1)));
                }

                _sheet.AutoSizeColumn(i);
            }

            using (var memoryStream = new MemoryStream())
            {
                _workbook.Write(memoryStream);
                var response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(memoryStream.ToArray())
                };

                return File(memoryStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Excel.xlsx");
            }
        }
    }
}