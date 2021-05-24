using System;
using System.Linq;

namespace Scrbll.Api.Utils
{
    public class Randoms
    {
        public static Random random = new Random();
        const string ALNUM = "abcdefghijlkmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        public static string RandomString(int length)
        {
            return new string(Enumerable.Repeat(ALNUM, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
