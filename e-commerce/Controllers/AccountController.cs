using E_commerce.Core.DTO;
using E_commerce.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace e_commerce.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AccountController : ControllerBase
	{
		private readonly UserManager<ApplicationUser> userManager;
		private readonly IConfiguration config;
		private readonly RoleManager<IdentityRole> _roleManager;
		public AccountController(UserManager<ApplicationUser> _UserManager,IConfiguration _config, RoleManager<IdentityRole> roleManager)//IConfiguration to allow me to read from appsetting
		{
			userManager = _UserManager;
			config = _config;
			_roleManager = roleManager;
		}

		[HttpPost("Register")]
		public async Task<IActionResult> Register(ApplicationUserDTO userFromRequest)
		{
			if (ModelState.IsValid)
			{
				var user = new ApplicationUser
				{
					UserName = userFromRequest.UserName,
					Email = userFromRequest.Email
				};

				var result = await userManager.CreateAsync(user, userFromRequest.Password);
				if (result.Succeeded)
				{
				
					await userManager.AddToRoleAsync(user, "User");
					return Created();
				}
				else
				{
					foreach (var error in result.Errors)
					{
						ModelState.AddModelError("", error.Description);
					}
				}
			}

			return BadRequest(ModelState);
		}

		[HttpPost("Login")]
		public async Task<IActionResult> Login(LoginDTO UserFromRequest)
		{
			if (ModelState.IsValid)
			{
				ApplicationUser userfromdb = await userManager.FindByNameAsync(UserFromRequest.UserName);
				if (userfromdb != null)
				{
					bool checkPassword = await userManager.CheckPasswordAsync(userfromdb, UserFromRequest.Password);
					if (checkPassword == true)
					{
						//designing token
						List<Claim> MyClaims = new List<Claim>();
						MyClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));//This is token id not user id
						MyClaims.Add(new Claim(ClaimTypes.NameIdentifier, userfromdb.Id));
						MyClaims.Add(new Claim(ClaimTypes.Name, userfromdb.UserName));

					var Userroles =	await userManager.GetRolesAsync(userfromdb);
						foreach(var RoleName in Userroles)
						{
							MyClaims.Add(new Claim(ClaimTypes.Role, RoleName));
						}

						var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:SecurityKey"]));
						var SignCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

						//ading token claims
						JwtSecurityToken MyToken = new JwtSecurityToken(
							issuer: config["JWT:IssureIP"],
							audience:config["JWT:AudienceIP"],//dh fe case eno ANGULAR 
							expires: DateTime.Now.AddHours(5),
							claims: MyClaims,
							signingCredentials: SignCredentials

							);
						return Ok(new {

							token = new JwtSecurityTokenHandler().WriteToken(MyToken)
							, expiration =MyToken.ValidTo
							});

					}
				}
				ModelState.AddModelError("UserName", "User Name or Password are incorrect");
			}
			return BadRequest(ModelState);
		}


		[HttpPost("AssignRole")]
		//[Authorize(Roles = "Admin")]
		public async Task<IActionResult> AssignRole(string userId, string roleName)
		{
			var user = await userManager.FindByIdAsync(userId);
			if (user == null)
			{
				return NotFound("User not found.");
			}

			if (!await _roleManager.RoleExistsAsync(roleName))
			{
				return BadRequest("Role does not exist.");
			}

			var result = await userManager.AddToRoleAsync(user, roleName);
			if (result.Succeeded)
			{
				return Ok($"Role '{roleName}' assigned to user '{user.UserName}'.");
			}

			return BadRequest("Failed to assign role.");
		}

		
		
		
		[HttpGet("GetRoles/{userId}")]
		//[Authorize(Roles = "Admin")]
		public async Task<IActionResult> GetUserRoles(string userId)
		{
			var user = await userManager.FindByIdAsync(userId);
			if (user == null)
			{
				return NotFound("User not found.");
			}

			var roles = await userManager.GetRolesAsync(user);
			return Ok(roles);
		}


	}
}
