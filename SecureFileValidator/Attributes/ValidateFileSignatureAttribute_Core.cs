#if NET8_0_OR_GREATER
using System;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SecureFileValidator.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ValidateFileSignatureAttribute : Attribute, IActionFilter
    {
        public string? FileParameterName { get; }

        public ValidateFileSignatureAttribute(string? fileParameterName = null)
        {
            FileParameterName = fileParameterName;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!string.IsNullOrEmpty(FileParameterName))
            {
                if (context.ActionArguments.TryGetValue(FileParameterName, out var fileObj) && fileObj is IFormFile formFile)
                {
                    using var stream = formFile.OpenReadStream();
                    if (!FileSignatureValidator.Validate(stream, formFile.FileName))
                        context.ModelState.AddModelError(FileParameterName, "檔案內容不符合副檔名");

                }
            }
            else
            {
                // check all files in form
                var files = context.HttpContext.Request.Form.Files;
                foreach (var formFile in files)
                {
                    using var stream = formFile.OpenReadStream();
                    if (!FileSignatureValidator.Validate(stream, formFile.FileName))
                    {
                        context.ModelState.AddModelError(formFile.FileName, "檔案內容不符合副檔名");
                        break;
                    }
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}
#endif
