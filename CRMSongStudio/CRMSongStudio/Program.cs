using CRMSongStudio;
using Serilog;

//Log.Logger = new LoggerConfiguration()
//    .WriteTo.Console()
//    .WriteTo.File(path:"logs/crmSongStudio/log-. log",outputTemplate: "{Timestamp:o} [{Level:u3}] ({SourceContext}) {Message}{NewLine}{Exception}",rollingInterval:RollingInterval.Day)
//    .MinimumLevel.Debug()
//    .CreateLogger();
  
    
var builder = WebApplication.CreateBuilder(args);
_ = builder.Host.UseSerilog((httpHost, config) => 
    _ =config.ReadFrom.Configuration(builder.Configuration));


var connectionstring = builder.Configuration.GetConnectionString("DefaultConnectrings");
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option => 
{
    option.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "CRMSongStudio.API", Version = "v1" });
});

var app = builder.Build();





// Configure the HTTP request pipeline.
if (builder.Configuration.GetValue<bool>("useSwagger"))
{
   app.UseSwagger();
   app.UseSwaggerUI();
}
List<Song> songs = new List<Song>() {
    new Song(){
        Id = 1,
        Name = "Lac Troi",
        Author = "Son Tung",
        Description = "Ta lac troi giua doi",
        Price = 10000
    }
};

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapGet("api/minimal/getallsong", () =>
{
    return Results.Ok(songs);
}
);
app.MapPost("api/minimal/addsong", (Song song) =>
{
    songs.Add(song);
    return Results.Ok(songs);
});
app.Run();
