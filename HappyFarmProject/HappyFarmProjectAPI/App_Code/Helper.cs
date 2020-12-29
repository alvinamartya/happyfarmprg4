using System;
using System.Collections.Generic;
using System.Linq;
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
                for(int i = 0; i < bytes.Length; i++)
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
            catch(Exception ex)
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
                int.Parse(phoneNumber);
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
                return false;
            }
        }
    }
}