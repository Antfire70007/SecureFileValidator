using System;
using System.IO;
using System.Linq;
using Xunit;
using FluentAssertions;

namespace SecureFileValidator.Tests
{
    public class FileSignatureValidator_ExtendedTests
    {
        [Fact]
        public void Validate_Should_Return_False_When_Extension_Not_Allowed()
        {
            // Arrange
            var dummyData = new byte[] { 0x50, 0x4B, 0x03, 0x04 }; // ZIP signature
            using var stream = new MemoryStream(dummyData);
            var fileName = "sample.docx";
            var allowedExtensions = new[] { ".xlsx" }; // intentionally mismatched

            // Act
            var result = FileSignatureValidator.Validate(stream, fileName, allowedExtensions);

            // Assert
            result.Should().BeFalse("副檔名雖正確，但不在允許清單中");
        }

        [Fact]
        public void Validate_Should_Return_True_When_Extension_Allowed_And_Signature_Matches()
        {
            // Arrange
            var dummyData = new byte[] { 0x50, 0x4B, 0x03, 0x04 }; // ZIP signature
            using var stream = new MemoryStream(dummyData);
            var fileName = "sample.xlsx";
            var allowedExtensions = new[] { ".xlsx", ".docx" };

            // Act
            var result = FileSignatureValidator.Validate(stream, fileName, allowedExtensions);

            // Assert
            result.Should().BeTrue("副檔名符合且簽名正確");
        }

        [Fact]
        public void Validate_Should_Return_False_When_Signature_Is_Invalid()
        {
            // Arrange
            var dummyData = new byte[] { 0x00, 0x00, 0x00, 0x00 }; // Invalid signature
            using var stream = new MemoryStream(dummyData);
            var fileName = "sample.docx";
            var allowedExtensions = new[] { ".docx" };

            // Act
            var result = FileSignatureValidator.Validate(stream, fileName, allowedExtensions);

            // Assert
            result.Should().BeFalse("雖然副檔名正確，但內容不符合格式");
        }
    }
}
