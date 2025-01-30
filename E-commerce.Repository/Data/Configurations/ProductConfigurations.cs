using E_commerce.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_commerce.Repository.Data.Configurations
{
	internal class ProductConfigurations : IEntityTypeConfiguration<Product>
	{
		public void Configure(EntityTypeBuilder<Product> builder)
		{
			
			builder.Property(x => x.Name)
				   .IsRequired()
				   .HasMaxLength(100);

			builder.Property(x => x.Description)
				   .IsRequired()
				   .HasMaxLength(500);

			builder.Property(x => x.Brand)
				   .HasMaxLength(50);

			builder.Property(x => x.Feedback)
				   .HasMaxLength(1000);

			builder.Property(x => x.Quantity)
				   .IsRequired()
				   .HasDefaultValue(0)
				   .HasAnnotation("CheckConstraint", "Quantity >= 0");

			
			builder.Property(x => x.Price)
				   .IsRequired()
				   .HasColumnType("decimal(18,2)")
				   .HasAnnotation("CheckConstraint", "Price > 0");

			builder.Property(x => x.CategoryId)
				   .IsRequired();

		}
	}
}