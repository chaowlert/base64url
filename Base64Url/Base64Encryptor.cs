using System.Web.Security;

namespace Base64Url
{
    public class Base64Encryptor : Base64Writer
    {
        readonly string[] _purposes;
        public Base64Encryptor(params string[] purposes)
        {
            _purposes = purposes;
        }

        public override string ToString()
        {
            var encrypted = MachineKey.Protect(Bytes, _purposes);
            return Base64.GetBase64(encrypted);
        }
    }
}