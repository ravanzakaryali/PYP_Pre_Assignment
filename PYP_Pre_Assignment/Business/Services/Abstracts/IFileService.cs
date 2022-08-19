using Business.DTOs.File;

namespace Business.Services.Abstracts
{
    public interface IFileService
    {
        Task ExcelToDb(FileUploadDto file);
        Task<string> FileUploadAsync(IFormFile file);
    }
}
