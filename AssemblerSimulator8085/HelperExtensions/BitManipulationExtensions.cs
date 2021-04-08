using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblerSimulator8085.HelperExtensions
{
    internal static class BitManipulationExtensions
    {
        public static bool[] GetBits(this byte b)
        {
            bool[] result = new bool[8];
            for (int i = 0; i < 8; i++)
                result[i] = (b & (1 << i)) == 0 ? false : true;
            Array.Reverse(result);
            return result;
        }
        public static bool[] GetBits(this ushort b)
        {
            bool[] result = new bool[16];
            for (int i = 0; i < 16; i++)
                result[i] = (b & (1 << i)) == 0 ? false : true;
            Array.Reverse(result);
            return result;
        }
        public static byte GetByte(this bool[] boolArray)
        {
            byte result=0;
            bool[] temp = boolArray.Reverse().ToArray();
            for (int i = 0; i < 8; i++)
            {
                if(temp[i])
                {
                    result += (byte)Math.Pow(2, i);
                }
            }
            return result;
        }
    }
}
