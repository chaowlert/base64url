using System;
using System.Linq;
using System.Security.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Base64Url.Test
{
    [TestClass]
    public class EncryptionTest
    {
        [TestMethod]
        public void TestEncryption()
        {
            using var rmCrypto = new RijndaelManaged();  
  
            byte[] key = {0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16};  
            byte[] iv = {0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16};

            using var encryptTransform = rmCrypto.CreateEncryptor(key, iv);

            var encryptor = new Base64Encryptor(encryptTransform);
            encryptor.Write(DateTime.MaxValue);
            encryptor.Write(DateTime.MaxValue, 1000);
            encryptor.Write(Guid.Empty);
            encryptor.Write(byte.MaxValue);
            encryptor.Write(new byte[] { 2, 3 });
            encryptor.Write(int.MaxValue);
            encryptor.Write(long.MaxValue);
            encryptor.Write(short.MaxValue);
            encryptor.Write(int.MinValue);
            encryptor.Write(long.MinValue);
            encryptor.Write(short.MinValue);
            var id = Base64.NewId();
            encryptor.WriteBase64(id);
            encryptor.WriteVar("OK");
            encryptor.WriteVar(new byte[] { 7, 8 });

            var encrypted = encryptor.ToString();
            var again = encryptor.ToString();

            Assert.AreEqual(encrypted, again);

            using var decryptTransform = rmCrypto.CreateDecryptor(key, iv);

            var decryptor = new Base64Decryptor(encrypted, decryptTransform);
            Assert.AreEqual(DateTime.MaxValue, decryptor.ReadDateTime());
            Assert.IsTrue(DateTime.MaxValue.Ticks - decryptor.ReadDateTime(1000).Ticks <= 1000);
            Assert.AreEqual(Guid.Empty, decryptor.ReadGuid());
            Assert.AreEqual(byte.MaxValue, decryptor.ReadByte());
            Assert.IsTrue(new byte[] {2, 3}.SequenceEqual(decryptor.ReadBytes(2)));
            Assert.AreEqual(int.MaxValue, decryptor.ReadInt32());
            Assert.AreEqual(long.MaxValue, decryptor.ReadInt64());
            Assert.AreEqual(short.MaxValue, decryptor.ReadInt16());
            Assert.AreEqual(int.MinValue, decryptor.ReadInt32());
            Assert.AreEqual(long.MinValue, decryptor.ReadInt64());
            Assert.AreEqual(short.MinValue, decryptor.ReadInt16());
            Assert.AreEqual(id, decryptor.ReadBase64(16));
            Assert.AreEqual("OK", decryptor.ReadVarString());
            Assert.IsTrue(new byte[] {7, 8}.SequenceEqual(decryptor.ReadVarBytes()));
        }
    }
}
