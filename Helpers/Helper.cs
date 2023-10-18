using System.Security.Cryptography;
using System.Text;

namespace AlifTestTask
{
    public static class Helper
    {
        public static string CreateSHA1Hash(string str)
        {
            string rethash = "";
            try
            {
                SHA1 hash = SHA1.Create();
                ASCIIEncoding encoder = new ASCIIEncoding();
                byte[] combined = encoder.GetBytes(str);
                byte[] hashBytes = hash.ComputeHash(combined);
                rethash = BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
                //rethash = Convert.ToBase64String(hash.Hash);  LRtiSVriU5bcA/n+DRiyxlQ01fo=
            }
            catch (Exception ex)
            {
                string strerr = "Error in HashCode : " + ex.Message;
            }

            return rethash;
        }
    }
}
