using MediNet_BE.Dto.Orders;
using Microsoft.IdentityModel.Tokens;
using System.Linq;

namespace MediNet_BE.Services.Image
{
	public class FileService : IFileService
	{
		private IWebHostEnvironment environment;
		public FileService(IWebHostEnvironment env)
		{
			this.environment = env;
		}


		//public async Task<IFormFile> GetImage(string imageFile)
		//{
		//	var path = Path.Combine(environment.WebRootPath, imageFile);
		//	if (File.Exists(path))
		//	{
		//		using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
		//		{
		//			// Đọc dữ liệu từ FileStream và ghi vào MemoryStream
		//			var memoryStream = new MemoryStream();
		//			await stream.CopyToAsync(memoryStream);

		//			// Thiết lập độ dài của MemoryStream
		//			memoryStream.Seek(0, SeekOrigin.Begin);

		//			// Tạo một đối tượng IFormFile từ MemoryStream
		//			IFormFile file = new FormFile(memoryStream, 0, memoryStream.Length, null, Path.GetFileName(path));
		//			return file;
		//		}
		//	}
		//	return null;
		//}
		//public async Task<IFormFile[]> GetImages(string imageFiles)
		//{
		//	var images = new List<IFormFile>();

		//	// Phân tách đường dẫn ảnh bằng dấu ";"
		//	var imagePaths = imageFiles.Split(';');

		//	foreach (var path in imagePaths)
		//	{
		//		// Kiểm tra xem đường dẫn có tồn tại không
		//		if (File.Exists(path))
		//		{
		//			using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
		//			{
		//				// Đọc dữ liệu từ FileStream và ghi vào MemoryStream
		//				var memoryStream = new MemoryStream();
		//				await stream.CopyToAsync(memoryStream);

		//				// Thiết lập độ dài của MemoryStream
		//				memoryStream.Seek(0, SeekOrigin.Begin);

		//				// Tạo một đối tượng IFormFile từ MemoryStream
		//				IFormFile file = new FormFile(memoryStream, 0, memoryStream.Length, null, Path.GetFileName(path));

		//				// Thêm đối tượng IFormFile vào danh sách
		//				images.Add(file);
		//			}
		//		}
		//	}

		//	return images.ToArray();
		//}

		public Tuple<int, string> SaveImage(IFormFile imageFile, string filePath)
		{
			try
			{
					var contentPath = this.environment.WebRootPath;
					var path = Path.Combine(contentPath, filePath);
					if (!Directory.Exists(path))
					{
						Directory.CreateDirectory(path);
					}

					var ext = Path.GetExtension(imageFile.FileName);
					var allowedExtensions = new string[] { ".jpg", ".png", ".jpeg" };
					if (!allowedExtensions.Contains(ext))
					{
						string msg = string.Format("Only {0} extensions are allowed", string.Join(",", allowedExtensions));
						return new Tuple<int, string>(0, msg);
					}
					string uniqueString = Guid.NewGuid().ToString();

					var newFileName = uniqueString + ext;
					var fileWithPath = Path.Combine(path, newFileName);
					var stream = new FileStream(fileWithPath, FileMode.Create);
				    imageFile.CopyTo(stream);
					stream.Close();
				string picture = filePath + newFileName;
				return new Tuple<int, string>(1, picture);
			}
			catch (Exception ex)
			{
				return new Tuple<int, string>(0, "Error has occured");
			}
		}

		public Tuple<int, string> SaveImages(IFormFile[] imagesFile, string filePath)
		{
			try
			{
				string pictures = "";
				foreach (var formFile in imagesFile)
				{
					var contentPath = this.environment.WebRootPath;

					var path = Path.Combine(contentPath, filePath);
					if (!Directory.Exists(path))
					{
						Directory.CreateDirectory(path);
					}

					var ext = Path.GetExtension(formFile.FileName);
					var allowedExtensions = new string[] { ".jpg", ".png", ".jpeg" };
					if (!allowedExtensions.Contains(ext))
					{
						string msg = string.Format("Only {0} extensions are allowed", string.Join(",", allowedExtensions));
						pictures = "";
						return new Tuple<int, string>(0, msg);
					}
					string uniqueString = Guid.NewGuid().ToString();

					var newFileName = uniqueString + ext;
					var fileWithPath = Path.Combine(path, newFileName);
					var stream = new FileStream(fileWithPath, FileMode.Create);
					formFile.CopyTo(stream);
					stream.Close();
					pictures += filePath + newFileName + ";";
				}
				return new Tuple<int, string>(1, pictures);
			}
			catch (Exception ex)
			{
				return new Tuple<int, string>(0, "Error has occured");
			}
		}

		public async Task DeleteImage(string imageFile)
		{
			var path = Path.Combine(this.environment.WebRootPath, imageFile);
			if (File.Exists(path))
				File.Delete(path);
		}

		public async Task DeleteImages(string imagesFile)
		{
			string[] picturePaths = imagesFile.Split(';');

			foreach (string picturePath in picturePaths)
			{
				if (!string.IsNullOrEmpty(picturePath))
				{
					var fullPath = Path.Combine(this.environment.WebRootPath, picturePath.TrimStart('/'));
					if (File.Exists(fullPath))
					{
						File.Delete(fullPath);
					}
				}
			}
		}

	}
}
