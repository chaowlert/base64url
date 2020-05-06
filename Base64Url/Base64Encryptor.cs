using System.IO;
using System.Security.Cryptography;

namespace Base64Url
{
    public class Base64Encryptor : Base64Writer
    {
        readonly ICryptoTransform _cryptoTransform;
        public Base64Encryptor(ICryptoTransform cryptoTransform)
        {
            _cryptoTransform = cryptoTransform;
        }

        public override string ToString()
        {
            using (var byteStream = new MemoryStream())
            using (var cryptoStream = new CryptoStream(byteStream, _cryptoTransform, CryptoStreamMode.Write))
            {
                cryptoStream.Write(Bytes, 0, Bytes.Length);
                cryptoStream.FlushFinalBlock();
                return Base64.GetBase64(byteStream.ToArray());
            }
        }
    }
}