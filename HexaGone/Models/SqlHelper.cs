using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HexaGone.Models
{
    public class SqlHelper
    {
        public static List<string> forbiddenStrings = new List<string> { "drop", "--'",  ";", "*",  "=", "'", "(", ")", "{", "}", "[", "]", "%", "select", "from", "count", "where", "distinct", "desc", "insert", "into", "set", "user", "delete", "truncate", "update" };
        static public bool IsStringValid(string query)
        {
            query = query.ToLower();
            foreach (string n in forbiddenStrings)
            {
                if (query.Contains(n))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
