using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;

namespace HexaGone.Models
{
    public static class Hash
    {
        private static string salt = "dfcd3454 bbea788a";
        /// <summary>
        /// Gibt einen MD5 Hash als String zurück
        /// </summary>
        /// <param name="text"/>string der Gehasht werden soll.
        /// <returns>Hash als string.</returns>
        public static string GetMD5Hash(string text)
        {
            //Prüfen ob Daten übergeben wurden.
            if ((text == null) || (text.Length == 0))
            {
                return string.Empty;
            }

            //MD5 Hash aus dem String berechnen. Dazu muss der string in ein Byte[]
            //zerlegt werden. Danach muss das Resultat wieder zurück in ein string.
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] textToHash = Encoding.Default.GetBytes(text + salt);
            byte[] result = md5.ComputeHash(textToHash);
            string hashString = string.Empty;
            foreach (byte b in result)
            {
                hashString += String.Format("{0:x2}", b);
            }

            return hashString;
        }
    }
}
