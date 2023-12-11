using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using green_craze_be_v1.Application.Common.Options;
using green_craze_be_v1.Application.Intefaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace green_craze_be_v1.Infrastructure.Services
{
    public class UploadService : IUploadService
    {
        private readonly Cloudinary _cloudinary;

        public UploadService(IConfiguration configuration)
        {
            CloudinaryOptions cloudinaryOptions = new();
            configuration.GetSection("Cloudinary").Bind(cloudinaryOptions);
            var account = new Account(cloudinaryOptions.CloudName, cloudinaryOptions.APIKey, cloudinaryOptions.APISecret);
            _cloudinary = new Cloudinary(account);
            _cloudinary.Api.Secure = true;
        }

        private static string GetPublicId(string url)
        {
            return Path.GetFileNameWithoutExtension(url);
        }

        public async Task DeleteFile(string url)
        {
            var publicId = GetPublicId(url);
            await _cloudinary.DestroyAsync(new(publicId));
        }

        public async Task<string> UploadFile(IFormFile file)
        {
            if (file == null) return "";

            var uploadParams = new RawUploadParams()
            {
                File = new FileDescription(file.FileName, file.OpenReadStream())
            };
            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            return uploadResult.Url.AbsoluteUri;
        }
    }
}