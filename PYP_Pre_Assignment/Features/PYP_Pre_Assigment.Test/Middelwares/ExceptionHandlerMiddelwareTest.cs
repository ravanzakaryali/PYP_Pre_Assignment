using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using PYP_Pre_Assignment.API.Middelwares;
using System.Threading.Tasks;
using Xunit;

namespace PYP_Pre_Assigment.Test.Middelwares
{
    public class ExceptionHandlerMiddelwareTest
    {
        readonly HttpContext _context;
        public ExceptionHandlerMiddelwareTest()
        {
            _context = new DefaultHttpContext();
        }
        [Fact]
        public async Task Middleware_should_add_header()
        {
            RequestDelegate next = (HttpContext hc) => Task.CompletedTask;
            ExceptionHandlerMiddelware mw = new(next, new Mock<ILogger<ExceptionHandlerMiddelware>>().Object);
            await _context.Response.StartAsync(default);
            await mw.InvokeAsync(_context);
            Assert.NotNull(next);
        }
    }
}
