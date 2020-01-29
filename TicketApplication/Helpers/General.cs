using System;
using System.Data;
using System.Text;
using System.Web;

namespace TicketApplication.Helpers
{
    public static class General
    {
        private static Random random = new Random((int)DateTime.Now.Ticks);
        private static string randomChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private static string randomCharsLower = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        public static string GetStartOfUrl()
        {
            return IsOnLocalhost() ? "http://" : "https://";
        }

        public static bool IsOnLocalhost()
        {
            return HttpContext.Current.Request.Url.AbsoluteUri.Substring(0, 5) == "https" ? false : true;
        }

        public static string RandomString(int size)
        {
            StringBuilder builder = new StringBuilder();
            char ch;

            for (int i = 0; i < size; i++)
            {
                ch = randomChars[Convert.ToInt32(Math.Floor(randomChars.Length * random.NextDouble()))];
                builder.Append(ch);
            }

            return builder.ToString();
        }

        public static string RandomStringLower(int size)
        {
            StringBuilder builder = new StringBuilder();
            char ch;

            for (int i = 0; i < size; i++)
            {
                ch = randomCharsLower[Convert.ToInt32(Math.Floor(randomChars.Length * random.NextDouble()))];
                builder.Append(ch);
            }

            return builder.ToString();
        }

        internal static DataRow ReturnFirst(DataTable dt)
        {
            if (dt == null)
            {
                return null;
            }

            if (dt.Rows == null)
            {
                return null;
            }

            return dt.Rows[0];
        }
    }
}