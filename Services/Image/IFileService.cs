namespace MediNet_BE.Services.Image
{
	public interface IFileService
	{
		//public Task<IFormFile> GetImage(string imageFile);
		//public Task<IFormFile[]> GetImages(string imagesFile);
		public Tuple<int, string> SaveImages(IFormFile[] imagesFile, string filePath);
		public Tuple<int, string> SaveImage(IFormFile imageFile, string filePath);
		public Task DeleteImage(string imageFile);
		public Task DeleteImages(string imagesFile);

	}
}
