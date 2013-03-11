using System.Net;
using System.Net.Mail;
using SendGridMail;
using SendGridMail.Transport;


namespace LutExplorer.Helpers
{
    public static class Emailer
    {
  
        public static void sendError(int num)
        {
            string content = num.ToString();
            
            MailAddress from = new MailAddress("noreply@lutexplorer.cloudapp.net");
            MailAddress[] to = new MailAddress[]{new MailAddress("d0358412@lut.fi")};
            MailAddress[] cc;

            SendGrid msg = SendGrid.GetInstance();
            msg.From = from;
            msg.AddTo("d0358412@lut.fi");
            msg.Subject = "LUT Explorer: Missing code number: "+content;
            msg.Text = "A user has just reported QR-code number " + content + " missing. Please check this out ;) " ;
                
                //(from, to, null, null, "hi", "", "test content for test message");

            NetworkCredential cred = new NetworkCredential("azure_fcc88ca298085ddc5d1498034fdb9e46@azure.com", "g7subjxe");

            SMTP transport = SMTP.GetInstance(cred);

            transport.Deliver(msg);

            return;
        }

        

  

    }
}
