#if NET8_0_OR_GREATER
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace SecureFileValidator.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ValidFileSignatureAttribute : ValidationAttribute
    {
        public string[] AllowedExtensions { get; set; } = Array.Empty<string>();

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is not IFormFile file)
                return ValidationResult.Success;

            using var stream = file.OpenReadStream();
            if (FileSignatureValidator.Validate(stream, file.FileName, AllowedExtensions))
                return ValidationResult.Success;

            return new ValidationResult(ErrorMessage ?? "檔案內容與副檔名不符");
        }
    }
}
#endif
