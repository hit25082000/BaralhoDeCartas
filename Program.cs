using BaralhoDeCartas.Api;
using BaralhoDeCartas.Api.Interfaces;
using BaralhoDeCartas.Factory;
using BaralhoDeCartas.Factory.Interfaces;
using BaralhoDeCartas.Services;
using BaralhoDeCartas.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configurar HttpClient
builder.Services.AddHttpClient<IBaralhoApiClient, BaralhoApiClient>();

// Registrar servi�os
builder.Services.AddScoped<IJogoService, JogoService>();
builder.Services.AddScoped<IBlackjackService, BlackjackService>();
builder.Services.AddScoped<IJogadorFactory, JogadorFactory>();
builder.Services.AddScoped<ICartaFactory, CartaFactory>();
builder.Services.AddScoped<IBaralhoFactory, BaralhoFactory>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();

app.Run();
