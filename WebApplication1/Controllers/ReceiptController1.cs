using ClientStatements.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using Syncfusion.DocIORenderer;
using Syncfusion.Pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using waica_V1.Services;
using WebApplication1.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceiptController1 : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly OptivenContext _context;
        private readonly ILogger<ReceiptController1> _logger;
        public ReceiptController1(IConfiguration Configuration, ILogger<ReceiptController1> logger, OptivenContext context)
        {
            _configuration = Configuration ?? throw new ArgumentNullException(nameof(Configuration));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        // GET: api/<ReceiptController1>
        [HttpGet]
        public IActionResult Get([FromQuery] ResendClass resendClass)
        {
            if (ModelState.IsValid)
            {
                resendClass.Receiptnumber = resendClass.Receiptnumber.Trim();

                var response = _context.Receipts.FirstOrDefault(m => m.Receiptno == resendClass.Receiptnumber);

                if(response!=null)
                {
                    string Username = this._configuration.GetConnectionString($"{response.receivedby.Trim()}_Username");
                    string Password = this._configuration.GetConnectionString($"{response.receivedby.Trim()}_Password");


                    if (string.IsNullOrEmpty(Username))
                    {
                        Username = "receivables@optiven.co.ke";
                    }
                    if (string.IsNullOrEmpty(Password))
                    {
                        Password = "hudumaforyouth@2042#!";
                    }

                    if (response.accno.Length > 4)
                        response.accno = "*" + response.accno.Substring(response.accno.Length - 4, 4);


                    Username = Username.Trim();
                    Password = Password.Trim();
                    var basePath = Path.Combine(Directory.GetCurrentDirectory(), "Template");
                    var basePath1 = Path.Combine(Directory.GetCurrentDirectory(), "Documents");
                    string path = "";
                    if (response.paymentfor.ToLower().Trim() == "deposit")
                    {
                        path = basePath + "/" + "Deposit.doc";
                    }
                    else if (response.paymentfor.ToLower().Trim() == "final")
                    {
                        path = basePath + "/" + "Final.doc";
                    }
                    else if (response.paymentfor.ToLower().Trim() != "final" && response.paymentfor.ToLower().Trim() != "deposit")
                    {
                        path = basePath + "/" + "Installment.doc";
                    }
                    if (!string.IsNullOrEmpty(path))
                    {
                        FileStream fileStreamPath = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                        WordDocument document = new WordDocument(fileStreamPath, FormatType.Docx);
                        string[] fieldNames = new string[] { "Receiptno", "PaymentDate", "client", "Paymode", "accno", "cheque", "item", "paymentfor", "project", "PlotNo", "Amount", "Receivedby" };
                        string[] fieldValues = new string[] { response.Receiptno, response.PaymentDate.ToString("MMM dd, yyyy"), response.client.ToString(), response.Paymode, response.accno, response.chequenumber, response.item, response.paymentfor, response.project, response.PlotNo, response.Amount.ToString("n"), response.receivedby };

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
                        string filename1 = response.paymentfor + "-" + response.PlotNo + "-Receipt No#" + response.Receiptno + ".pdf";
                        FileStream filep = new FileStream(basePath1 + "/" + filename1, FileMode.Create, FileAccess.Write);
                        memoryStream.WriteTo(filep);
                        filep.Close();

                        pdf.Close();
                        document.Close();
                        ms.Position = 0;
                        IMailer t = new MailClass();
                        bool result = t.mail(Username,Password,"Optiven Receipt", $"Dear {response.client},<br />&nbsp;<br /> We trust that you are well.<br />&nbsp;<br /> Kindly find attached the official receipt for payment towards your investment. Kindly confirm the receipt.<br />&nbsp;<br /> We value your great support. <br /><br /> Kind Regards, <br /> The Optiven Team.", basePath1 + "/" + filename1, response.ReceiverEmail, response.copy, response.bcopy);

                    }
                }
                return Ok("Successfully resend to client");
            }
            else
            {
                return BadRequest("Salefile id is required");
            }
           
        }

        // GET api/<ReceiptController1>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ReceiptController1>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ReceiptController1>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ReceiptController1>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
