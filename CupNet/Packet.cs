using System;
using System.Collections.Generic;
using System.Text;

namespace CupNet
{
    public class Packet // : IDisposable
    {
        private List<byte> buffer = new List<byte>();
        private int readPos = 0;

        public Packet(int packetId)
        {
            Write(packetId);
        }
        
        #region Write method overloads
        public void Write(byte value) => buffer.Add(value);
        public void Write(byte[] value) => buffer.AddRange(value);
        public void Write(short value) => buffer.AddRange(BitConverter.GetBytes(value));
        public void Write(int value) => buffer.AddRange(BitConverter.GetBytes(value));
        public void Write(long value) => buffer.AddRange(BitConverter.GetBytes(value));
        public void Write(float value) => buffer.AddRange(BitConverter.GetBytes(value));
        public void Write(bool value) => buffer.AddRange(BitConverter.GetBytes(value));
        public void Write(string value)
        {
            Write(value.Length);
            buffer.AddRange(Encoding.ASCII.GetBytes(value));
        }
        #endregion

        public void WriteLength()
        {
            buffer.InsertRange(0, BitConverter.GetBytes(buffer.Count));
        }

        public byte[] ToArray() => buffer.ToArray();
        public int Length() => buffer.Count;
        public int UnreadLength() => Length() - readPos;
    }
}