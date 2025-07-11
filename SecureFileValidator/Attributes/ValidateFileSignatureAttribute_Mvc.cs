#if NETFRAMEWORK
using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
namespace SecureFileValidator.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ValidateFileSignatureAttribute : FilterAttribute, IActionFilter
    {
        public string FileParameterName { get; }

        public ValidateFileSignatureAttribute(string fileParameterName = "file")
        {
            FileParameterName = fileParameterName;
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var file = filterContext.HttpContext.Request.Files[FileParameterName];
            if (file != null)
            {
                using (var stream = file.InputStream)
                {
                    if (!FileSignatureValidator.Validate(stream, file.FileName))
                    {
                        filterContext.Result = new HttpStatusCodeResult(400, "檔案內容不符合副檔名");
                    }
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext filterContext) { }
    }
}
#endif
