using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace HappyFarmProjectAPI
{
    public class Helper
    {
        /// <summary>
        /// To Encrypt string using SHA 256 Hash Algorithm
        /// </summary>
        /// <param name="rawData"></param>
        /// <returns></returns>
        public static string EncryptStringSha256Hash(string rawData)
        {
            // create a SHA256
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // computeHash - returns byte array
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // convert byte array to a string
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }

        /// <summary>
        /// To Validate Email must be valid
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool ValidateEmail(string email)
        {
            try
            {
                string emailRegex = @"^[A-Za-z0-9._+]+@[A-Za-z0-9._+]+[.][A-Za-z0-9._+]+$";
                Regex regex = new Regex(emailRegex);
                return regex.IsMatch(email);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// To Validate Phone Number must be numberic
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public static bool ValidatePhoneNumber(string phoneNumber)
        {
            try
            {
                long.Parse(phoneNumber);
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Send email async
        /// </summary>
        /// <param name="to"></param>
        /// <param name="subject"></param>
        /// <param name="Body"></param>
        public static void SendMailAsync(string to, string subject, string Body)
        {
            // mail message
            MailMessage mailMessage = new MailMessage();
            mailMessage.Subject = subject;
            mailMessage.From = new MailAddress("happyfarm441@gmail.com");
            mailMessage.Body = Body;
            mailMessage.IsBodyHtml = true;
            mailMessage.To.Add(new MailAddress(to));

            // smptp for sending a new message
            SmtpClient client = new SmtpClient();
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("happyfarm441@gmail.com", "HappyFarm123Admin");
            client.SendAsync(mailMessage, "Mail State");
        }

        /// <summary>
        /// Generate a random number between two numbers
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        private static int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        /// <summary>
        /// Generate a random string with a given size and case
        /// </summary>
        /// <param name="size"></param>
        /// <param name="lowerCase"></param>
        /// <returns></returns>
        private static string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            Char ch;
            for(int i =0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }

            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }

        public static string RandomCode()
        {
            return RandomString(7, false);
        }

        public static string RandomPassword()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(RandomString(4, true));
            builder.Append(RandomNumber(1000, 9999));
            builder.Append(RandomString(3, false));
            char[] validChars = builder.ToString().ToCharArray();

            Random random = new Random();
            int length = builder.ToString().Length;
            char[] chars = new char[length];
            for(int i = 0; i < length; i++)
            {
                chars[i] = validChars[random.Next(0, length)];
            }

            return new string(chars);
        }
        
        public static bool IsFileAvailable(HttpRequest httpRequest)
        {
            List<string> files = new List<string>();
            foreach (string file in httpRequest.Files)
            {
                // get file
                var postedFile = httpRequest.Files[file];
                if (postedFile.FileName != "") files.Add(postedFile.FileName);
            }

            return files.Count > 0;
        }
    }
}