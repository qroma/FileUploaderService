using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace FileUploaderService.Models
{
    [Serializable()]
    [XmlRoot("TransactionCollection")]
    public class XmlTransactionsModel
    {
        [XmlArray("Transactions")]
        [XmlArrayItem("Transaction", typeof(TransactionXml))]
        public List<TransactionXml> TransactionsCollection { get; set; }
    }

    [Serializable()]   
    public class TransactionXml
    {
        [XmlAttribute]
        public string id { get; set; }

        [XmlElement("TransactionDate")]
        [Required(ErrorMessage = "TransactionDate is required")]
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
