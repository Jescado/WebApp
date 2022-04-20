using AppCore.Models;
using AppServiceOne;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Diagnostics;
using WebAppMain.Models;

namespace WebAppMain.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ConnectionFactory factory;
        private readonly IServiceOne _service;

        public HomeController(ILogger<HomeController> logger, IServiceOne service)
        {
            _logger = logger;
            _service= service;
            factory = new ConnectionFactory()
            {
                HostName = "192.168.43.97",
                UserName = "eren",
                Password = "eren",
            };
        }

        public IActionResult Index()
        {
            string fileName = "ExcelList-" + Guid.NewGuid().ToString() + ".xlsx";

            List<ContactInfo>? list = new List<ContactInfo>();

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var consumer = new AsyncEventingBasicConsumer(channel);
                consumer.Received += async (ch, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var text = System.Text.Encoding.UTF8.GetString(body);
                    await Task.CompletedTask;
                    list.Add(new ContactInfo { name = text });
                    channel.BasicAck(ea.DeliveryTag, false);
                };
                channel.BasicConsume("teatqueue", false, consumer);
            }
            using (MemoryStream stream = new MemoryStream())
            {
                IWorkbook workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet(fileName);
                List<string> columns = new List<string>();
                IRow row = excelSheet.CreateRow(0);
                int columnIndex = 0;

                //foreach (System.Data.DataColumn column in dataTable.Columns)
                {
                    columns.Add("Name");//(column.ColumnName);
                    row.CreateCell(columnIndex).SetCellValue("Name");//(column.ColumnName);
                    columnIndex++;
                }

                int rowIndex = 1;
                foreach (var dsrow in list)
                {
                    row = excelSheet.CreateRow(rowIndex);
                    int cellIndex = 0;
                    foreach (String col in columns)
                    {
                        row.CreateCell(cellIndex).SetCellValue(dsrow.name.ToString());
                        cellIndex++;
                    }

                    rowIndex++;
                }

                workbook.Write(stream);
                _service.GetContact(2);
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}