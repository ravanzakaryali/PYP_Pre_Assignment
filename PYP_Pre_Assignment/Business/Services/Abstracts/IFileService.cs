using Business.DTOs.File;
using Microsoft.AspNetCore.Http;

namespace Business.Services.Abstracts
{
    public interface IFileService
    {
        Task ExcelToDb(FileUploadDto file);
        Task<string> FileUploadAsync(IFormFile file);
    }
}
