using Microsoft.OpenApi.Models;

namespace API.Extensions
{
	public static class SwaggerServiceExtensions
	{
		public static IApplicationBuilder AddSwaggerDocumentation(this IApplicationBuilder app)
		{
			app.UseSwagger();
			app.UseSwaggerUI();
			return app;
		}

		public static IServiceCollection AddSwaggerAuthorizationConfiguration(this IServiceCollection services)
		{
			services.AddSwaggerGen(c =>
			{
				var securitySchema = new OpenApiSecurityScheme()
				{
					Description = "JWT Auth Bearer Scheme",
					Name = "Authorization",
					In = ParameterLocation.Header,
					Type = SecuritySchemeType.Http,
					Scheme = "Bearer",
					Reference = new OpenApiReference()
					{
						Type = ReferenceType.SecurityScheme,
						Id = "Bearer"
					}
				};

				c.AddSecurityDefinition("Bearer", securitySchema);
				var securityRequirement = new OpenApiSecurityRequirement()
				{
					{securitySchema, new[]{"Bearer"} }
				};

				c.AddSecurityRequirement(securityRequirement);
			});


			return services;
		}

	}
}

