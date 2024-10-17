using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
namespace OCMDomain.Repository.Edmx
{
    public class EmailService
    {
        public bool SendWelcomeEmail(string toEmail, string name, string Attorney, string url = "")
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("mail.ab-sol.net");
                mail.From = new MailAddress("eService@ab-sol.net");
                mail.To.Add(toEmail);
                mail.Subject = "Welcome to OCM";
                mail.Body = ConvertHtmlToStringWelcome(name, Attorney, url);
                mail.IsBodyHtml = true;
                SmtpServer.Port = 587;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new System.Net.NetworkCredential("eService@ab-sol.net", "P@kistan1947");
                SmtpServer.EnableSsl = false;
                SmtpServer.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        private string ConvertHtmlToStringWelcome(string name, string Attorney, string url)
        {
            url = "http://localhost:25149" + url;
            string html = string.Empty;
            html += "<style>body { margin-top: 20px;} </style>";
            html += "<table class='body-wrap' style='width:100%;'><tbody><tr><td></td><td>";
            html += "<div style='box-sizing:border-box;max-width:600px;margin:0 auto;padding:20px;'><table><tbody><tr>";
            html += "<td style='padding:30px;border:3px solid #67A8E4;border-radius:7px;'><meta><table><tbody><tr>";
            html += "<td style='font-family:Helvetica Neue,Helvetica,Arial,sans-serif;font-size:14px;padding: 0 0 20px;'>";
            html += $"Hi {name}. Get Started with enrolled course and learn today!</td></tr><tr>";
            html += "<td style='font-family:Helvetica Neue,Helvetica,Arial,sans-serif;font-size:14px;padding: 0 0 20px;'>";
            html += "If you want Sign in please click the button below to complete your registration process.";
            html += "</td></tr><tr><td style ='padding: 0 0 20px;'><a href=";
            html += $"{url} class='btn-primary' itemprop='url' style='font-family:Helvetica Neue,Helvetica,Arial,sans-serif;font-size: 14px; color: #FFF; text-decoration: none; line-height: 2em; font-weight: bold;cursor: pointer; display: inline-block; border-radius: 5px;background-color: #F06292;border-color: #F06292; border-style: solid; border-width: 8px 16px;'>";
            html += "Upload Challan</a></td></tr><tr>";
            html += "<td class='content-block' style='font-family:Helvetica Neue,Helvetica,Arial,sans-serif;font-size:14px;padding: 0 0 20px;'>";
            html += "<p>Thank you for enrollment in course at OCM by Multronica Solutions, you need to login and upload the submited fee challan for access to your registred class.</p></td></tr><tr>";
            html += "<td style='text-align:center;font-family:Helvetica Neue,Helvetica,Arial,sans-serif;font-size:14px;'>&copy; Multronica Solutions " + DateTime.Now.Year;
            html += "</td></tr></tbody></table></td></tr></tbody></table></div></td></tr></tbody></table>";
            return html;
        }
        public bool SendEmailPasswordReset(string userEmail, string link)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("eService@ab-sol.net");
            mailMessage.To.Add(new MailAddress(userEmail));
            mailMessage.Subject = "Password Reset";
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = link;
            SmtpClient client = new SmtpClient("mail.ab-sol.net");
            client.Credentials = new System.Net.NetworkCredential("eService@ab-sol.net", "P@kistan1947");
            client.Port = 587;
            client.UseDefaultCredentials = false;
            client.EnableSsl = false;
            try
            {
                client.Send(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                // log exception
            }
            return false;
        }
        public bool SendApproveMail(string toEmail, string name, string Attorney, string url = "")
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("mail.ab-sol.net");
                mail.From = new MailAddress("eService@ab-sol.net");
                mail.To.Add(toEmail);
                mail.Subject = "Welcome to OCM";
                mail.Body = ConvertHtmlToStringApprove(name, Attorney, url);
                mail.IsBodyHtml = true;
                SmtpServer.Port = 587;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new System.Net.NetworkCredential("eService@ab-sol.net", "P@kistan1947");
                SmtpServer.EnableSsl = false;
                SmtpServer.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        private string ConvertHtmlToStringApprove(string name, string Attorney, string url)
        {
            string html = string.Empty;
            html += "<style>body { margin-top: 20px;} </style>";
            html += "<table class='body-wrap' style='width:100%;'><tbody><tr><td></td><td>";
            html += "<div style='box-sizing:border-box;max-width:600px;margin:0 auto;padding:20px;'><table><tbody><tr>";
            html += "<td style='padding:30px;border:3px solid #67A8E4;border-radius:7px;'><meta><table><tbody><tr>";
            html += "<td style='font-family:Helvetica Neue,Helvetica,Arial,sans-serif;font-size:14px;padding: 0 0 20px;'>";
            html += $"Hi {name}. Welcome to Multronica solutions</td></tr><tr>";
            html += "<td style='font-family:Helvetica Neue,Helvetica,Arial,sans-serif;font-size:14px;padding: 0 0 20px;'>";
            html += "Your Employee account has generated on OCM, Use you email as username and Password. To directly login by clicking the button below.";
            html += "</td></tr><tr><td style ='padding: 0 0 20px;'><a href=";
            html += $"{url} class='btn-primary' itemprop='url' style='font-family:Helvetica Neue,Helvetica,Arial,sans-serif;font-size: 14px; color: #FFF; text-decoration: none; line-height: 2em; font-weight: bold;cursor: pointer; display: inline-block; border-radius: 5px;background-color: #F06292;border-color: #F06292; border-style: solid; border-width: 8px 16px;'>";
            html += "Login</a></td></tr><tr>";
            html += "<td class='content-block' style='font-family:Helvetica Neue,Helvetica,Arial,sans-serif;font-size:14px;padding: 0 0 20px;'>";
            html += "<p>Congratulation for being the part of Multronica Solutions.</p></td></tr><tr>";
            html += "<td style='text-align:center;font-family:Helvetica Neue,Helvetica,Arial,sans-serif;font-size:14px;'>&copy; Multronica Solutions " + DateTime.Now.Year;
            html += "</td></tr></tbody></table></td></tr></tbody></table></div></td></tr></tbody></table>";
            return html;
        }
        public bool SendRejectMail(string toEmail, string name, string Attorney, string url = "")
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("mail.ab-sol.net");
                mail.From = new MailAddress("eService@ab-sol.net");
                mail.To.Add(toEmail);
                mail.Subject = "Welcome to OCM";
                mail.Body = ConvertHtmlToStringReject(name, Attorney, url);
                mail.IsBodyHtml = true;
                SmtpServer.Port = 587;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new System.Net.NetworkCredential("eService@ab-sol.net", "P@kistan1947");
                SmtpServer.EnableSsl = false;
                SmtpServer.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        private string ConvertHtmlToStringReject(string name, string Attorney, string url)
        {
            string html = string.Empty;
            html += "<style>body { margin-top: 20px;} </style>";
            html += "<table class='body-wrap' style='width:100%;'><tbody><tr><td></td><td>";
            html += "<div style='box-sizing:border-box;max-width:600px;margin:0 auto;padding:20px;'><table><tbody><tr>";
            html += "<td style='padding:30px;border:3px solid #67A8E4;border-radius:7px;'><meta><table><tbody><tr>";
            html += "<td style='font-family:Helvetica Neue,Helvetica,Arial,sans-serif;font-size:14px;padding: 0 0 20px;'>";
            html += $"Hi {name}. Your application has been rejected, you are not qualified for current positions, we wish you best of luck for future.</td></tr><tr>";
            html += "<td class='content-block' style='font-family:Helvetica Neue,Helvetica,Arial,sans-serif;font-size:14px;padding: 0 0 20px;'>";
            html += "<p>If any query, please visit the campus.</p></td></tr><tr>";
            html += "<td style='text-align:center;font-family:Helvetica Neue,Helvetica,Arial,sans-serif;font-size:14px;'>&copy; Multronica Solutions " + DateTime.Now.Year;
            html += "</td></tr></tbody></table></td></tr></tbody></table></div></td></tr></tbody></table>";
            return html;
        }

