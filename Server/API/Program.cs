
using API.Extensions;
using Core.Entities.IdentityEntities;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddDbContext<AppIdentityDbContext>(p => p.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection")));
			builder.Services.AddIdentityServices(builder.Configuration);
			builder.AddApplicationServices();
			builder.Services.AddSwaggerAuthorizationConfiguration();
			builder.Services.AddSwaggerGen();

			var app = builder.Build();
			using (var scope = app.Services.CreateScope())
			{

				var services = scope.ServiceProvider;
				var loggerfactory = services.GetRequiredService<ILoggerFactory>();
				try
				{
			

					var userManager = services.GetRequiredService<UserManager<AppUser>>();
					var identityContext = services.GetRequiredService<AppIdentityDbContext>();
					await identityContext.Database.MigrateAsync();
					var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
					await AppIdentityDbContextSeedData.SeedUserData(userManager, roleManager);


				}
				catch (Exception ex)
				{
					var logger = loggerfactory.CreateLogger<Program>(); // where u create it in oprogram
					logger.LogError(ex, "error occured during migration");
				}


			}
			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();
			app.UseCors("CorsPolicy");
			app.UseAuthentication();
			app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}
	}
}