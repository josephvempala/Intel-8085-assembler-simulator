using System.Collections.Generic;

namespace AssemblerSimulator8085.HelperExtensions
{
    internal static class DictionarySwapHelper
    {
        public static Dictionary<Y, X> SwapKeyValues<X, Y>(this Dictionary<X, Y> Dictionary)
        {
            Dictionary<Y, X> swapped = new Dictionary<Y, X>();
            foreach (KeyValuePair<X, Y> item in Dictionary)
            {
                if (!swapped.ContainsKey(item.Value))
                {
                    swapped.Add(item.Value, item.Key);
                }
            }
            return swapped;
        }
    }
}
