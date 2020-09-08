using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FileUploaderService.Models
{
    [Serializable()]
    //[XmlRoot("Transactions")]
    [XmlRoot("TransactionCollection")]
    public class Transactions
    {
        //[XmlArray("Cars")]
        //[XmlAttribute("Transaction", typeof(Transaction))]
        [XmlArray("Transactions")]
        [XmlArrayItem("Transaction", typeof(Transaction))]
        public Transaction[] TransactionsCollection { get; set; }
    }

    [Serializable()]
    public class Transaction
    {       
        [XmlElement("TransactionDate")]
        [Required(ErrorMessage = "TransactionDate is required")]
        //[DataType(DataType.DateTime)]
        public DateTime TransactionDate { get; set; }

        [XmlElement("Status")]
        [Required(ErrorMessage = "Status is required")]
        public XmlStatusEnum Status { get; set; }

        [XmlElement("PaymentDetails")]
        public PaymentDetails PaymentDetails { get; set; }
    }

    [Serializable()]
    public class PaymentDetails
    {       
        [XmlElement("Amount")]
        [Required(ErrorMessage = "Amount is required")]
        public double Amount { get; set; }

        [XmlElement("CurrencyCode")]
        [Required(ErrorMessage = "CurrencyCode is required")]
        [StringLength(3)]
        public string CurrencyCode { get; set; }       
    }
}
