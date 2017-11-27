using System.Security.Cryptography;
using System.Text;

namespace ITI.Survey.Web.Dll.Helper
{
    public sealed class PhpCompatible
    {
        public static string Md5Hash(string password)
        {
            MD5 md5 = MD5CryptoServiceProvider.Create();
            byte[] dataMd5 = md5.ComputeHash(Encoding.Default.GetBytes(password));
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < dataMd5.Length; i++)
            {
                stringBuilder.AppendFormat("{0:x2}", dataMd5[i]);
            }
            return stringBuilder.ToString();
        }
    }
}
