using Business.Common;
using Business.DTOs.Reports;
using Business.Enums;
using Business.Exceptions.FileExceptions;
using Business.Services.Implementations;
using Business.Validations;
using Core.Abstracts;
using Core.Abstracts.UnitOfWork;
using FluentValidation.TestHelper;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Moq;
using PYP_Pre_Assigment.Test.TestDatas;
using PYP_Pre_Assignment.API.Controllers;
using PYP_Pre_Assignment.API.Middelwares;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;
using Xunit.Sdk;

namespace PYP_Pre_Assigment.Test.ControllersTest
{
    public class SaleControllerTest
    {
        readonly SaleController _controller;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IHostEnvironment> _mockHostEnvironment;
        private readonly Mock<ISendEndpointProvider> _mockSendEndpointProvider;
        private readonly Mock<ISendEndpoint> mockSendEndpoint;
        public SaleControllerTest()
        {
            EndpointConvention.Map<TestMessage>(new Uri(new($"queue:{RabbitMqConstants.SendReportQueue}")));

            mockSendEndpoint = new Mock<ISendEndpoint>();

            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockUnitOfWork
                .Setup(m => m.ProductRepository)
                .Returns(new Mock<IProductRepository>().Object);
            _mockUnitOfWork
                .Setup(m => m.SegmentRepository)
                .Returns(new Mock<ISegmentRepository>().Object);
            _mockUnitOfWork
                .Setup(m => m.CountryRepository)
                .Returns(new Mock<ICountryRepository>().Object);

            _mockUnitOfWork
                .Setup(m => m.SaleTransactionRepository)
                .Returns(new Mock<ISaleTransactionRepository>().Object);

            _mockUnitOfWork
                .Setup(m => m.PriceRepository)
                .Returns(new Mock<IPriceRepository>().Object);

            _mockHostEnvironment = new Mock<IHostEnvironment>();
            _mockHostEnvironment
                .Setup(m => m.ContentRootPath)
                .Returns(Directory.GetCurrentDirectory);
            _mockSendEndpointProvider = new Mock<ISendEndpointProvider>();

            _mockSendEndpointProvider
                .Setup(x => x.GetSendEndpoint(It.IsAny<Uri>()))
                .Returns(Task.FromResult(mockSendEndpoint.Object));
            FileService fileService = new(_mockUnitOfWork.Object, _mockHostEnvironment.Object);
            ReportsService reportsService = new(_mockUnitOfWork.Object, _mockSendEndpointProvider.Object);
            _controller = new SaleController(fileService, reportsService);

        }

        //[Fact]
        //public async void UploadFile_ValidObjectPassed_ReturnsOkResult()
        //{
        //    var fileName = "example_data.xlsx";
        //    var currentDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        //    using FileStream fileStream = new(Path.Combine(currentDir, fileName), FileMode.Open);
        //    var file = new FormFile(fileStream, 0, fileStream.Length, null, Path.GetFileName(fileStream.Name));
        //    var result = await _controller.UploadFile(file);
        //    Assert.IsType<NoContentResult>(result);
        //}

        [Fact]
        public async Task UploadFile_InValidObjectPassed_ReturnsBadResult()
        {
            var fileName = "test.jpg";
            var currentDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            using FileStream fileStream = new(Path.Combine(currentDir, fileName), FileMode.Open);
            var file = new FormFile(fileStream, 0, fileStream.Length, null, Path.GetFileName(fileStream.Name));
            var ex = await Assert.ThrowsAsync<FileTypeException>(async () => await _controller.UploadFile(file));
            Assert.IsType<FileTypeException>(ex);
        }
        [Theory]
        [ClassData(typeof(ReportData))]
        public async Task SendReport_ValidObjectPassed_ReturnsOkResult(SendReportDto sendReport)
        {
            var result = await _controller.SendReport(sendReport);
            Assert.IsType<OkObjectResult>(result);
        }
        [Fact]
        public void SendReport_DateTime_InValidObjectPassed_ReturnBoolean()
        {
            var validator = new SendReportValidator();
            var emails = new List<EmailDto> { new EmailDto { Email = "ravan.pz@code.edu.az" } };
            var startDate = new DateTime(2015, 01, 01);
            var endDate = new DateTime(2014, 01, 01);
            var model = new SendReportDto
            {
                SendEmails = emails,
                StartDate = startDate,
                EndDate = endDate,
                Report = Report.SalesBySegment
            };
            var result = validator.TestValidate(model);
            Assert.Equal(false, result?.IsValid);
        }
        [Fact]
        public void SendReport_Mail_InValidObjectPassed_ReturnBoolean()
        {
            var validator = new SendReportValidator();
            var emails = new List<EmailDto> { new EmailDto { Email = "ravan.pz@gmail.com" } };
            var startDate = new DateTime(2014, 01, 01);
            var endDate = new DateTime(2015, 01, 01);
            var model = new SendReportDto
            {
                SendEmails = emails,
                StartDate = startDate,
                EndDate = endDate,
                Report = Report.SalesBySegment
            };
            var result = validator.TestValidate(model);
            Assert.Equal(false, result?.IsValid);
        }
        
    }
}
