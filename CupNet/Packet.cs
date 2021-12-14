using System;
using System.Collections.Generic;
using System.Text;

namespace CupNet
{
    public class Packet // : IDisposable
    {
        private List<byte> buffer = new List<byte>();
        private int readPos = 0;

        #region Constructors
        public Packet() { }
        public Packet(int packetId)
        {
            Write(packetId);
        }
        public Packet(byte[] data)
        {
            Write(data);
        }
        #endregion
        
        #region Write methods
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
    
        public void WriteLength()
        {
            buffer.InsertRange(0, BitConverter.GetBytes(buffer.Count));
        }
        #endregion

        #region Read methods
        public int ReadInt(bool moveReadPos = true)
        {
            try
            {
                int value = BitConverter.ToInt32(ToArray(), readPos);
                if (moveReadPos) readPos += sizeof(int);
                return value;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error occured while attempting to read Int32 in packet: {e}");
                throw;
            }
        }
    
        public byte[] ReadBytes(int count, bool moveReadPos = true)
        {
            try
            {
                byte[] value = buffer.GetRange(readPos, count).ToArray();
                if (moveReadPos) readPos += count;
                return value;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error occured while attempting to read Int32 in packet: {e}");
                throw;
            }
        }

        public string ReadString(bool moveReadPos = true)
        {
            try
            {
                int length = ReadInt();
                string value = Encoding.ASCII.GetString(ToArray(), readPos, length);
                if (moveReadPos)
                {
                    readPos += length;
                }
                return value;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error occured while attempting to read string in packet: {e}");
                throw;
            }
        }
        #endregion
    
        #region Public Properties
        public byte[] ToArray() => buffer.ToArray();
        public int Size() => buffer.Count;
        public int UnreadSize() => Size() - readPos;
        #endregion
    }
}