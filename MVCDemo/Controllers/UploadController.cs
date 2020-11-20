using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCDemo.Controllers
{
    [AllowAnonymous]
    public class UploadController : Controller
    {
        // GET: Upload
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase fileupload)
        {
            if (fileupload == null || fileupload.ContentLength < 1)
            {
                ViewData["error_message"] = "please select file";
                return View();
            }

            string uploadFolder = "~/Uploads/";
            string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + Path.GetFileName(fileupload.FileName);
            string path = Path.Combine(Server.MapPath(uploadFolder), fileName);
            fileupload.SaveAs(path);

            return RedirectToAction("Index");
        }
    }
}