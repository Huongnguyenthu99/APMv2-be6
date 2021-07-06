using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace APMv2.Utilities
{
    public class MailUtils
    {
        public static async Task<bool> SendMail(String _from, string _to, string _subject,
            string _body, SmtpClient client)
        {
            MailMessage message = new MailMessage(
                from: _from,
                to: _to,
                subject: _subject,
                body: _body
            );
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.SubjectEncoding = System.Text.Encoding.UTF8;
            message.IsBodyHtml = true;
            message.ReplyToList.Add(new MailAddress(_from));
            message.Sender = new MailAddress(_from);

            try
            {
                await client.SendMailAsync(message);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        public static async Task<bool> SendMailLocalSmtp(string _from, string _to, string _subject, string _body)
        {
            using (SmtpClient client = new SmtpClient("localhost"))
            {
                return await SendMail(_from, _to, _subject, _body, client);
            }
        }

        public static async Task<string> SendMailGoogleSmtp(string _to, int type)
        {
            string passwordTemporary = CreatePassword();
            string username = _to;
            string _body = "";
            string _subject = "Agile Project Management";
            if (type == 0)
            {
                _body = "Chào bạn, chúng tôi đã tạo tài khoản cho bạn trên trang web của chúng tôi. Vui lòng truy cập trang web với username: " + username + " và mật khẩu: " + passwordTemporary;
            }
            else if (type == 1)
            {
                _body = "Chào bạn, mật khẩu tạm thời: " + passwordTemporary + ". Vui lòng đổi mật khẩu ngay sau khi đăng nhập";
            };
            string _from = "huongnguyenym99@gmail.com";
            string _pass = "huongntt151099";

            MailMessage message = new MailMessage(
                from: _from,
                to: _to,
                subject: _subject,
                body: _body);
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.SubjectEncoding = System.Text.Encoding.UTF8;
            message.IsBodyHtml = true;
            message.ReplyToList.Add(new MailAddress(_from));
            message.Sender = new MailAddress(_from);

            // Tạo SmtpClient kết nối đến smtp.gmail.com
            try
            {
                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.Credentials = new NetworkCredential(_from, _pass);
                    smtp.EnableSsl = true;
                    smtp.Send(message);
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            return passwordTemporary;
        }

        public static string CreatePassword()
        {
            var date = DateTime.Now.ToString("yyyyMMddHHmmss");
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            int length = 6;
            while (0 < length--)
            {
                res.Append(date[rnd.Next(date.Length)]);
            }
            return res.ToString();
        }
    }
}
