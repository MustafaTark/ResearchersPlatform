using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using ResearchersPlatform.Extenstions;
using ResearchersPlatform.Hubs;
using ResearchersPlatform_DAL.Models;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
               policy =>
               {
                   policy.WithOrigins("http://localhost:3000").AllowAnyHeader().AllowAnyMethod();
               });
});
builder.Services.ConfigureLifeTime();
builder.Services.ConfigureSqlContext(builder.Configuration);
builder.Services.ConfigureIdentity<User>();
builder.Services.ConfigureIdentity<Student>();
builder.Services.ConfigureJwt(builder.Configuration);
builder.Services.AddSignalR();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddSignalR();
builder.Services.AddControllers().AddJsonOptions(
  opt =>
      opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles
);
builder.Services.AddMemoryCache();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseCors(MyAllowSpecificOrigins);
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapHub<DiscussionHub>("hubs/discussion");
app.MapHub<ChatHub>("hubs/chat");
app.Run();
