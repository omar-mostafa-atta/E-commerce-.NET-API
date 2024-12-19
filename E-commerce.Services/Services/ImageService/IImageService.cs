

using Microsoft.AspNetCore.Http;

namespace E_commerce.Services.Services.ImageService
{
	public interface IImageService
	{
			Task<string> SaveImageAsync(IFormFile Img, string folderPath);
			Task DeleteFileAsync(string File);
		
	}
}
