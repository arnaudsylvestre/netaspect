using System.Collections.Generic;

namespace System.Runtime.CompilerServices
{
    public class ConditionalWeakTable<TKey, TValue>
        where TValue : new()
    {
        Dictionary<TKey, TValue> dico = new Dictionary<TKey, TValue>(); 

        public TValue GetOrCreateValue(TKey o)
        {
            if (!dico.ContainsKey(o))
                dico.Add(o, new TValue());
            return dico[o];
        }
    }
}