using Microsoft.AspNetCore.Http;
using System.IO;

namespace Company.Service.Helper
{
    public class DocumentSettings
    {
        public static string UploudFile(IFormFile? file, string folderName)
        {
            if (file == null || file.Length == 0)
            {
                return null;
            }

            try
            {
                // Validate file size (e.g., 5MB limit)
                if (file.Length > 5 * 1024 * 1024)
                {
                    throw new ArgumentException("File size exceeds 5MB limit.");
                }

                // Validate file type (optional)
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(extension))
                {
                    throw new ArgumentException("Invalid file type. Only JPG, JPEG, PNG, and GIF are allowed.");
                }

                // Get folder path
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Files", folderName);
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                // Generate unique file name
                var fileName = $"{Guid.NewGuid()}{extension}";
                var filePath = Path.Combine(folderPath, fileName);

                // Save file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                // Return relative path
                return $"/Files/{folderName}/{fileName}";
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to upload file: {ex.Message}", ex);
            }
        }
    }
}
