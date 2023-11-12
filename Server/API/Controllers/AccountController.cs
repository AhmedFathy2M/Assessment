using API.Dtos;
using Core.Entities.IdentityEntities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
	public class AccountController:BaseController
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signInUser;
		private readonly ITokenService _tokenService;
		public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signUser, ITokenService tokenService)
		{
			_userManager = userManager;
			_signInUser = signUser;
			_tokenService = tokenService;
		}
		/// <summary>
		/// This API returns the current authorized user.
		/// </summary>
		/// <returns>UserDto first and last name and Email</returns>
		[Authorize(AuthenticationSchemes = "Bearer")]
		[HttpGet]
		public async Task<ActionResult<UserDto>> GetCurrentUser()
		{
			var email = User.FindFirstValue(ClaimTypes.Email);
			var user = await _userManager.FindByEmailAsync(email);
			return new UserDto
			{
				FirstName = user.FirstName,
				LastName = user.LastName,
				Email = user.Email

			};
		}
		/// <summary>
		/// A help function that checks if the email being registered is in the database already
		/// </summary>
		/// <param name="email">the email being checked, if it already exists in the database</param>
		/// <returns></returns>
		[HttpGet("emailexists")]
		public async Task<ActionResult<bool>> CheckIfEmailExistsAsync([FromQuery] string email)
		{
			return await _userManager.FindByEmailAsync(email) != null;
		}
		/// <summary>
		/// logs in the user in the database
		/// </summary>
		/// <param name="loginDto">Has the properties needed to login the user</param>
		/// <returns></returns>
		[HttpPost("login")]
		public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
		{
			var user = await _userManager.FindByEmailAsync(loginDto.Email);
			if (user == null)
			{
				return Unauthorized(new UnauthorizedAccessException());
			}
			else
			{
				var result = await _signInUser.CheckPasswordSignInAsync(user, loginDto.Password, false);
				if (!result.Succeeded)
				{
					return Unauthorized(new UnauthorizedAccessException());
				}
				else
				{
					return new UserDto
					{
						Email = user.Email,
						FirstName = user.FirstName,
						LastName = user.LastName,
						Token = _tokenService.CreateToken(user)

					};
				}
			}


		}
		/// <summary>
		/// Registers the user
		/// </summary>
		/// <param name="registerDto">Has the properties needed to register the user</param>
		/// <returns></returns>
		[HttpPost("register")]
		public async Task<ActionResult<RegisteredUserDto>> Register(RegisterDto registerDto)
		{
			if (CheckIfEmailExistsAsync(registerDto.Email).Result.Value)
			{
				return BadRequest("Email is already in use");
			}
			var user = new AppUser()
			{
				FirstName = registerDto.FirstName,
				LastName = registerDto.LastName,
				Email = registerDto.Email,
				UserName = registerDto.Email,
				MarketingConsent = registerDto.MarketingConsent
			};

			var result = await _userManager.CreateAsync(user, registerDto.Password);
			if (!result.Succeeded)
			{
				return BadRequest(new BadRequestResult());
			}
			else
			{
				return new RegisteredUserDto
				{
					Id = user.Id,
					Token = _tokenService.CreateToken(user)

				};
			}

		}
		/// <summary>
		/// finds the user using his/her id and token
		/// </summary>
		/// <param name="Id">Parameter used to find the user along with the Token</param>
		/// <param name="Token">Parameter used to find the user along with the Id</param>
		/// <returns></returns>
		[HttpGet("find-user")]
		public async Task<ActionResult<UserDto>> FindUser(string Id, string Token)
		{
			var user = await _userManager.FindByIdAsync(Id);

			if (user == null)
			{
				return NotFound("User not found");
			}
			var tokenIsValid = _tokenService.ValidateToken(user, Token);

			if (!tokenIsValid)
			{
				return Unauthorized("Invalid token");
			}
			if(user.MarketingConsent == false)
			{
				return new UserDto
				{
					Id = Id,
					FirstName = user.FirstName,
					LastName = user.LastName,
					MarketingConsent = user.MarketingConsent,
					Email = user.Email
				};
			}
			else
			{
				return new UserDto
				{
					Id = Id,
					FirstName = user.FirstName,
					LastName = user.LastName,
					MarketingConsent = user.MarketingConsent
				};
			}
		
		}
	}

}

