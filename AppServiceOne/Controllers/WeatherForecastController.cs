using AppCore.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace AppServiceOne.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetContactInfo")]
        public IEnumerable<ContactInfo> Get()
        {
            var list = new List<ContactInfo>();
            ContactInfo contactInfo = new ContactInfo
            {
                address = new List<Address>(),
                company = "test",
                name = "test",
                surName = "test"
            };

            var factory = new ConnectionFactory()
            {
                HostName = "192.168.43.97",
                UserName = "eren",
                Password = "eren",
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


                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, eventArgs) =>
                {
                    var body = eventArgs.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    list.Add(new ContactInfo { name=message});
                };

                return list;
            }
        }
    }
}