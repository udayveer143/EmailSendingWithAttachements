using EmailService;
using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
builder.Host.ConfigureServices(services =>
{
    var emailConfig = configuration.GetSection("EmailConfiguration").Get<EmailConfigurations>();
    services.AddSingleton(emailConfig);
    services.AddScoped<IEmailSender, EmailSender>();
    services.Configure<FormOptions>(o => 
    {
        o.ValueLengthLimit =int.MaxValue;
        o.MultipartBodyLengthLimit =int.MaxValue;
        o.MemoryBufferThreshold =int.MaxValue;
    });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
