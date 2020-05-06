using System;
using System.Text;

namespace Base64Url
{
    public static class Base64
    {
        public static char GetChar(byte b)
        {
            if (b == 0)
                return '-';
            if (b <= 10)
                return (char)(47 + b);
            if (b <= 36)
                return (char)(54 + b);
            if (b == 37)
                return '_';
            if (b <= 63)
                return (char)(59 + b);
            else
                throw new ArgumentOutOfRangeException(nameof(b));
        }

        public static byte GetByte(char c)
        {
            if (c == '-')
                return 0;
            if (c >= '0' && c <= '9')
                return (byte)(c - 47);
            if (c >= 'A' && c <= 'Z')
                return (byte)(c - 54);
            if (c == '_')
                return 37;
            if (c >= 'a' && c <= 'z')
                return (byte)(c - 59);
            else
                throw new ArgumentOutOfRangeException(nameof(c));
        }

        public static int GetStringLength(int byteLength)
        {
            var result = Math.DivRem(byteLength, 3, out var rem) * 4;
            if (rem == 1)
                result += 2;
            else if (rem == 2)
                result += 3;
            return result;
        }

        public static int GetByteLength(int strLength)
        {
            var result = Math.DivRem(strLength, 4, out var rem) * 3;
            if (rem == 1)
                throw new InvalidOperationException();
            else if (rem == 2)
                result += 1;
            else if (rem == 3)
                result += 2;
            return result;
        }

        public static string GetBase64(byte[] bytes)
        {
            var len = bytes.Length;
            var sb = new StringBuilder(GetStringLength(len));
            for (var i = 0; i < len; )
            {
                //b1
                var b = bytes[i++];
                var b1 = (byte)(b >> 2); //xxxxxx-- => xxxxxx
                sb.Append(GetChar(b1));

                //b2
                var b2 = (byte)((b & 3) << 4); //------xx => xx----
                if (i >= len)
                {
                    sb.Append(GetChar(b2));
                    break;
                }
                b = bytes[i++];
                b2 |= (byte)(b >> 4); //xxxx---- => --xxxx
                sb.Append(GetChar(b2));

                //b3
                var b3 = (byte)((b & 15) << 2); //----xxxx => xxxx--
                if (i >= len)
                {
                    sb.Append(GetChar(b3));
                    break;
                }
                b = bytes[i++];
                b3 |= (byte)(b >> 6); //xx------ => ----xx
                sb.Append(GetChar(b3));

                //b4
                var b4 = (byte)(b & 63); //--xxxxxx => xxxxxx
                sb.Append(GetChar(b4));
            }
            return sb.ToString();
        }

        public static byte[] ToBytes(string str)
        {
            var len = str.Length;
            var byteLength = GetByteLength(len);
            return ToBytes(str, byteLength);
        }

        public static byte[] ToBytes(string str, int byteLength)
        {
            var bytes = new byte[byteLength];
            for (int i = 0,
                     j = 0; j < byteLength; )
            {
                //c1
                var c = GetByte(str[i++]);
                var c1 = (byte)(c << 2); //xxxxxx => xxxxxx--
                c = GetByte(str[i++]);
                c1 |= (byte)(c >> 4); //xx---- => ------xx
                bytes[j++] = c1;
                if (j >= byteLength)
                    break;

                //c2
                var c2 = (byte)((c & 15) << 4); //--xxxx => xxxx----
                c = GetByte(str[i++]);
                c2 |= (byte)(c >> 2); //xxxx-- => ----xxxx
                bytes[j++] = c2;
                if (j >= byteLength)
                    break;

                //c3
                var c3 = (byte)((c & 3) << 6); //----xx >> xx------
                c = GetByte(str[i++]);
                c3 |= c; //xxxxxx >> --xxxxxx
                bytes[j++] = c3;
            }
            return bytes;
        }

        public static string GetBase64(string str)
        {
            var bytes = Encoding.UTF8.GetBytes(str);
            return GetBase64(bytes);
        }

        public static string ToString(string str)
        {
            var bytes = ToBytes(str);
            return Encoding.UTF8.GetString(bytes);
        }

        public static string NewId()
        {
            return GetBase64(Guid.NewGuid().ToByteArray());
        }
    }
}
