namespace ETICARET.WebUI.Helpers
{
    public static class ProductImageHelper
    {
        public static string GetUrl(string? imageUrl)
        {
            var fileName = Path.GetFileName(imageUrl);
            if (string.IsNullOrWhiteSpace(fileName))
            {
                fileName = "placeholder.jpg";
            }

            return $"/img/{Uri.EscapeDataString(fileName)}";
        }
    }
}
