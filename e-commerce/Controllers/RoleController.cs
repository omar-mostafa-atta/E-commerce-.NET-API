using E_commerce.Core.DTO;
using E_commerce.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace e_commerce.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class RoleController : ControllerBase
	{
		public readonly RoleManager<IdentityRole> _RoleManager;
		private readonly UserManager<ApplicationUser> _userManager;
		public RoleController(RoleManager<IdentityRole> RoleManager, UserManager<ApplicationUser> userManager)
		{
			_RoleManager = RoleManager;
			_userManager = userManager;	
		}
		[HttpPost("AddRole")]
		public async Task<IActionResult> AddRole(RoleDTO RoleFromRequest)
		{
			if (ModelState.IsValid)
			{
				IdentityRole role = new IdentityRole();
				role.Name = RoleFromRequest.RoleName;
				var result = await _RoleManager.CreateAsync(role);
				if (result.Succeeded)
				{
					return Ok(role);
				}
				foreach (var error in result.Errors)
				{
					ModelState.AddModelError("", error.Description);
				}
			}
			
			return BadRequest(ModelState);

		}

		[HttpGet("GetUserRoles/{username}")]
		public async Task<IActionResult> GetUserRoles(string username)
		{
			// Find the user by username
			var user = await _userManager.FindByNameAsync(username);
			if (user == null)
			{
				return NotFound(new { Message = "User not found" });
			}

			// Get the roles for the user
			var roles = await _userManager.GetRolesAsync(user);
			if (roles == null || roles.Count == 0)
			{
				return NotFound(new { Message = "User has no roles assigned" });
			}

			// Return the roles
			return Ok(new
			{
				Username = username,
				Roles = roles
			});
		}


	}
}
