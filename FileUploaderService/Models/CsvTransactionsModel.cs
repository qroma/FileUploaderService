using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileUploaderService.Models
{
    public class CsvTransactionsModel
    {
        public List<TransactionCsv> TransactionsCollection {get;set;}
        
    }

    public class TransactionCsv 
    {
        public string TransactionIdentifier { get; set; }
        public double Amount { get; set; }
        public string CurrencyCode { get; set; }
        public DateTime TransactionDate { get; set; }
        public CsvStatusEnum Status { get; set; }
    }
}
