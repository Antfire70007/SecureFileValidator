#if NET8_0_OR_GREATER
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace SecureFileValidator.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Method)]
    public class ValidFileSignatureAttribute : ValidationAttribute
    {
        public string[] AllowedExtensions { get; set; } = Array.Empty<string>();
        public string? FileParameterName { get; set; }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var http = validationContext.GetService(typeof(IHttpContextAccessor)) as IHttpContextAccessor;
            if (http?.HttpContext == null)
                return ValidationResult.Success;

            var files = http.HttpContext.Request.Form.Files;

            if (!string.IsNullOrWhiteSpace(FileParameterName))
            {
                var file = files.GetFile(FileParameterName);
                if (file != null && !IsValidFile(file))
                    return new ValidationResult(ErrorMessage ?? $"檔案 {file.FileName} 驗證失敗");
            }
            else if (value is IFormFile formFile)
            {
                if (!IsValidFile(formFile))
                    return new ValidationResult(ErrorMessage ?? $"檔案 {formFile.FileName} 驗證失敗");
            }
            else
            {
                foreach (var file in files)
                {
                    if (!IsValidFile(file))
                        return new ValidationResult(ErrorMessage ?? $"檔案 {file.FileName} 驗證失敗");
                }
            }

            return ValidationResult.Success;
        }

        private bool IsValidFile(IFormFile file)
        {
            using var stream = file.OpenReadStream();
            return FileSignatureValidator.Validate(stream, file.FileName, AllowedExtensions);
        }
    }
}
#endif
