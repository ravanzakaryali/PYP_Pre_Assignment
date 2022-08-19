using Business.DTOs.File;
using Business.Extensions;
using Core.Abstracts.UnitOfWork;
using Microsoft.AspNetCore.Http;

namespace Business.Services.Implementations
{
    public class FileService
    {
        readonly IUnitOfWork _unitOfWork;
        public FileService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        //public Task FileUploadToDb(FileUploadDto file)
        //{
        //}
    }
}
