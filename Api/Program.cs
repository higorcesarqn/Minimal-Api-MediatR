using Api;
using Api.Handlers;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(x => x.AsScoped(), typeof(Program));
builder.Services.AddNotificatons();

builder.Services.AddRequest<ExampleSuccessRequest, ExampleRequestSuccessHandler>();
builder.Services.AddRequest<ExampleUnsuccessRequest, ExampleRequestUnsuccessHandler>();
builder.Services.AddRequest<ExamplePostRequest, ExampleRequestPostHandler>();

var app = builder.Build();

app.MediatGet<ExampleSuccessRequest>("example/success/{name}");
app.MediatGet<ExampleUnsuccessRequest>("example/unsuccess/{name}");
app.MediatPost<ExamplePostRequest>("example/post/{section}");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();