        public bool SendRejectMailToStudent(string toEmail, string name, string Attorney, string url = "")
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("mail.ab-sol.net");
                mail.From = new MailAddress("eService@ab-sol.net");
                mail.To.Add(toEmail);
                mail.Subject = "Welcome to OCM";
                mail.Body = ConvertHtmlToStringRejectStd(name, Attorney, url);
                mail.IsBodyHtml = true;
                SmtpServer.Port = 587;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new System.Net.NetworkCredential("eService@ab-sol.net", "P@kistan1947");
                SmtpServer.EnableSsl = false;
                SmtpServer.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        private string ConvertHtmlToStringRejectStd(string name, string Attorney, string url)
        {
            string html = string.Empty;
            html += "<style>body { margin-top: 20px;} </style>";
            html += "<table class='body-wrap' style='width:100%;'><tbody><tr><td></td><td>";
            html += "<div style='box-sizing:border-box;max-width:600px;margin:0 auto;padding:20px;'><table><tbody><tr>";
            html += "<td style='padding:30px;border:3px solid #67A8E4;border-radius:7px;'><meta><table><tbody><tr>";
            html += "<td style='font-family:Helvetica Neue,Helvetica,Arial,sans-serif;font-size:14px;padding: 0 0 20px;'>";
            html += $"Hi {name}. Your application has been rejected.</td></tr><tr>";
            html += "<td class='content-block' style='font-family:Helvetica Neue,Helvetica,Arial,sans-serif;font-size:14px;padding: 0 0 20px;'>";
            html += "<p>If any query, please contact Hr department.</p></td></tr><tr>";
            html += "<td style='text-align:center;font-family:Helvetica Neue,Helvetica,Arial,sans-serif;font-size:14px;'>&copy; Multronica Solutions " + DateTime.Now.Year;
            html += "</td></tr></tbody></table></td></tr></tbody></table></div></td></tr></tbody></table>";
            return html;
        }

