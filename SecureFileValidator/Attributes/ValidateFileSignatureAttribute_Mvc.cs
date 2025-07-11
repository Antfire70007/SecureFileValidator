#if NETFRAMEWORK
using System;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace SecureFileValidator.Attributes
{
    public class ValidateFileSignatureAttribute : FilterAttribute, IActionFilter
    {
        public string? FileParameterName { get; }

        public ValidateFileSignatureAttribute(string? fileParameterName = null)
        {
            FileParameterName = fileParameterName;
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!string.IsNullOrEmpty(FileParameterName))
            {
                var file = filterContext.HttpContext.Request.Files[FileParameterName];
                if (file == null)
                    return;

                using (var stream = file.InputStream)
                {
                    if (!FileSignatureValidator.Validate(stream, file.FileName))
                        filterContext.Result = new HttpStatusCodeResult(400, "檔案內容與副檔名不符");
                }
            }
            else
            {
                var files = filterContext.HttpContext.Request.Files;
                for (int i = 0; i < files.Count; i++)
                {
                    var file = files[i];
                    using (var stream = file.InputStream)
                    {
                        if (!FileSignatureValidator.Validate(stream, file.FileName))
                        {
                            filterContext.Result = new HttpStatusCodeResult(400, $"檔案 {file.FileName} 的內容與副檔名不符");
                            break;
                        }
                    }
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext filterContext) { }
    }
}
#endif
