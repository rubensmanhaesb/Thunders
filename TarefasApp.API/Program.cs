using TarefasApp.API.Extensions;
using TarefasApp.Application.Extensions;
using TarefasApp.Infra.Data.Extensions;
using TarefasApp.Infra.Storage.Extensions;
using TarefasApp.Infra.Messages.Extensions;
using TarefasApp.Domain.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddSwaggerDoc();
builder.Services.AddApplicationServices();
builder.Services.AddDomainServices();
builder.Services.AddDataContext(builder.Configuration);
builder.Services.AddMongoDb(builder.Configuration);
builder.Services.AddRabbitMQ(builder.Configuration);

var app = builder.Build();

app.UseSwaggerDoc();
app.UseAuthorization();
app.MapControllers();
app.Run();
