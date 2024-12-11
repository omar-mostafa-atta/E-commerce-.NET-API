using E_commerce.Core.DTO;
using E_commerce.Core.Models;
using FluentEmail.Core;
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
	public class RegisterController : ControllerBase
	{
		private static readonly Dictionary<string, string> OtpStore = new Dictionary<string, string>();
		private readonly UserManager<ApplicationUser> userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		public RegisterController(UserManager<ApplicationUser> _UserManager,  RoleManager<IdentityRole> roleManager,IFluentEmail fluentEmail)
		{
			userManager = _UserManager;
			 
			_roleManager = roleManager;
			FluentEmail = fluentEmail;
		}

		public IFluentEmail FluentEmail { get; }

		[HttpPost]
		public async Task<IActionResult> Register(ApplicationUserDTO userFromRequest)
		{
			if (ModelState.IsValid)
			{
				Random random=new Random();
				string OTP=random.Next(0,1000000).ToString("D6");
				OtpStore[userFromRequest.Email] = OTP;


				await FluentEmail.To(userFromRequest.Email)
					.Subject("verify your OTP email")
					.Body($"your OTP is {OTP}")
					.SendAsync();
			return Ok("we have sent you an OTP via email");
			}

			return BadRequest(ModelState);
		}

		[HttpPost("confirmRegisteration")]
		public async Task<IActionResult> ConfirmRegisteration(ConfirmApplicationUserDTO userFromRequest)
		{

			if (ModelState.IsValid &&OtpStore.TryGetValue(userFromRequest.Email, out var storedOtp) &&storedOtp == userFromRequest.OTP)
			{
				OtpStore.Remove(userFromRequest.Email);

				var user = new ApplicationUser
				{
					UserName = userFromRequest.UserName,
					Email = userFromRequest.Email
				};

				var result = await userManager.CreateAsync(user, userFromRequest.Password);
				if (result.Succeeded)
				{

					await userManager.AddToRoleAsync(user,"User");
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
		
			return BadRequest("Invalid OTP or Email");

		}
	}
}
