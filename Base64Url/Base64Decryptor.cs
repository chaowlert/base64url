using System.Web.Security;

namespace Base64Url
{
    public class Base64Decryptor : Base64Reader
    {
        public Base64Decryptor(string encrypted, params string[] purposes) : 
            base(Decrypt(encrypted, purposes)) { }

        static byte[] Decrypt(string encrypted, string[] purposes)
        {
            var bytes = Base64.ToBytes(encrypted);
            return MachineKey.Unprotect(bytes, purposes);
        }
    }
}