using DataCollections.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollections.Collection.Hashtable
{
    public class DoubleHashtable<K, V> : AbsOpenAddressing<K, V>
        where V : IComparable<V>
        where K : IComparable<K>
    {
        protected override int GetIncrement(int iAttempt, K Key)
        {
            return (1 + Math.Abs(Key.GetHashCode()) % (HTSize - 1)) * iAttempt;
        }

        public DoubleHashtable()
        {

        }
    }
}
