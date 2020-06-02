using System.Collections.Concurrent;

namespace Ecotek.Common.Extentions
{

    public static class ConcurrentDictionaryExtension
    {
        public static void AddOrUpdateDic<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> dic, TKey key, TValue value)
        {
            if (!dic.ContainsKey(key))
            {
                dic.TryAdd(key, value);
            }
            else
            {
                dic[key] = value;
            }
        }
    }

}