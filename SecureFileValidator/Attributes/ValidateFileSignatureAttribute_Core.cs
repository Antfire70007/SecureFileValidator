#if NET8_0_OR_GREATER
using System;
using System.IO;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SecureFileValidator.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ValidateFileSignatureAttribute : Attribute, IActionFilter
    {
        public string FileParameterName { get; }

        public ValidateFileSignatureAttribute(string fileParameterName = "file")
        {
            FileParameterName = fileParameterName;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ActionArguments.TryGetValue(FileParameterName, out var fileObj) && fileObj is IFormFile formFile)
            {
                using var stream = formFile.OpenReadStream();
                if (!FileSignatureValidator.Validate(stream, formFile.FileName))
                {
                    context.Result = new BadRequestObjectResult("檔案內容不符合副檔名");
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}
#endif
