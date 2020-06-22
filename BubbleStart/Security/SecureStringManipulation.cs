using System;
using System.Runtime.InteropServices;
using System.Security;

namespace BubbleStart.Security
{
    internal class SecureStringManipulation
    {
        public static byte[] ConvertSecureStringToByteArray(SecureString value)
        {
            //Byte array to hold the return value
            byte[] returnVal = new byte[value.Length];

            IntPtr valuePtr = IntPtr.Zero;
            try
            {
                valuePtr = Marshal.SecureStringToGlobalAllocUnicode(value);
                for (int i = 0; i < value.Length; i++)
                {
                    short unicodeChar = Marshal.ReadInt16(valuePtr, i * 2);
                    returnVal[i] = Convert.ToByte(unicodeChar);
                }

                return returnVal;
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
            }
        }
    }
}