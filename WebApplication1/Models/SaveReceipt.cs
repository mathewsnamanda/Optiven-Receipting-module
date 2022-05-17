using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class SaveReceipt
    {
        [Key]
        public int id { get; set; }
        public double Amount { get; set; }
   
        public DateTime PaymentDate { get; set; } = DateTime.Now;
    
        public string PlotNo { get; set; } = "";
  
        public string Receiptno { get; set; } = "";

        public string client { get; set; } = "";

        public string accno { get; set; } = "";

        public string item { get; set; } = "";
 
        public string paymentfor { get; set; } = "";
  
        public string project { get; set; } = "";
    
        public string receivedby { get; set; } = "";
 
        public string ReceiverEmail { get; set; } = "";

        public string Paymode { get; set; } = "";
        public string chequenumber { get; set; } = "";
        public string copy { get; set; } = "";
        public string bcopy { get; set; } = "";
    }
}
