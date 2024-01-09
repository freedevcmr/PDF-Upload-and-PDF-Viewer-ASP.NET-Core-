using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using PdfViewer.Models;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace PdfViewer.Controllers
{
    public class Files1Controller : Controller
    {

        IHostingEnvironment _hostingEnvironment ;

        public Files1Controller(IHostingEnvironment hostingEnvironment)
        {
            this._hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public IActionResult Index(string fileName="",bool isDelete = false)
        {
            FileClass fileObj = new FileClass();
            fileObj.Name = fileName;
           
            if (isDelete)
            {
                string filePath = $"{_hostingEnvironment.WebRootPath}\\files\\{fileObj.Name}";

                
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            //pdf will upload in this folder
            string path = $"{_hostingEnvironment.WebRootPath}\\files\\";

            int nId = 1;

            foreach(string pdfPath in Directory.EnumerateFiles(path, "*.pdf"))
            {
                fileObj.Files.Add(new FileClass()
                {
                    FileId = nId++,
                    Name = Path.GetFileName(pdfPath),
                    Path = pdfPath

                });
            }

            return View(fileObj);
        }


        [HttpPost]
        public IActionResult Index(IFormFile file, [FromServices] IHostingEnvironment hostingEnvironment )
        {         
            string fileName = $"{hostingEnvironment.WebRootPath}\\files\\{file.FileName}";
            using (FileStream fileStream = System.IO.File.Create(fileName))
            {
                file.CopyTo(fileStream);
                fileStream.Flush();
            }
            return Index();
        }
        
        public IActionResult PDFViewerNewTab(string fileName)
        {
            string path = $"{_hostingEnvironment.WebRootPath}\\files\\{fileName}";
           
            return File(System.IO.File.ReadAllBytes(path),"application/pdf");
        }



    }

}
