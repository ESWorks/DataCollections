using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollections.Classes
{
    public class KeyValue<K, V>
        where V : IComparable<V>
        where K : IComparable<K>
    {
        // store the key of the data (similar to a primary key in database records)
        K kKey;
        // store the actual data
        V vValue;

        int priority;

        public int Priority
        {
            get { return priority; }
            set { priority = value; }
        }



        public KeyValue(K key, V value)
        {
            kKey = key;
            vValue = value;
        }

        public V Value
        {
            get { return vValue; }
        }

        public K Key
        {
            get { return kKey; }
        }

        /// <summary>
        /// Need to override equals so the key/value pair can be compared to another key/value pair. 
        /// Espcially important for the ArrayList.Contains method, which calls Equals for comparisons.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            KeyValue<K, V> kv = (KeyValue<K, V>)obj;
            return this.Key.CompareTo(kv.Key) == 0;
        }

        public override int GetHashCode() => base.GetHashCode();
    }
}
