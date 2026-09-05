var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// register tarefas repository and service
builder.Services.AddSingleton<CP4_to_do_api.Tarefas.Repository.TarefasRepository, CP4_to_do_api.Tarefas.Repository.JsonTarefasRepository>();
builder.Services.AddScoped<CP4_to_do_api.Tarefas.Services.TarefasService>();

// optional: set default file path for tarefas.json inside app folder
builder.Configuration["TarefasFile"] ??= Path.Combine(AppContext.BaseDirectory, "tarefas.json");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
