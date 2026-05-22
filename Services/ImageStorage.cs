using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;

namespace Windows_Programing.Services
{
    public static class ImageStorage
    {
        private const long MaxImageSize = 2 * 1024 * 1024;
        private static readonly HashSet<string> AllowedExtensions = new(StringComparer.OrdinalIgnoreCase)
        {
            ".jpg", ".jpeg", ".png", ".webp", ".gif"
        };

        public static async Task<string?> SaveProfileImageAsync(IFormFile? imageFile, IWebHostEnvironment environment)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                return null;
            }

            var extension = Path.GetExtension(imageFile.FileName);
            if (!AllowedExtensions.Contains(extension))
            {
                throw new InvalidOperationException("Only JPG, PNG, WEBP, and GIF images are allowed.");
            }

            if (imageFile.Length > MaxImageSize)
            {
                throw new InvalidOperationException("Image size must be 2 MB or less.");
            }

            var uploadsFolder = Path.Combine(environment.WebRootPath, "images", "uploads");
            Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = $"{Guid.NewGuid():N}{extension.ToLowerInvariant()}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            await using var fileStream = new FileStream(filePath, FileMode.Create);
            await imageFile.CopyToAsync(fileStream);

            return uniqueFileName;
        }

        public static void DeleteUploadedImage(string? imageName, IWebHostEnvironment environment)
        {
            if (string.IsNullOrWhiteSpace(imageName))
            {
                return;
            }

            var filePath = Path.Combine(environment.WebRootPath, "images", "uploads", imageName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
