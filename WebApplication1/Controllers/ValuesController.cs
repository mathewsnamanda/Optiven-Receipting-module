using ClientStatements.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ReceiptingModule.Models;
using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using Syncfusion.DocIORenderer;
using Syncfusion.Pdf;
using System;
using System.IO;
using waica_V1.Services;
using WebApplication1.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ReceiptingModule.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly OptivenContext _context;
        private readonly ILogger<ValuesController> _logger;

        public ValuesController(IConfiguration Configuration, ILogger<ValuesController> logger, OptivenContext context)
        {
            _configuration = Configuration ?? throw new ArgumentNullException(nameof(Configuration));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        // GET: api/<ValuesController>
        [HttpGet]
        public IActionResult Get([FromQuery]ReceiptClass receiptClass)
        {
            receiptClass.receivedby = receiptClass.receivedby.Split(" ")[receiptClass.receivedby.Split("").Length - 1].Trim();
          if (!string.IsNullOrEmpty(receiptClass.PlotNo))
            {
                if (receiptClass.PlotNo.Contains("-"))
                    receiptClass.PlotNo = receiptClass.PlotNo.Split("-")[1].Trim();
            }
           if(!string.IsNullOrEmpty(receiptClass.accno))
            {
                if (receiptClass.accno.Length > 4)
                    receiptClass.accno = "*" + receiptClass.accno.Substring(receiptClass.accno.Length - 4, 4);
            }
            if(!string.IsNullOrEmpty(receiptClass.chequenumber))
            {
                if (receiptClass.chequenumber.Length > 4)
                    receiptClass.chequenumber = "*" + receiptClass.chequenumber.Substring(receiptClass.chequenumber.Length - 4, 4);

            }
            if (ModelState.IsValid)
            {
                string Username = this._configuration.GetConnectionString($"{receiptClass.receivedby.Trim()}_Username");
                string Password = this._configuration.GetConnectionString($"{receiptClass.receivedby.Trim()}_Password");
               

                if (string.IsNullOrEmpty(Username))
                {
                    Username = "receivables@optiven.co.ke";
                }
                if (string.IsNullOrEmpty(Password))
                {
                    Password = "hudumaforyouth@2042#!";
                }
                Username = Username.Trim();
                Password = Password.Trim();
                var basePath = Path.Combine(Directory.GetCurrentDirectory(), "Template");
                var basePath1 = Path.Combine(Directory.GetCurrentDirectory(), "Documents");
                string path = "";
                if (receiptClass.paymentfor.ToLower().Trim() == "deposit")
                {
                    path = basePath + "/" + "Deposit.doc";
                }
                else if (receiptClass.paymentfor.ToLower().Trim().Contains("final"))
                {
                    path = basePath + "/" + "Final.doc";
                }
                else if(!receiptClass.paymentfor.ToLower().Trim().Contains("final") && !receiptClass.paymentfor.ToLower().Trim().Contains("deposit"))
                {
                    path = basePath + "/" + "Installment.doc";
                }
                if (!string.IsNullOrEmpty(path))
                {
                    FileStream fileStreamPath = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    WordDocument document = new WordDocument(fileStreamPath, FormatType.Docx);
                    string[] fieldNames = new string[] { "Receiptno", "PaymentDate", "client", "Paymode", "accno", "cheque", "item", "paymentfor", "project", "PlotNo", "Amount", "Receivedby" };
                    string[] fieldValues = new string[] { receiptClass.Receiptno, receiptClass.PaymentDate.ToString("MMM dd, yyyy"), receiptClass.client.ToString(),receiptClass.Paymode, receiptClass.accno, receiptClass.chequenumber, receiptClass.item,receiptClass.paymentfor,receiptClass.project,receiptClass.PlotNo,receiptClass.Amount.ToString("n"),receiptClass.receivedby };

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
                    SaveReceipt saveReceipt = new SaveReceipt();
                    saveReceipt.client = receiptClass.client;
                    saveReceipt.paymentfor = receiptClass.paymentfor;
                    saveReceipt.copy = receiptClass.copy;
                    saveReceipt.chequenumber = receiptClass.chequenumber;
                    saveReceipt.accno = receiptClass.accno;
                    saveReceipt.Receiptno = receiptClass.Receiptno;
                    saveReceipt.ReceiverEmail = receiptClass.ReceiverEmail;
                    saveReceipt.receivedby = receiptClass.receivedby;
                    saveReceipt.project = receiptClass.project;
                    saveReceipt.PlotNo = receiptClass.PlotNo;
                    saveReceipt.Paymode = receiptClass.Paymode;
                    saveReceipt.PaymentDate = receiptClass.PaymentDate;
                    saveReceipt.item = receiptClass.item;
                    saveReceipt.bcopy = receiptClass.bcopy;
                    saveReceipt.Amount=receiptClass.Amount;
                   
                    
                    _context.Receipts.Add(saveReceipt);
                    _context.SaveChanges();
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
