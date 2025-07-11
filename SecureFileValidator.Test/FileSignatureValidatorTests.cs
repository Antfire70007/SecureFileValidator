using System.IO;
using System.Text;
using Xunit;
using FluentAssertions;
using System;

namespace SecureFileValidator.Tests
{
    public class FileSignatureValidatorTests
    {
        [Theory]
        [InlineData("FFD8FFE000104A464946", ".jpg", true)] // JPEG
        [InlineData("89504E470D0A1A0A", ".png", true)]     // PNG
        [InlineData("255044462D312E", ".pdf", true)]        // PDF
        [InlineData("504B030414000600", ".docx", true)]    // zip but no word/ folder
        [InlineData("1234567890ABCDEF", ".exe", false)]     // fake
        public void Validate_FileSignature_ChecksCorrectly(string hexString, string extension, bool expected)
        {
            byte[] bytes = ConvertHexStringToByteArray(hexString);
            using var stream = new MemoryStream(bytes);

            bool result = FileSignatureValidator.Validate(stream, "test" + extension);
            result.Should().Be(expected);
        }

        private static byte[] ConvertHexStringToByteArray(string hex)
        {
            byte[] bytes = new byte[hex.Length / 2];
            for (int i = 0; i < bytes.Length; i++)
                bytes[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            return bytes;
        }
    }
}
