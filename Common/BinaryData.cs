using System;

namespace Common
{
    public class BinaryData
    {
        public byte[] Bytes { get; private set; }

        public BinaryData(byte[] bytes)
        {
            Bytes = bytes ?? throw new ArgumentNullException(nameof(bytes));
        }
    }
}