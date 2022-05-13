using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;

namespace waica_V1.Services
{
    public class MailClass : IMailer
    {
        public bool mail(string username, string password, string emailsubject, string body, string fileattachmentpath, string receiveremail, string cc, string bcc)
        {
            Regex r = new Regex(@"\D{0,50}\W\D{0,50}\W\D{0,50}");

            String UserName = username;
            String Password = password;
            MailMessage msg = new MailMessage(UserName, receiveremail);
            msg.Subject = emailsubject;
            msg.Body = body;

            if(!string.IsNullOrEmpty(cc))
            {
                if(cc.ToLower().Contains(";"))
                {
                    foreach(var t in cc.Split(new char[] {';'}))
                    {
                        if(r.IsMatch(t))
                        {
                            msg.CC.Add(t);
                        }
                    }
                }
                else
                {
                    if (r.IsMatch(cc))
                    {
                        msg.CC.Add(cc);
                    }
                }
               
            }
            if (!string.IsNullOrEmpty(bcc))
            {
                if (bcc.ToLower().Contains(";"))
                {
                    foreach (var t in bcc.Split(new char[] { ';' }))
                    {
                        if (r.IsMatch(t))
                        {
                            msg.Bcc.Add(t);
                        }
                    }
                }
                else
                {
                    if (r.IsMatch(bcc))
                    {
                        msg.CC.Add(bcc);
                    }
                }

            }

            Attachment attach = new Attachment(fileattachmentpath);
            msg.Attachments.Add(attach);
            msg.IsBodyHtml = true;
            SmtpClient SmtpClient = new SmtpClient();
            SmtpClient.Credentials = new System.Net.NetworkCredential(UserName, Password);
            SmtpClient.Host = "smtp.gmail.com";
            SmtpClient.Port = 587;
            SmtpClient.EnableSsl = true;
            SmtpClient.Send(msg);
           
            return true;

        }
    }
}
