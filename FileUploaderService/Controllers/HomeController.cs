using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FileUploaderService.Models;
using Microsoft.AspNetCore.Http;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace FileUploaderService.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFileCollection files)
        {
            try 
            {
                foreach (IFormFile file in files)
                {
                    var xml = ReadAsString(file);
                    var xmlModel = ParseXml<Transactions>(xml);
                }
            }
            catch (Exception e) {
                return BadRequest();
            }
            
            return Ok(new { text = "file is uploaded and processed" });
        }

        private string ReadAsString(IFormFile file)
        {
            var result = new StringBuilder();
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                while (reader.Peek() >= 0)
                    result.AppendLine(reader.ReadLine());
            }
            return result.ToString().Trim();
        }        

        protected T ParseXml<T>(String xml)
        {
            T returnedXmlClass = default(T);

            try
            {
                using (TextReader reader = new StringReader(xml))
                {                   
                        returnedXmlClass = (T)new XmlSerializer(typeof(T)).Deserialize(reader);                                    
                }
            }
            catch (Exception e)
            {

            }
            return returnedXmlClass;
        }

        public IActionResult Privacy()
        {
            return View();
        }                

    }
}