        public bool SendApproveMailToStudent(string toEmail, string name, string Attorney, string url = "")
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("mail.ab-sol.net");
                mail.From = new MailAddress("eService@ab-sol.net");
                mail.To.Add(toEmail);
                mail.Subject = "Welcome to OCM";
                mail.Body = ConvertHtmlToStringApprovestd(name, Attorney, url);
                mail.IsBodyHtml = true;
                SmtpServer.Port = 587;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new System.Net.NetworkCredential("eService@ab-sol.net", "P@kistan1947");
                SmtpServer.EnableSsl = false;
                SmtpServer.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        private string ConvertHtmlToStringApprovestd(string name, string Attorney, string url)
        {
            string html = string.Empty;
            html += "<style>body { margin-top: 20px;} </style>";
            html += "<table class='body-wrap' style='width:100%;'><tbody><tr><td></td><td>";
            html += "<div style='box-sizing:border-box;max-width:600px;margin:0 auto;padding:20px;'><table><tbody><tr>";
            html += "<td style='padding:30px;border:3px solid #67A8E4;border-radius:7px;'><meta><table><tbody><tr>";
            html += "<td style='font-family:Helvetica Neue,Helvetica,Arial,sans-serif;font-size:14px;padding: 0 0 20px;'>";
            html += $"Hi {name}. Welcome to Multronica solutions</td></tr><tr>";
            html += "<td style='font-family:Helvetica Neue,Helvetica,Arial,sans-serif;font-size:14px;padding: 0 0 20px;'>";
            html += "Your Student account has generated on OCM, Use you email as username and Password. To directly login by clicking the button below.";
            html += "</td></tr><tr><td style ='padding: 0 0 20px;'><a href=";
            html += $"{url} class='btn-primary' itemprop='url' style='font-family:Helvetica Neue,Helvetica,Arial,sans-serif;font-size: 14px; color: #FFF; text-decoration: none; line-height: 2em; font-weight: bold;cursor: pointer; display: inline-block; border-radius: 5px;background-color: #F06292;border-color: #F06292; border-style: solid; border-width: 8px 16px;'>";
            html += "Login</a></td></tr><tr>";
            html += "<td class='content-block' style='font-family:Helvetica Neue,Helvetica,Arial,sans-serif;font-size:14px;padding: 0 0 20px;'>";
            html += "<p>Congratulation for being the part of Multronica Solutions.</p></td></tr><tr>";
            html += "<td style='text-align:center;font-family:Helvetica Neue,Helvetica,Arial,sans-serif;font-size:14px;'>&copy; Multronica Solutions " + DateTime.Now.Year;
            html += "</td></tr></tbody></table></td></tr></tbody></table></div></td></tr></tbody></table>";
            return html;
        }

