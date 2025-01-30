using E_commerce.Core.DTO;
using E_commerce.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace e_commerce.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(Roles = "Admin")]
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
		[Authorize(Roles = "Admin")]
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
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> GetUserRolesByName(string username)
		{
		 
			var user = await _userManager.FindByNameAsync(username);
			if (user == null)
			{
				return NotFound(new { Message = "User not found" });
			}

		 
			var roles = await _userManager.GetRolesAsync(user);
			if (roles == null || roles.Count == 0)
			{
				return NotFound(new { Message = "User has no roles assigned" });
			}

	 
			return Ok(new
			{
				Username = username,
				Roles = roles
			});
		}

		[HttpGet("GetUserRolesById/{userId}")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> GetUserRolesById(string userId)
		{
			var user = await _userManager.FindByIdAsync(userId);
			if (user == null)
			{
				return NotFound("User not found.");
			}

			var roles = await _userManager.GetRolesAsync(user);
			return Ok(roles);
		}



		[HttpPut("AssignRole")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> AssignRole(string userId, string roleName)
		{
			var user = await _userManager.FindByIdAsync(userId);
			if (user == null)
			{
				return NotFound("User not found.");
			}

			if (!await _RoleManager.RoleExistsAsync(roleName))
			{
				return BadRequest("Role does not exist.");
			}

			var result = await _userManager.AddToRoleAsync(user, roleName);
			if (result.Succeeded)
			{
				return Ok($"Role '{roleName}' assigned to user '{user.UserName}'.");
			}

			return BadRequest("Failed to assign role.");
		}

		[HttpDelete("Delete Role/{roleName}")]
		[Authorize(Roles ="Admin")]
		public async Task<IActionResult> DeleteRole(string roleName)
		{
			if (!await _RoleManager.RoleExistsAsync(roleName))
			{
				return BadRequest("Role does not exist.");
			}
			var role = await _RoleManager.FindByNameAsync(roleName);

			var result = await _RoleManager.DeleteAsync(role);
			if (result.Succeeded)
			{
				return Ok($"Role '{roleName}' has been deleted successfully.");
			}

			return BadRequest("Failed to delete the role.");
		}



	}
}
