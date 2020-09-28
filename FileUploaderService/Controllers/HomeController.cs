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
using System.Linq;

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
                    if (extension == XmlFileExtension)
                    {
                        var xml = ReadFileAsString(file);
                        var xmlModel = ParseXml<XmlTransactionsModel>(xml);
                        if (ValidModel(xmlModel))
                        {

                        }
                        else
                        {
                            return BadRequest(new { text = "Model is not valid" });
                        }
                    }
                    else if (extension == CsvFileExtension)
                    {
                        var csvModel = ParseCsv(file);
                        if (ValidModel(csvModel))
                        {

                        }
                        else
                        {
                            return BadRequest(new { text = "Model is not valid" });
                        }
                    }
                    else
                    {
                        return BadRequest(new { text = "wrong file extension" });
                    }

                }
            }
            catch (Exception e)
            {
                return BadRequest(new { text = "exception" });
            }

            return Ok(new { text = "file is uploaded and processed" });
        }

        private bool ValidModel(CsvTransactionsModel csvModel)
        {
            bool isValidModel = true;

            foreach (var item in csvModel.TransactionsCollection)
            {
                if (item.TransactionIdentifier == null || item.TransactionIdentifier == string.Empty || item.TransactionIdentifier.Length > 50)
                {
                    isValidModel = false;
                    _logger.LogError($"Validation Error for item ID - {item.TransactionIdentifier}. Field - {item.TransactionIdentifier}");
                }
                if (item.Amount == 0)
                {
                    isValidModel = false;
                    _logger.LogError($"Validation Error for item ID - {item.TransactionIdentifier}. Field - {item.Amount}");
                }
                if (!isCurrencyCode(item.CurrencyCode))
                {
                    isValidModel = false;
                    _logger.LogError($"Validation Error for item ID - {item.TransactionIdentifier}. Field - {item.CurrencyCode}");
                }
            }

            return isValidModel;
        }

        private bool isCurrencyCode(string ISOCurrencySymbol)
        {
            var symbol = CultureInfo.GetCultures(CultureTypes.AllCultures).Where(c => !c.IsNeutralCulture).Select(culture =>
            {
                try
                {
                    return new RegionInfo(culture.Name);
                }
                catch
                {
                    return null;
                }
            }).Where(ri => ri != null && ri.ISOCurrencySymbol == ISOCurrencySymbol).Select(ri => ri.CurrencySymbol).FirstOrDefault();

            return symbol != null;
        }

        private bool ValidModel(XmlTransactionsModel xmlModel)
        {
            bool isValidModel = true;

            foreach (var item in xmlModel.TransactionsCollection)
            {
                if (item.id == null || item.id.Length > 50)
                {
                    isValidModel = false;
                    _logger.LogError($"Validation Error for item ID - {item.id}. Field - {item.id}");
                }
                if (item.PaymentDetails.Amount == 0)
                {
                    isValidModel = false;
                    _logger.LogError($"Validation Error for item ID - {item.id}. Field - {item.PaymentDetails.Amount}");
                }
                if (!isCurrencyCode(item.PaymentDetails.CurrencyCode))
                {
                    isValidModel = false;
                    _logger.LogError($"Validation Error for item ID - {item.id}. Field - {item.PaymentDetails.CurrencyCode}");
                }
            }

            return isValidModel;
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
