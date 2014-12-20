using System;
using System.Collections.Generic;
using System.Text;

namespace Base64Url
{
    public class Base64Writer
    {
        public Base64Writer()
        {
            _bytes = new List<byte>();
        }

        public Base64Writer(int capacity)
        {
            _bytes = new List<byte>(capacity);
        }

        readonly List<byte> _bytes;
        protected byte[] Bytes
        {
            get { return _bytes.ToArray(); }
        }

        public void WriteVar(byte[] bytes)
        {
            Write(bytes.Length);
            _bytes.AddRange(bytes);
        }

        public void WriteVar(string str)
        {
            var bytes = Encoding.UTF8.GetBytes(str);
            WriteVar(bytes);
        }

        public void Write(byte b)
        {
            _bytes.Add(b);
        }

        public void Write(byte[] bytes)
        {
            _bytes.AddRange(bytes);
        }

        public void Write(short i)
        {
            _bytes.Add((byte)(i >> 8));
            _bytes.Add((byte)i);
        }

        public void Write(int i)
        {
            _bytes.Add((byte)(i >> 24));
            _bytes.Add((byte)(i >> 16));
            _bytes.Add((byte)(i >> 8));
            _bytes.Add((byte)i);
        }

        public void Write(long i)
        {
            _bytes.Add((byte)(i >> 56));
            _bytes.Add((byte)(i >> 48));
            _bytes.Add((byte)(i >> 40));
            _bytes.Add((byte)(i >> 32));
            _bytes.Add((byte)(i >> 24));
            _bytes.Add((byte)(i >> 16));
            _bytes.Add((byte)(i >> 8));
            _bytes.Add((byte)i);
        }

        public void Write(Guid guid)
        {
            _bytes.AddRange(guid.ToByteArray());
        }

        public void Write(DateTime date, long precisionTick = 1)
        {
            if (precisionTick <= 0)
                throw new ArgumentOutOfRangeException("precisionTick");

            var value = date.Ticks / precisionTick;
            value <<= 2;

            var size = 8;
            for (var i = precisionTick << 2; i >= 256; i >>= 8)
                size--;

            for (int i = 0; i < size; i++)
            {
                var pos = (7 - i) * 8;
                _bytes.Add((byte)(value >> pos));
            }
        }

        public void WriteBase64(string str)
        {
            _bytes.AddRange(Base64.ToBytes(str));
        }

        public override string ToString()
        {
            return Base64.GetBase64(Bytes);
        }
    }
}