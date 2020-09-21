using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FileUploaderService.Models;
using Microsoft.AspNetCore.Http;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using Microsoft.VisualBasic.FileIO;
using System.Collections.Generic;
using System.Globalization;

namespace FileUploaderService.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private const string XmlFileExtension = ".xml";
        private const string CsvFileExtension = ".csv";

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
                    var extension = Path.GetExtension(file.FileName).ToLower();
                    if (extension == XmlFileExtension) {
                        var xml = ReadFileAsString(file);
                        var xmlModel = ParseXml<XmlTransactionsModel>(xml);
                    }
                    else if (extension == CsvFileExtension) {
                        //var csv = ReadFileAsString(file);
                        var csvModel = ParseCsv(file);
                    }
                    else {
                        return BadRequest(new { text = "wrong file extension" });
                    }
                    
                }
            }
            catch (Exception e) {
                return BadRequest(new { text = "exception"});
            }
            
            return Ok(new { text = "file is uploaded and processed" });
        }

        private string ReadFileAsString(IFormFile file)
        {
            using var reader = new StreamReader(file.OpenReadStream());
            return reader.ReadToEnd();
        }

        private CsvTransactionsModel ParseCsv(IFormFile file) 
        {
            CsvTransactionsModel csvModel = new CsvTransactionsModel();

            try
            {
                using TextFieldParser parser = new TextFieldParser(file.OpenReadStream());
                csvModel.TransactionsCollection = new List<TransactionCsv>();

                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                while (!parser.EndOfData)
                {
                    //process row
                    string[] fields = parser.ReadFields();
                    csvModel.TransactionsCollection.Add(new TransactionCsv()
                    {
                        TransactionIdentifier = fields[0],
                        Amount = ToInt(fields[1]),
                        CurrencyCode = fields[2],
                        TransactionDate = Convert.ToDateTime(fields[3]),
                        Status = (CsvStatusEnum)Enum.Parse(typeof(CsvStatusEnum), fields[4])
                    });                  
                }
            }
            catch (Exception e)
            {
                //log
            }
            return csvModel;
        }

        private int ToInt(string num) 
        {
            num = num.Replace(",", "");
            int index = num.IndexOf('.');
            num = num.Remove(index);
            return int.Parse(num);
        }

        private T ParseXml<T>(string xml)
        {
            T xmlModel = default(T);

            try
            {
                using TextReader reader = new StringReader(xml);
                xmlModel = (T)new XmlSerializer(typeof(T)).Deserialize(reader);
            }
            catch (Exception e)
            {
                //log             
            }
            return xmlModel;
        }

        public IActionResult Privacy()
        {
            return View();
        }                

    }
}
