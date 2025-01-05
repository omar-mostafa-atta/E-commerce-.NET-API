using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

 
namespace E_commerce.Services.Services.ImageService
{
	public class ImageService : IImageService
	{
		private readonly IWebHostEnvironment _webHostEnvironment;

		public ImageService(IWebHostEnvironment webHostEnvironment)
		{
			_webHostEnvironment = webHostEnvironment;
		}


		//public async Task<string> SaveImageAsync(IFormFile Img, string folderPath)
		//{
		//	if (Img == null || Img.Length == 0)
		//	{
		//		return null;
		//	}

		//	string fileName = Guid.NewGuid().ToString();
		//	string extension = Path.GetExtension(Img.FileName);
		//	string filePath = Path.Combine(_webHostEnvironment.WebRootPath, folderPath, fileName + extension);

		//	using (var fileStream = new FileStream(filePath, FileMode.Create))
		//	{
		//		await Img.CopyToAsync(fileStream);
		//	}

		//	return Path.Combine(folderPath, fileName + extension);
		//}
		public async Task<string> SaveImageAsync(IFormFile file, string folderPath)
		{
			if (!Directory.Exists(folderPath))
			{
				Directory.CreateDirectory(folderPath);
			}

			string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
			string filePath = Path.Combine(folderPath, fileName);

			using (var stream = new FileStream(filePath, FileMode.Create))
			{
				await file.CopyToAsync(stream);
			}

			return Path.Combine("Images/ProductImages", fileName); // Return relative path
		}

		public async Task DeleteFileAsync(string? filePath)
		{
			if (!string.IsNullOrEmpty(filePath))
			{
				string absolutePath = Path.Combine("wwwroot", filePath);
				if (File.Exists(absolutePath))
				{
					File.Delete(absolutePath);
				}
			}
		}


		//public async Task DeleteFileAsync(string FilePath)
		//{
		//	if (FilePath != null)
		//	{
		//		var RootPath = _webHostEnvironment.WebRootPath;
		//		var oldFile = Path.Combine(RootPath, FilePath);

		//		if (System.IO.File.Exists(oldFile))
		//		{
		//			System.IO.File.Delete(oldFile);
		//		}
		//	}
		//}
	}
}
