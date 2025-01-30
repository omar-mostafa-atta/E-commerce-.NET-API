using E_commerce.Core.Models;
using E_commerce.Repository.GenericRepository;
using E_commerce.Services.Services.ImageService;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq;


namespace E_commerce.Services.Services.ProductService
{
	public class ProductService : IProductService
	{
		private readonly IGenericRepository<Product> _productRepository;
		private readonly IGenericRepository<Category> _categoryRepository;
		private readonly IImageService _imageService;

		public ProductService(IGenericRepository<Product> productRepository, IGenericRepository<Category> categoryRepository, IImageService imageService)
		{
			_productRepository = productRepository;
			_categoryRepository = categoryRepository;
			_imageService = imageService;
		}

		public IQueryable<Product> GetAllAsync()
		{
			return _productRepository.GetAllQueryable();
		}


		public async Task<Product> GetByIdAsync(int id)
		{
			return await _productRepository.GetByIdAsync(id);
		}



		public async Task<IEnumerable<Product>> GetByCategoryIdAsync(int categoryId)
		{
			return await _productRepository.FindAsync(x => x.CategoryId == categoryId);
		}

		public async Task AddAsync(Product product, IFormFile[]? images)
		{
			if (images != null && images.Length > 0)
			{
				for (int i = 0; i < Math.Min(images.Length, 5); i++)
				{
					string folderPath = @"wwwroot/Images/ProductImages/";
					string savedImagePath = await _imageService.SaveImageAsync(images[i], folderPath);

					if (i == 0) product.Img1 = savedImagePath;
					else if (i == 1) product.Img2 = savedImagePath;
					else if (i == 2) product.Img3 = savedImagePath;
					else if (i == 3) product.Img4 = savedImagePath;
					else if (i == 4) product.Img5 = savedImagePath;
				}
			}

			var category = await _categoryRepository.GetByIdAsync(product.CategoryId);
			if (category == null)
			{
				throw new ArgumentException("Invalid CategoryId");
			}

			await _productRepository.AddAsync(product);
		}

		public async Task UpdateAsync(Product product, IFormFile[]? images, string[]? deleteImages)
		{

			if (deleteImages != null)
			{
				foreach (var imageToDelete in deleteImages)
				{
					await _imageService.DeleteFileAsync(imageToDelete);

					if (product.Img1 == imageToDelete) product.Img1 = null;
					else if (product.Img2 == imageToDelete) product.Img2 = null;
					else if (product.Img3 == imageToDelete) product.Img3 = null;
					else if (product.Img4 == imageToDelete) product.Img4 = null;
					else if (product.Img5 == imageToDelete) product.Img5 = null;
				}
			}

			if (images != null && images.Length > 0)
			{
				// Delete old images if necessary
				await _imageService.DeleteFileAsync(product.Img1);
				await _imageService.DeleteFileAsync(product.Img2);
				await _imageService.DeleteFileAsync(product.Img3);
				await _imageService.DeleteFileAsync(product.Img4);
				await _imageService.DeleteFileAsync(product.Img5);

				// Add new images
				for (int i = 0; i < Math.Min(images.Length, 5); i++)
				{
					string folderPath = @"wwwroot/Images/ProductImages/";
					string savedImagePath = await _imageService.SaveImageAsync(images[i], folderPath);

					if (i == 0) product.Img1 = savedImagePath;
					else if (i == 1) product.Img2 = savedImagePath;
					else if (i == 2) product.Img3 = savedImagePath;
					else if (i == 3) product.Img4 = savedImagePath;
					else if (i == 4) product.Img5 = savedImagePath;
				}
			}

			await _productRepository.UpdateAsync(product);
		}
		public async Task UpdateQuantityAsync(int productId, int newQuantity)
		{
			var product = await _productRepository.GetByIdAsync(productId);
			if (product == null)
			{
				throw new Exception($"Product with ID {productId} not found.");
			}

			product.Quantity = newQuantity;
			await _productRepository.UpdateAsync(product);
		}


		public async Task DeleteAsync(int id)
		{
			var product = await _productRepository.GetByIdAsync(id);
			if (product != null)
			{
				await _imageService.DeleteFileAsync(product.Img1);
				await _imageService.DeleteFileAsync(product.Img2);
				await _imageService.DeleteFileAsync(product.Img3);
				await _imageService.DeleteFileAsync(product.Img4);
				await _imageService.DeleteFileAsync(product.Img5);

				await _productRepository.DeleteAsync(id);
			}
		}
	}
}
