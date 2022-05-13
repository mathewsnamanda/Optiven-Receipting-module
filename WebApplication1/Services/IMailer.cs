namespace waica_V1.Services
{
    public interface IMailer
    {
        public bool mail(string username, string password, string emailsubject, string body, string fileattachmentpath, string receiveremail,string cc,string bcc);
    }
}
