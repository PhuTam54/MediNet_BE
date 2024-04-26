namespace MediNet_BE.Services.Image
{
	public interface IFileService
	{
		public Tuple<int, string> SaveImages(IFormFile[] files, string filePath);
		public Tuple<int, string> SaveImage(IFormFile imageFile, string filePath);
		public Task DeleteImage(string imageFileName, string filePath);

	}
}
