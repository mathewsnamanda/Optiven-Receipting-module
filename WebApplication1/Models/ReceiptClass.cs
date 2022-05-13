using System;
using System.ComponentModel.DataAnnotations;

namespace ReceiptingModule.Models
{
    public class ReceiptClass
    {
        [Required]
        public double Amount { get; set; }
        [Required]
        public DateTime PaymentDate { get; set; } = DateTime.Now;
        [Required]
        public string PlotNo { get; set; } = "";
        [Required]
        public string Receiptno { get; set; } = "";
        [Required]
        public string client { get; set; } = "";
        [Required]
        public string accno { get; set; } = "";
        [Required]
        public string item { get; set; } = "";
        [Required]
        public string paymentfor { get; set; } = "";
        [Required]
        public string project { get; set; } = "";
        [Required]
        public string receivedby { get; set; } = "";
        [Required]
        public string ReceiverEmail { get; set; } = "";
        [Required]
        public string Paymode { get; set; } = "";
        public string chequenumber { get; set; } = "";
        public string copy { get; set; } = "";
        public string bcopy { get; set; } = "";
    }
}
