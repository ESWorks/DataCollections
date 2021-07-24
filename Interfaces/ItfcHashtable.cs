using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollections.Interfaces
{
    public interface ItfcHashtable<K, V> : IEnumerable<V>
        where V : IComparable<V>
        where K : IComparable<K>
    {
        /// <summary>
        /// Finds a stored value from a passed key
        /// </summary>
        /// <param name="key">Used to determine the location of the value in the array</param>
        /// <returns>The value stored at that location</returns>
        V Get(K key);

        /// <summary>
        /// Add the key and value as a key-value pair.
        /// </summary>
        /// <param name="key">Used to determine the location in the array</param>
        /// <param name="value">Value to be store at the location.</param>
        void Add(K key, V value);

        /// <summary>
        /// Remove the item from the array at the location provided by hasing the key
        /// </summary>
        /// <param name="key">The key to determine the location of the item</param>
        void Remove(K key);

        /// <summary>
        /// Removes all values from the hashtable and initializes to the default array size.
        /// </summary>
        void Clear();
    }
}
