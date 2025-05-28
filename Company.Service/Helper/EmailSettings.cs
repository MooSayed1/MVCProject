using System.Net.Mail;

namespace Company.Service.Helper;

public class EmailSettings
{
    public static void SendEmail(Email email)
    {
        var client = new SmtpClient("smtp.gmail.com", 587);
        client.Credentials = new System.Net.NetworkCredential("mohamed1212213183@gmail.com", "mzlzqzdeofmxoyjh");
        
        client.EnableSsl = true;
        client.Send("mohamed1212213183@gmail.com",email.To,email.Subject,email.Body);
    }
}