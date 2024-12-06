using E_commerce.Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Repository.Data
{
	public class E_commerceContext:IdentityDbContext<ApplicationUser>
	{
        public E_commerceContext(DbContextOptions<E_commerceContext>options):base(options)
        {
            
        }
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<OrderProduct>(entity =>
			{
				entity.HasNoKey(); 
			});
		
		}
		public DbSet<Product> Product {  get; set; }
		public DbSet<Payment> Payment {  get; set; }
		public DbSet<Category> Category {  get; set; }
		public DbSet<Order> Order {  get; set; }

	}
}
