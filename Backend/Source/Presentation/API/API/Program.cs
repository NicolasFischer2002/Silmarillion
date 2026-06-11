using API.Extensions;
using API.Modules.AccessControl;
using Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddApi();

var app = builder.Build();

app.UseApi();

app.MapAccessControlEndpoints();

app.Run();