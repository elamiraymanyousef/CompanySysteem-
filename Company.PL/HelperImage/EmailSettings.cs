using System.Net.Mail;
using System.Net;

namespace Company.PL.HelperImage
{
    public  static class EmailSettings
    {
        public static bool SendEmail(Email email)
        {
			try
			{
                //cdqionvvnjoaubsw
                // Mail Server  :Gmail OR Outlock

                //SMTP
                var client = new SmtpClient("smtp.gmail.com", 587);
                client.EnableSsl = true; //  تشفير البيانات  
                client.Credentials = new NetworkCredential("alamyraymnalhmd@gmail.com", "zkgyohffpbsinsky");// sender 
                client.Send("alamyraymnalhmd@gmail", email.To, email.Subject, email.Body); //
                return true;
                
                
                
               


            }
            catch (Exception e)
			{

				return false;
			}
        }
    }
}
