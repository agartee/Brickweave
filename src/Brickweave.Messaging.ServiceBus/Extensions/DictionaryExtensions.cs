using System.Collections.Generic;

namespace Brickweave.Messaging.ServiceBus.Extensions
{
    public static class DictionaryExtensions
    {
        public static void TryAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary,
            KeyValuePair<TKey, TValue> item)
        {
            if (!dictionary.ContainsKey(item.Key))
                dictionary.Add(item);
        }
    }
}
