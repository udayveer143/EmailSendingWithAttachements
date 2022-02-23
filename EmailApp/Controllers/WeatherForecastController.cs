using EmailService;
using Microsoft.AspNetCore.Mvc;

namespace EmailApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly IEmailSender _emailSender;

        public WeatherForecastController(IEmailSender emailSender)
        {
            _emailSender= emailSender;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            var message = new Message(new string[] { "udayveer143@gmail.com" }, "Test Email Async", "This is the content from our email", null);
            await _emailSender.SendEmailAsync(message);
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
        [HttpPost]
        public async Task<IEnumerable<WeatherForecast>> Post()
        {
            var files=Request.Form.Files.Any() ? Request.Form.Files:new FormFileCollection();
            var message = new Message(new string[] { "udayveer143@gmail.com" }, "Test Email Async with attachements", "This is the content from our email with attachements",files);
            await _emailSender.SendEmailAsync(message);
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}