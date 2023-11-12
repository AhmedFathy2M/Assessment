using Core.Interfaces;
using Infrastructure.Services;

namespace API.Extensions
{
	public static class ApplicationServiceExtensions
	{
		public static WebApplicationBuilder AddApplicationServices(this WebApplicationBuilder builder)
		{
			builder.Services.AddScoped<ITokenService, TokenService>();
			builder.Services.AddCors(options => options.AddPolicy("CorsPolicy", policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()));
			return builder;
		}
	}
}
