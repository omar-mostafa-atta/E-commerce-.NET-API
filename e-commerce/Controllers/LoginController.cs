using E_commerce.Core.DTO;
using E_commerce.Core.Models;
using FluentEmail.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
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
	public class LoginController : ControllerBase
	{
		private IFluentEmail FluentEmail { get; }
		private static readonly Dictionary<string, string> OtpStore = new Dictionary<string, string>();
		private readonly UserManager<ApplicationUser> userManager;
		private readonly IConfiguration config;

		public LoginController(UserManager<ApplicationUser> _UserManager, IConfiguration _config, IFluentEmail fluentEmail)//IConfiguration to allow me to read from appsetting
		{
			userManager = _UserManager;
			config = _config;
			FluentEmail = fluentEmail;
		}


		[HttpPost]
		public async Task<IActionResult> Login(LoginDTO UserFromRequest)
		{
			ApplicationUser userfromdb = await userManager.FindByNameAsync(UserFromRequest.UserName);

			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			if (userfromdb == null)
			{
				ModelState.AddModelError("UserName", "User Name or Password are incorrect");
				return BadRequest(ModelState);
			}
			bool checkPassword = await userManager.CheckPasswordAsync(userfromdb, UserFromRequest.Password);
			if (checkPassword != true)
			{
				ModelState.AddModelError("Password", " Password is incorrect");
				return BadRequest(ModelState);
			}
			//designing token
			List<Claim> MyClaims = new List<Claim>();
			MyClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));//This is token id not user id
			MyClaims.Add(new Claim(ClaimTypes.NameIdentifier, userfromdb.Id));
			MyClaims.Add(new Claim(ClaimTypes.Name, userfromdb.UserName));
			MyClaims.Add(new Claim(ClaimTypes.Email, userfromdb.Email));

			var Userroles = await userManager.GetRolesAsync(userfromdb);
			foreach (var RoleName in Userroles)
			{
				MyClaims.Add(new Claim(ClaimTypes.Role, RoleName));
			}

			var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:SecurityKey"]));
			var SignCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

			//ading token claims
			JwtSecurityToken MyToken = new JwtSecurityToken(
				issuer: config["JWT:IssureIP"],
				audience: config["JWT:AudienceIP"],//dh fe case eno ANGULAR 
				expires: DateTime.Now.AddHours(5),
				claims: MyClaims,
				signingCredentials: SignCredentials
				);
			return Ok(new
			{
				token = new JwtSecurityTokenHandler().WriteToken(MyToken),
				expiration = MyToken.ValidTo
			});
		}

		
		
		[HttpPost("ResetPassword")]
		public async Task<IActionResult> ResetPassword(ResetPasswordDTO request)
		{
			 
			if (ModelState.IsValid && OtpStore.TryGetValue(request.Email, out var storedOtp) && storedOtp == request.OTP)
			{
				OtpStore.Remove(request.Email);
				var user = await userManager.FindByEmailAsync(request.Email);
				var token = await userManager.GeneratePasswordResetTokenAsync(user);
				if (String.IsNullOrEmpty(token))
					return BadRequest("something went wrong");
				var result = await userManager.ResetPasswordAsync(user, token, request.NewPassword);
				if (result.Succeeded)
					return Ok("your password has been changed");
				return BadRequest("something went wrong");
			}
			return BadRequest("something went wrong");

		}





		[HttpPost]
		[Route("ForgotPassword")]
		public async Task<IActionResult> ForgotPassword(ForgotPasswordDTO request)
		{
			if(!ModelState.IsValid)
				return BadRequest("invalid Payload");

			var user=await userManager.FindByEmailAsync(request.Email);
			if (user == null)
				return BadRequest("User not found");

			
			Random random = new Random();
			string OTP = random.Next(0, 1000000).ToString("D6");
			OtpStore[request.Email] = OTP;

			await FluentEmail.To(request.Email)
				.Subject("Reset Password OTP verification")
				.Body($"your OTP number is {OTP}")
				.SendAsync();
			return Ok($"OTP has been sent to your email{user.Email}");
		}

	}
}
