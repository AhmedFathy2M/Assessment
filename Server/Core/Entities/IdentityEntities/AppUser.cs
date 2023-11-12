using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
namespace Core.Entities.IdentityEntities
{
	public class AppUser: IdentityUser
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public bool MarketingConsent { get; set; }
		public void GenerateId(string email)
		{
			string salt = "450d0b0db2bcf4adde5032eca1a7c416e560cf44";
			string combinedString = email + salt;
			using (SHA1 sha1 = SHA1.Create())
			{
				byte[] hashBytes = sha1.ComputeHash(Encoding.UTF8.GetBytes(combinedString));
				Id = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
			}
		}

		public string VertifyId()
		{
			using (var sha1 = new SHA1Managed())
			{
				var hashedBytes = sha1.ComputeHash(Encoding.UTF8.GetBytes(Email + "450d0b0db2bcf4adde5032eca1a7c416e560cf44"));
				return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
			}
		}

	}
}
