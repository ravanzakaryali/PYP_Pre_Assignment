using Business.Common;
using Business.DTOs.Excel;
using Business.DTOs.File;
using Business.Exceptions.FileExceptions;
using Business.Extensions;
using Business.Services.Abstracts;
using Core.Abstracts.UnitOfWork;
using Core.Entities;
using Ganss.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace Business.Services.Implementations
{
    public class FileService : IFileService
    {
        readonly IUnitOfWork _unitOfWork;
        readonly IHostEnvironment _environment;
        public FileService(IUnitOfWork unitOfWork, IHostEnvironment environment)
        {
            _unitOfWork = unitOfWork;
            _environment = environment;
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
            IEnumerable<ExcelDataDto> excelDatas = new ExcelMapper(await FileUploadAsync(file.File)).Fetch<ExcelDataDto>();
            await DatabaseSaveAsync(excelDatas);
        }
        public async Task<string> FileUploadAsync(IFormFile file)
        {
            string path = Path.Combine(_environment.ContentRootPath, "upload", file.FileName);
            using (FileStream fs = new(path, FileMode.Create))
            {
                await file.CopyToAsync(fs);
            }
            return path;
        }
        private async Task DatabaseSaveAsync(IEnumerable<ExcelDataDto> excelDatas)
        {
            List<Product> productsDb = await _unitOfWork.ProductRepository.GetAllAsync();
            List<Segment> segmentsDb = await _unitOfWork.SegmentRepository.GetAllAsync();
            List<Country> countriesDb = await _unitOfWork.CountryRepository.GetAllAsync();
            List<Product> products = new List<Product>(productsDb) ?? new List<Product>();
            List<Segment> segments = new List<Segment>(segmentsDb) ?? new List<Segment>();
            List<Country> countries = new List<Country>(countriesDb) ?? new List<Country>();
            foreach (var excel in excelDatas)
            {

                if (!segments.Exists(p => p.Name == excel.Segment))
                {
                    Segment newSegment = new()
                    {
                        Name = excel.Segment,
                    };
                    segments.Add(newSegment);
                    await _unitOfWork.SegmentRepository.AddAsync(newSegment);
                }
                if (!products.Exists(p => p.Name == excel.Product))
                {
                    Product newProduct = new()
                    {
                        Name = excel.Product,
                    };
                    products.Add(newProduct);
                    await _unitOfWork.ProductRepository.AddAsync(newProduct);
                }
                if (!countries.Exists(p => p.Name == excel.Country))
                {
                    Country newCountry = new()
                    {
                        Name = excel.Country,
                    };
                    countries.Add(newCountry);
                    await _unitOfWork.CountryRepository.AddAsync(newCountry);
                }
                Price price = new()
                {
                    ManufacturingPrice = excel.ManufacturingPrice,
                    SalePrice = excel.SalePrice,
                    Product = products.FirstOrDefault(p => p.Name == excel.Product),
                };
                await _unitOfWork.PriceRepository.AddAsync(price);
                SaleTransaction saleTransaction = new()
                {
                    UnitSold = excel.UnitSold,
                    Date = excel.Date,
                    Discounts = excel.Discounts * 100 / excel.GrossSales,
                    Sales = (excel.SalePrice * excel.UnitSold) - excel.Discounts,
                    Segment = segments.FirstOrDefault(s => s.Name == excel.Segment),
                    Country = countries.FirstOrDefault(c => c.Name == excel.Country),
                    Product = products.FirstOrDefault(p => p.Name == excel.Product),
                    Price = price,
                };
                await _unitOfWork.SaleTransactionRepository.AddAsync(saleTransaction);
            }
            await _unitOfWork.SaveAsync();
        }
        public string ObjectToExcel<T>(IEnumerable<T> @object, string fileName)
        {
            ExcelMapper mapper = new();
            var newFileName = Helper.GenerateUniqueDateName() + fileName;
            string root = Path.Combine(_environment.ContentRootPath, "sendreports", newFileName);
            mapper.Save(root, @object, "SheetName", true);
            return root;
        }
    }
}
