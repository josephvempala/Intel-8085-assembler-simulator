using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblerSimulator8085.HelperExtensions
{
    static class DictionarySwapHelper
    {
        public static Dictionary<Y,X> SwapKeyValues<X,Y>(this Dictionary<X,Y> Dictionary)
        {
            Dictionary<Y, X> swapped = new Dictionary<Y, X>();
            foreach(var item in Dictionary)
            {
                if(!swapped.ContainsKey(item.Value))
                {
                    swapped.Add(item.Value,item.Key);
                }
            }
            return swapped;
        }
    }
}
