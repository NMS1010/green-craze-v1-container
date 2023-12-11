using Microsoft.AspNetCore.Http;

namespace green_craze_be_v1.Application.Intefaces
{
    public interface IUploadService
    {
        Task<string> UploadFile(IFormFile file);

        Task DeleteFile(string url);
    }
}