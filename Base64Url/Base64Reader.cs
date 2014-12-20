using System;
using System.Text;

namespace Base64Url
{
    public class Base64Reader
    {
        readonly byte[] _bytes;
        int _position;

        public Base64Reader(string base64) : this(Base64.ToBytes(base64)) { }
        public Base64Reader(string base64, int byteLength) : this(Base64.ToBytes(base64, byteLength)) { }
        protected Base64Reader(byte[] bytes)
        {
            _bytes = bytes;
        }

        public int Length
        {
            get { return _bytes.Length; }
        }

        public int Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public byte[] ReadVarBytes()
        {
            var length = ReadInt32();
            var bytes = new byte[length];
            Buffer.BlockCopy(_bytes, _position, bytes, 0, length);
            _position += length;
            return bytes;
        }

        public string ReadVarString()
        {
            var length = ReadInt32();
            var bytes = new byte[length];
            Buffer.BlockCopy(_bytes, _position, bytes, 0, length);
            _position += length;
            return Encoding.UTF8.GetString(bytes);
        }

        public byte ReadByte()
        {
            return _bytes[_position++];
        }

        public short ReadInt16()
        {
            var i = (_bytes[_position++] << 8) |
                    _bytes[_position++];
            return (short)i;
        }

        public int ReadInt32()
        {
            return (_bytes[_position++] << 24) |
                   (_bytes[_position++] << 16) |
                   (_bytes[_position++] << 8) |
                   _bytes[_position++];
        }

        public long ReadInt64()
        {
            return ((long)_bytes[_position++] << 56) |
                   ((long)_bytes[_position++] << 48) |
                   ((long)_bytes[_position++] << 40) |
                   ((long)_bytes[_position++] << 32) |
                   ((long)_bytes[_position++] << 24) |
                   ((long)_bytes[_position++] << 16) |
                   ((long)_bytes[_position++] << 8) |
                   _bytes[_position++];
        }

        public Guid ReadGuid()
        {
            var bytes = new byte[16];
            Buffer.BlockCopy(_bytes, _position, bytes, 0, 16);
            var result = new Guid(bytes);
            _position += 16;
            return result;
        }

        public DateTime ReadDateTime(long precisionTick = 1)
        {
            var size = 8;
            for (var i = precisionTick << 2; i >= 256; i >>= 8)
                size--;

            long value = 0;
            for (int i = 0; i < size; i++)
            {
                value <<= 8;
                value |= _bytes[_position++];
            }
            value >>= 2;
            return new DateTime(value * precisionTick);
        }

        public string ReadBase64(int length)
        {
            var bytes = new byte[length];
            Buffer.BlockCopy(_bytes, _position, bytes, 0, length);
            _position += length;
            return Base64.GetBase64(bytes);
        }
    }
}