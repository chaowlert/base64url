using System.IO;
using System.Security.Cryptography;

namespace Base64Url
{
    public class Base64Decryptor : Base64Reader
    {
        public Base64Decryptor(string encrypted, ICryptoTransform cryptoTransform) : 
            base(Decrypt(encrypted, cryptoTransform)) { }

        static byte[] Decrypt(string encrypted, ICryptoTransform cryptoTransform)
        {
            var bytes = Base64.ToBytes(encrypted);
            using (var source = new MemoryStream(bytes))
            using (var cryptoStream = new CryptoStream(source, cryptoTransform, CryptoStreamMode.Read))
            using (var target = new MemoryStream())
            {
                cryptoStream.CopyTo(target);
                return target.ToArray();
            }
        }
    }
}