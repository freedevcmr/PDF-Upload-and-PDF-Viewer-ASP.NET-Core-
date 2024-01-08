using Microsoft.AspNetCore.Mvc;
using PdfViewer.Models;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace PdfViewer.Controllers
{
    public class Files1Controller : Controller
    {

        IHostingEnvironment _hostingEnvironment = null;

        public Files1Controller(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public IActionResult Index(string fileName="")
        {
            FileClass fileObj = new FileClass();
            fileObj.Name = fileName;

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
            string path = _hostingEnvironment.WebRootPath + "\\ files \\" + fileName;
            return File(System.IO.File.ReadAllBytes(path),"application/pdf");
        }



    }

}
