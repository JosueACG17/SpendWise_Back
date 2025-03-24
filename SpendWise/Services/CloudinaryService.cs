using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace SpendWise.Services
{
    public class CloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(IConfiguration configuration)
        {
            var account = new Account(
                configuration["Cloudinary:CloudName"],
                configuration["Cloudinary:ApiKey"],
                configuration["Cloudinary:ApiSecret"]
            );

            _cloudinary = new Cloudinary(account);
        }

        public async Task<ImageUploadResult?> UploadImageToCloudinary(IFormFile file, string folderName)
        {
            if (file == null || file.Length == 0)
                return null;

            // Lista de tipos MIME permitidos para imágenes
            var allowedMimeTypes = new[] { "image/jpeg", "image/png" };

            if (!allowedMimeTypes.Contains(file.ContentType))
            {
                throw new InvalidOperationException("Solo se permiten archivos de imagen.");
            }

            await using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = folderName
            };

            return await _cloudinary.UploadAsync(uploadParams);
        }
    }
}