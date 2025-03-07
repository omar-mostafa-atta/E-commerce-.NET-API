using e_commerce.ConfigurationAutoMapper;
using E_commerce.Core.Models;
using E_commerce.Repository.Data;
using E_commerce.Repository.GenericRepository;
using E_commerce.Services.Services.CategoryService;
using E_commerce.Services.Services.OrderService;
using E_commerce.Services.Services.ProductService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using E_commerce.Services.Services.ImageService;
using Stripe;
using ProductService = E_commerce.Services.Services.ProductService.ProductService;

namespace e_commerce
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);


			builder.Services.AddControllers();
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();
			builder.Services.AddScoped<IProductService, ProductService>();
			builder.Services.AddScoped<ICategoryService, CategoryService>();
			builder.Services.AddScoped<IOrderService, OrderService>();
			builder.Services.AddScoped<IImageService, ImageService>();
			builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
			builder.Services.AddCors(options =>
			{
				options.AddPolicy("AllowAll",
						policy => policy.AllowAnyOrigin()
						.AllowAnyMethod()
						.AllowAnyHeader()
				);

			}


			);

			StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];
			builder.Services.AddCors(options =>
			{
				options.AddDefaultPolicy(builder =>
				{
					builder.AllowAnyOrigin()
						.AllowAnyHeader()
						.AllowAnyMethod();
				});
			});
			builder.Services.AddRouting(options => options.LowercaseUrls = true);

			builder.Services.AddDbContext<E_commerceContext>(options =>
				options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


			builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<E_commerceContext>().AddDefaultTokenProviders();

			builder.Services
			.AddFluentEmail("omarmostafaatta@gmail.com", "E-commerce")
			.AddSmtpSender(new SmtpClient("smtp.gmail.com")
			{
				Port = 587,
				Credentials = new NetworkCredential("omarmostafaatta@gmail.com", "zgtx gopp tswt owag"),
				EnableSsl = true
			});





			builder.Services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer(options =>
			{
				options.SaveToken = true;
				options.RequireHttpsMetadata = false;
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidIssuer = builder.Configuration["JWT:IssureIP"],
					ValidateAudience = true,
					ValidAudience = builder.Configuration["JWT:AudienceIP"],
					ValidateLifetime = true,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecurityKey"]))
				};
			});
			JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

			builder.Services.AddAutoMapper(typeof(AutoMapperConfig));

			var app = builder.Build();
			app.UseCors("AllowAll");

			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			using (var scope = app.Services.CreateScope())
			{
				var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
				var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();


				await SeedRolesAndAdminAsync(roleManager, userManager);
			}

			app.UseHttpsRedirection();

			app.UseAuthentication();
			app.UseAuthorization();
			app.UseCors();
			app.UseRouting();
			app.MapControllers();

			app.Run();
		}

		private static async Task SeedRolesAndAdminAsync(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
		{
			var roles = new List<string> { "Admin", "User" };

			foreach (var role in roles)
			{
				if (!await roleManager.RoleExistsAsync(role))
				{
					await roleManager.CreateAsync(new IdentityRole(role));
				}
			}

			var adminEmail = "admin@admin.com";
			var adminUser = await userManager.FindByEmailAsync(adminEmail);
			if (adminUser == null)
			{
				var newAdmin = new ApplicationUser
				{
					UserName = "admin",
					Email = adminEmail,
					EmailConfirmed = true
				};

				var result = await userManager.CreateAsync(newAdmin, "Admin@123");
				if (result.Succeeded)
				{
					await userManager.AddToRoleAsync(newAdmin, "Admin");
				}
				else
				{
					Console.WriteLine("Failed to create admin user:");
					foreach (var error in result.Errors)
					{
						Console.WriteLine($"- {error.Description}");
					}
				}
			}
		}
	}
}
