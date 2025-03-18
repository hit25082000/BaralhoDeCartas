using BaralhoDeCartas.Api;
using BaralhoDeCartas.Api.Interfaces;
using BaralhoDeCartas.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configurar HttpClient
builder.Services.AddHttpClient<IBaralhoApiClient, BaralhoApiClient>();

// Registrar serviços
builder.Services.AddScoped<IBaralhoApiClient, BaralhoApiClient>();
builder.Services.AddScoped<IJogoService, JogoService>();
builder.Services.AddScoped<IBlackjackService, BlackjackService>();

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
