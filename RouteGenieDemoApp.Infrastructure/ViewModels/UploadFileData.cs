using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;

namespace RouteGenieDemoApp.Infrastructure.ViewModels
{
   public class UploadFileData
    {
        public UploadFileData()
        {
           // File = new List<HttpPostedFileBase>();

        }

        [Required]
        //[Microsoft.Web.Mvc.FileExtensions(Extensions = "csv",
        //     ErrorMessage = "Specify a CSV file. (Comma-separated values)")]
        public HttpPostedFileBase File { get; set; }
        //public List <HttpPostedFileBase> File { get; set; }

    }
}