        public bool NewAdmin(string toEmail, string name, string url = "")
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("mail.ab-sol.net");
                mail.From = new MailAddress("eService@ab-sol.net");
                mail.To.Add(toEmail);
                mail.Subject = "Welcome to OCM";
                mail.Body = ConvertHtmlToStringAdminApprove(toEmail, name , url);
                mail.IsBodyHtml = true;
                SmtpServer.Port = 587;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new System.Net.NetworkCredential("eService@ab-sol.net", "P@kistan1947");
                SmtpServer.EnableSsl = false;
                SmtpServer.Send(mail);
   
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        private string ConvertHtmlToStringAdminApprove(string toEmail, string name,  string url)
        {
            string html = string.Empty;
            html += "<style>body { margin-top: 20px;} </style>";
            html += "<table class='body-wrap' style='width:100%;'><tbody><tr><td></td><td>";
            html += "<div style='box-sizing:border-box;max-width:600px;margin:0 auto;padding:20px;'><table><tbody><tr>";
            html += "<td style='padding:30px;border:3px solid #67A8E4;border-radius:7px;'><meta><table><tbody><tr>";
            html += "<td style='font-family:Helvetica Neue,Helvetica,Arial,sans-serif;font-size:14px;padding: 0 0 20px;'>";
            html += $"Hi {toEmail}. Welcome to Multronica solutions</td></tr><tr>";
            html += "<td style='font-family:Helvetica Neue,Helvetica,Arial,sans-serif;font-size:14px;padding: 0 0 20px;'>";
            html += "Your Admin account has generated on OCM, Use mentioned email as username and Password to access admin dashboard. You can directly login by clicking the button below.";
            html += "</td></tr><tr><td style ='padding: 0 0 20px;'><a href=";
            html += $"{url} class='btn-primary' itemprop='url' style='font-family:Helvetica Neue,Helvetica,Arial,sans-serif;font-size: 14px; color: #FFF; text-decoration: none; line-height: 2em; font-weight: bold;cursor: pointer; display: inline-block; border-radius: 5px;background-color: #F06292;border-color: #F06292; border-style: solid; border-width: 8px 16px;'>";
            html += "Login</a></td></tr><tr>";
            html += "<td class='content-block' style='font-family:Helvetica Neue,Helvetica,Arial,sans-serif;font-size:14px;padding: 0 0 20px;'>";
            html += "<p>Congratulation for being the part of Multronica Solutions.</p></td></tr><tr>";
            html += "<td style='text-align:center;font-family:Helvetica Neue,Helvetica,Arial,sans-serif;font-size:14px;'>&copy; Multronica Solutions " + DateTime.Now.Year;
            html += "</td></tr></tbody></table></td></tr></tbody></table></div></td></tr></tbody></table>";
            return html;
        }











    }
}

