using LevelEditor.asp.Data;
using LevelEditor.asp.Data.Api;
using TestJeux.Business.Managers;
using TestJeux.Business.Managers.API;

var builder = WebApplication.CreateBuilder(args);

var baseUri = "https://localhost:5001";

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<LevelService>();
builder.Services.AddSingleton<ImageManager>();

builder.Services.AddHttpClient<ILevelService, LevelService>(client => client.BaseAddress = new Uri(baseUri));
builder.Services.AddHttpClient<IShaderService, ShaderService>(client => client.BaseAddress = new Uri(baseUri));
builder.Services.AddHttpClient<IMusicService, MusicService>(client => client.BaseAddress = new Uri(baseUri));

builder.Services.AddSingleton<IImageManager, ImageManager>();

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

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
