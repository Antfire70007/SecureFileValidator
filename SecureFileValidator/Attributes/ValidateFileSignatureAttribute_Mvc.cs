#if NETFRAMEWORK
using System;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace SecureFileValidator.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ValidateFileSignatureAttribute : ActionFilterAttribute
    {
        public string FileParameterName { get; set; }

        public string[] AllowedExtensions { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var request = filterContext.HttpContext.Request;

            if (!string.IsNullOrEmpty(FileParameterName))
            {
                var file = request.Files[FileParameterName];
                if (file != null && !IsValidFile(file))
                {
                    filterContext.Controller.ViewData.ModelState.AddModelError(FileParameterName, "檔案內容不符合副檔名");
                }
            }
            else
            {
                foreach (string key in request.Files)
                {
                    var file = request.Files[key];
                    if (file != null && !IsValidFile(file))
                    {
                        filterContext.Controller.ViewData.ModelState.AddModelError(key, "檔案內容不符合副檔名");
                        break;
                    }
                }
            }
        }

        private bool IsValidFile(HttpPostedFileBase file)
        {
            using (var stream = file.InputStream)
            {
                return FileSignatureValidator.Validate(stream, file.FileName, AllowedExtensions);
            }
        }
    }
}
#endif
