using E_commerce.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_commerce.Repository.Data.Configurations
{
	internal class CategoryConfigurations : IEntityTypeConfiguration<Category>
	{
		public void Configure(EntityTypeBuilder<Category> builder)
		{
			
			builder.Property(x => x.Name)
				   .IsRequired()
				   .HasMaxLength(100);

			builder.Property(x => x.Description)
				   .IsRequired()
				   .HasMaxLength(500);
		}
	}
}