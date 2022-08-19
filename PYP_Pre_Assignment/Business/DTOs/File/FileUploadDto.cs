using Microsoft.AspNetCore.Http;

namespace Business.DTOs.File
{
    public class FileUploadDto
    {
        public IFormFile File { get; set; } 
    }
}
