using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;
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
            string fromdisplayname = "";
            var basePath = Path.Combine(Directory.GetCurrentDirectory(), "Template");
            string path = basePath + "/" + "marketers.csv";

            foreach (string line in System.IO.File.ReadLines(path))
            {
                if (line.ToLower().Contains(","+UserName.ToLower().Trim()))
                {
                    fromdisplayname = line.Split(",")[0].Trim();
                }

            }


            MailAddress from = new MailAddress(username, fromdisplayname);
            MailAddress to = new MailAddress(receiveremail, "");
            MailMessage msg = new MailMessage(from, to);
            msg.Subject = emailsubject;
            msg.Body = body;
            msg.Priority.HasFlag(MailPriority.High);

            if(!string.IsNullOrEmpty(cc))
            {
                if(cc.ToLower().Contains(";"))
                {
                    foreach(var t in cc.Split(new char[] {';'}))
                    {
                        if(r.IsMatch(t))
                        {
                            string bccname = "";
                            foreach (string line in System.IO.File.ReadLines(path))
                            {
                                if (line.ToLower().Contains("," + t.ToLower().Trim()))
                                {
                                    bccname = line.Split(",")[0].Trim();
                                }

                            }
                            MailAddress bccc = new MailAddress(t, bccname);
                            msg.CC.Add(bccc);
                        }
                    }
                }
                else
                {
                    if (r.IsMatch(cc))
                    {
                        string bccname = "";
                        foreach (string line in System.IO.File.ReadLines(path))
                        {
                            if (line.ToLower().Contains(("," + cc.Trim().ToLower().Trim())))
                            {
                                bccname = line.Split(",")[0].Trim();
                            }

                        }
                        MailAddress bccc = new MailAddress(cc, bccname);
                        msg.CC.Add(bccc);
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

            string file = fileattachmentpath;

            Attachment data = new Attachment(file, MediaTypeNames.Application.Octet);
            // Add time stamp information for the file.
            ContentDisposition disposition = data.ContentDisposition;
            disposition.CreationDate = System.IO.File.GetCreationTime(file);
            disposition.ModificationDate = System.IO.File.GetLastWriteTime(file);
            disposition.ReadDate = System.IO.File.GetLastAccessTime(file);
            // Add the file attachment to this email message.
            msg.Attachments.Add(data);

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
