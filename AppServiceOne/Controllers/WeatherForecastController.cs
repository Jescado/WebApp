using AppCore.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace AppServiceOne.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private ConnectionFactory factory;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
            factory = new ConnectionFactory()
            {
                HostName = "192.168.43.97",
                UserName = "eren",
                Password = "eren",
            };
        }

        [HttpGet(Name = "SetQue")]
        public bool Set()
        {
            ContactInfo contactInfo = new()
            {
                address = new List<Address>(),
                company = "test",
                name = "test",
                surName = "test"
            };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                //channel.QueueDeclare(queue: "teatqueue",
                //                     durable: false,
                //                     exclusive: false,
                //                     autoDelete: false,
                //                     arguments: null);

                var message = JsonConvert.SerializeObject(contactInfo);
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                                                 routingKey: "teatqueue",
                                                 basicProperties: null,
                                                 body: body);
            }

            return true;
        }

        //[HttpGet(Name = "GetExcel")]
        //public IActionResult GetExcel()
        //{
        //    string fileName = "ExcelList-" + Guid.NewGuid().ToString() + ".xlsx";

        //    List<ContactInfo>? list = new List<ContactInfo>();
        //    using (var connection = factory.CreateConnection())
        //    using (var channel = connection.CreateModel())
        //    {
        //        var consumer = new AsyncEventingBasicConsumer(channel);
        //        consumer.Received += async (ch, ea) =>
        //        {
        //            var body = ea.Body.ToArray();
        //            var text = System.Text.Encoding.UTF8.GetString(body);
        //            await Task.CompletedTask;
        //            list.Add(new ContactInfo { name = text });
        //            channel.BasicAck(ea.DeliveryTag, false);
        //        };
        //        channel.BasicConsume("teatqueue", false, consumer);
        //    }
        //    using (MemoryStream stream = new MemoryStream())
        //    {
        //        IWorkbook workbook = new XSSFWorkbook();
        //        ISheet excelSheet = workbook.CreateSheet(fileName);
        //        List<string> columns = new List<string>();
        //        IRow row = excelSheet.CreateRow(0);
        //        int columnIndex = 0;

        //        //foreach (System.Data.DataColumn column in dataTable.Columns)
        //        {
        //            columns.Add("Name");//(column.ColumnName);
        //            row.CreateCell(columnIndex).SetCellValue("Name");//(column.ColumnName);
        //            columnIndex++;
        //        }

        //        int rowIndex = 1;
        //        foreach (var dsrow in list)
        //        {
        //            row = excelSheet.CreateRow(rowIndex);
        //            int cellIndex = 0;
        //            foreach (String col in columns)
        //            {
        //                row.CreateCell(cellIndex).SetCellValue(dsrow.name.ToString());
        //                cellIndex++;
        //            }

        //            rowIndex++;
        //        }

        //        workbook.Write(stream);
        //        return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        //    }
        //}
    }
}