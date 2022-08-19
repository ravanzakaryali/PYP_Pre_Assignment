using Business.DTOs.Excel;
using Business.DTOs.File;
using Business.Exceptions.FileExceptions;
using Business.Extensions;
using Core.Abstracts.UnitOfWork;
using Ganss.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace Business.Services.Implementations
{
    public class FileService
    {
        readonly IUnitOfWork _unitOfWork;
        readonly IHostEnvironment _environment;
        public FileService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task ExcelToDb(FileUploadDto file)
        {
            if (!file.File.CheckFileSize(5120))
            {
                throw new FileTypeException("File size should not exceed 5 mb");
            }
            if (!file.File.CheckFileType(".xls", ".xlxs"))
            {
                throw new FileSizeException("The file type should be xls or xlxs");
            }
            IEnumerable<ExcelDataDto> excelData =  new ExcelMapper(await FileUploadAsync(file.File)).Fetch<ExcelDataDto>();

        }
        private async Task<string> FileUploadAsync(IFormFile file)
        {
            string path = Path.Combine(_environment.ContentRootPath, "upload", file.FileName);
            using (FileStream fs = new(path, FileMode.Create))
            {
                await file.CopyToAsync(fs);
            }
            return path;
        }
    }
}
