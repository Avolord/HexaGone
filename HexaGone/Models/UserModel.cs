using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace HexaGone.Models
{
    public class UserModel
    {


        public int userId { get;set; }
        public string email { get;set; }
        public string userName { get; set; }

        /// <summary>
        /// Seperates the session String and fills the Key Values of the UserModel (userId, email, userName), with the provided Data
        /// </summary>
        /// <param name="sessionData">The Current Session String</param>
        public void readSessionCookieData(string sessionData)
        {
            int indexOfData = sessionData.IndexOf('&')+1;
            int endIndexOfData = sessionData.IndexOf('&', indexOfData);
            int dataLength = endIndexOfData - indexOfData;
            userId = Int32.Parse(sessionData.Substring(indexOfData, dataLength));
            indexOfData = endIndexOfData + 1;
            endIndexOfData = sessionData.IndexOf('&', indexOfData);
            dataLength = endIndexOfData - indexOfData;
            email = sessionData.Substring(indexOfData, dataLength);
            indexOfData = endIndexOfData + 1;
            endIndexOfData = sessionData.IndexOf('&', indexOfData);
            dataLength = endIndexOfData - indexOfData;
            userName = sessionData.Substring(indexOfData, dataLength);
        }

        /// <summary>
        /// returns the UserId of the current Session
        /// </summary>
        /// <param name="sessionData">Session String</param>
        /// <param name="onlyID">Can be true or false, only necessary to overload function </param>
        /// <returns>UserId as Integera</returns>
        public int readSessionCookieData(string sessionData, bool onlyID)
        {
            
            int indexOfData = sessionData.IndexOf('&') + 1;
            int endIndexOfData = sessionData.IndexOf('&', indexOfData);
            int dataLength = endIndexOfData - indexOfData;

            return Int32.Parse(sessionData.Substring(indexOfData, dataLength));
            
        }

        /// <summary>
        /// Creates the Session String
        /// </summary>
        /// <returns>Session String</returns>
        public string createSessionString()
        {
            
            return "&" + userId.ToString() + "&" + email + "&" + userName + "&";
            
        }

    }
}
