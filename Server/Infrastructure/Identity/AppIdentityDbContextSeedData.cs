using Core.Entities.IdentityEntities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Identity
{
	public class AppIdentityDbContextSeedData
	{
		public static async Task SeedUserData(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
		{
			if (!userManager.Users.Any())
			{
				var user = new AppUser()
				{
					FirstName = "Ahmed",
					LastName ="Fathy",
					Email = "ahmedfathymohamed1998@gmail.com",
					UserName = "Ahmed"
					


				};

				await roleManager.CreateAsync(new IdentityRole() { Name = "admin" });

				await userManager.CreateAsync(user, "AhmedFathy_2m");
				await userManager.AddToRoleAsync(user, "admin");

			}
		}
	}
}
