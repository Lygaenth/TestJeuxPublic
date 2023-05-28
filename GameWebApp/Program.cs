public static class Program
{

	public static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		var urls = new string[] { @"https://localhost:5001" };

		// Add services to the container.

		builder.Services.AddControllers();
		// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
		builder.Services.AddEndpointsApiExplorer();
		builder.Services.AddSwaggerGen();
		builder.WebHost.UseUrls(urls);
	
		var app = builder.Build();
		app.Urls.Add(urls[0]);

		// Configure the HTTP request pipeline.
		if (app.Environment.IsDevelopment())
		{
			app.UseSwagger();
			app.UseSwaggerUI();
		}

		app.UseHttpsRedirection();

		app.UseAuthorization();

		app.MapControllers();

		app.Run();
	}

}

