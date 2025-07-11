using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using FluentAssertions;
using SecureFileValidator.Attributes;
using System.Collections.Generic;

namespace SecureFileValidator.Tests
{
    public class ValidateFileSignatureAttribute_CoreTests
    {
        [Fact]
        public void OnActionExecuting_Should_Block_When_Any_File_Invalid()
        {
            // Arrange
            var formFileMock1 = new Mock<IFormFile>();
            var formFileMock2 = new Mock<IFormFile>();

            formFileMock1.Setup(f => f.FileName).Returns("test1.docx");
            formFileMock1.Setup(f => f.OpenReadStream()).Returns(new MemoryStream(new byte[] { 0x50, 0x4B, 0x03, 0x04 })); // valid

            formFileMock2.Setup(f => f.FileName).Returns("test2.mp4");
            formFileMock2.Setup(f => f.OpenReadStream()).Returns(new MemoryStream(new byte[] { 0x00, 0x00, 0x00, 0x00 })); // invalid

            var formFileCollection = new FormFileCollection
            {
                formFileMock1.Object,
                formFileMock2.Object
            };

            var formCollection = new FormCollection(new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>(), formFileCollection);
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Form = formCollection;

            var context = new ActionExecutingContext(
                new ActionContext(httpContext, new Microsoft.AspNetCore.Routing.RouteData(), new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor()),
                new List<IFilterMetadata>(),
                new Dictionary<string, object>(),
                controller: null
            );

            var attr = new ValidateFileSignatureAttribute(); // no parameter name => check all

            // Act
            attr.OnActionExecuting(context);

            // Assert
            context.Result.Should().BeOfType<BadRequestObjectResult>();
        }
    }
}
