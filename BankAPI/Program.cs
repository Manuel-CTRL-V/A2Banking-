using BankAPI.DataAccess.Configuration;

var builder = WebApplication.CreateBuilder(args);

DatabaseConfig.Initialize(builder.Configuration);

builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ContractResolver =
            new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();
        options.SerializerSettings.NullValueHandling =
            Newtonsoft.Json.NullValueHandling.Ignore;
        options.SerializerSettings.DateFormatString = "yyyy-MM-ddTHH:mm:ss";
    });

var app = builder.Build();

app.UseRouting();
app.MapControllers();

app.Run();
