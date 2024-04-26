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
				return new Tuple<int, string>(1, newFileName);
			}
			catch (Exception ex)
			{
				return new Tuple<int, string>(0, "Error has occured");
			}
		}

		public Tuple<int, string> SaveImages(IFormFile[] files, string filePath)
		{
			try
			{
				string pictures = "";
				foreach (var formFile in files)
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
						//pictures = "";
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

		public async Task DeleteImage(string imageFileName, string filePath)
		{
			var path = Path.Combine(this.environment.WebRootPath, $"{filePath}", imageFileName);
			if (File.Exists(path))
				File.Delete(path);
		}

		
	}
}
