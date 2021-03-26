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
    }
}
