using Core.Entities.IdentityEntities;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace API.Extensions
{
	public static class IdentityServiceExtension
	{
		public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration _config)
		{

			services.AddIdentity<AppUser, IdentityRole>()
			  .AddEntityFrameworkStores<AppIdentityDbContext>()
			  .AddSignInManager<SignInManager<AppUser>>();
			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters()
			{
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Token:Key"])),
				ValidIssuer = _config["Token:Issuer"],
				ValidateIssuer = true,
				ValidateAudience = false
			});
			return services;

		}
	}
}
