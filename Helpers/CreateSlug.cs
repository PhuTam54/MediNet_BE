using System.Text.RegularExpressions;

namespace MediNet_BE.Helpers
{
	public static class CreateSlug
	{
		public static string Init_Slug(string text)
		{
			text = text.ToLower();
			text = text.Trim();
			for (int i = 32; i < 48; i++)
			{

				text = text.Replace(((char)i).ToString(), " ");

			}
			text = text.Replace(".", "-");

			text = text.Replace(" ", "-");

			text = text.Replace(",", "-");

			text = text.Replace(";", "-");

			text = text.Replace(":", "-");

			Regex regex = new Regex(@"\p{IsCombiningDiacriticalMarks}+");

			string strFormD = text.Normalize(System.Text.NormalizationForm.FormD);

			return regex.Replace(strFormD, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');

		}
	}
}
