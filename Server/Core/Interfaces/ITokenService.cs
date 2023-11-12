using Core.Entities.IdentityEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
	public interface ITokenService
	{
		string CreateToken(AppUser user);
		bool ValidateToken(AppUser user, string token);
	}
}
