using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ReceiptingModule.Models;
using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using Syncfusion.DocIORenderer;
using Syncfusion.Pdf;
using System;
using System.IO;
using waica_V1.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ReceiptingModule.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ValuesController(IConfiguration Configuration)
        {
            _configuration = Configuration ?? throw new ArgumentNullException(nameof(Configuration));
        }
        // GET: api/<ValuesController>
        [HttpGet]
        public IActionResult Get([FromQuery]ReceiptClass receiptClass)
        {
            receiptClass.receivedby = receiptClass.receivedby.Split(" ")[receiptClass.receivedby.Split("").Length - 1].Trim();
            if(receiptClass.PlotNo.Contains("-"))
            receiptClass.PlotNo=receiptClass.PlotNo.Split("-")[1].Trim();
            if (receiptClass.accno.Length>3)
                receiptClass.accno = "*****" + receiptClass.accno.Substring(receiptClass.accno.Length-3,3);
            if (receiptClass.chequenumber.Length > 3)
                receiptClass.chequenumber = "*****" + receiptClass.chequenumber.Substring(receiptClass.chequenumber.Length - 3, 3);
            if (ModelState.IsValid)
            {
                string Username = this._configuration.GetConnectionString("Username");
                string Password = this._configuration.GetConnectionString("Password");
                
                var basePath = Path.Combine(Directory.GetCurrentDirectory(), "Template");
                var basePath1 = Path.Combine(Directory.GetCurrentDirectory(), "Documents");
                string path = "";
                if (receiptClass.paymentfor.ToLower().Trim() == "deposit")
                {
                    path = basePath + "/" + "Deposit.doc";
                }
                else if (receiptClass.paymentfor.ToLower().Trim() == "final")
                {
                    path = basePath + "/" + "Final.doc";
                }
                else if(receiptClass.paymentfor.ToLower().Trim() != "final"&& receiptClass.paymentfor.ToLower().Trim() != "deposit")
                {
                    path = basePath + "/" + "Installment.doc";
                }
                if (!string.IsNullOrEmpty(path))
                {
                    FileStream fileStreamPath = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    WordDocument document = new WordDocument(fileStreamPath, FormatType.Docx);
                    string[] fieldNames = new string[] { "Receiptno", "PaymentDate", "client", "Paymode", "accno", "cheque", "item", "paymentfor", "project", "PlotNo", "Amount", "Receivedby" };
                    string[] fieldValues = new string[] { receiptClass.Receiptno, receiptClass.PaymentDate.ToString("MMM dd, yyyy"), receiptClass.client,receiptClass.Paymode, receiptClass.accno, receiptClass.chequenumber, receiptClass.item,receiptClass.paymentfor,receiptClass.project,receiptClass.PlotNo,receiptClass.Amount.ToString("n"),receiptClass.receivedby };

                    document.MailMerge.Execute(fieldNames, fieldValues);

                    MemoryStream ms = new MemoryStream();
                    FormatType type = FormatType.Docx;
                    document.Save(ms, type);

                    DocIORenderer render = new DocIORenderer();
                    PdfDocument pdf = render.ConvertToPDF(document);
                    PdfPage page = pdf.Pages[pdf.PageCount - 1];
                  
                    MemoryStream memoryStream = new MemoryStream();
                    // Save the PDF document.
                    pdf.Save(memoryStream);
                    render.Dispose();
                    string filename1 = receiptClass.paymentfor + "-" + receiptClass.PlotNo + "-Receipt No#"+receiptClass.Receiptno + ".pdf";
                    FileStream filep = new FileStream(basePath1 + "/" + filename1, FileMode.Create, FileAccess.Write);
                    memoryStream.WriteTo(filep);
                    filep.Close();

                    pdf.Close();
                    document.Close();
                    ms.Position = 0;
                    IMailer t = new MailClass();
                    bool result = t.mail(Username,Password,"Optiven Receipt", $"Dear {receiptClass.client},<br />&nbsp;<br /> We trust that you are well.<br />&nbsp;<br /> Kindly find attached the official receipt for payment towards your investment. Kindly confirm the receipt.<br />&nbsp;<br /> We value your great support. <br /><br /> Kind Regards, <br /> The Optiven Team.", basePath1 + "/" + filename1, receiptClass.ReceiverEmail, receiptClass.copy, receiptClass.bcopy);

                }
            }
                return Ok("Done");
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ValuesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
