using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollections.Interfaces
{
    public interface ItfcCollection<T> : IEnumerable<T> where T: IComparable<T>
    {   
        /// <summary>
        /// Adds the given data to this collection
        /// </summary>
        /// <param name="data">Data to add</param>
        void Add(T data);

        /// <summary>
        /// Removes all items from the collection
        /// </summary>
        void Clear();

        /// <summary>
        /// Determines if the data is in the structure or not
        /// </summary>
        /// <param name="data">Data item to find</param>
        /// <returns>True if found else false</returns>
        bool Contains(T data);

        /// <summary>
        /// Determines if this data structure is equal to another
        /// </summary>
        /// <param name="other">The passed in data structure to be cmpared with the
        /// calling data structure.</param>
        /// <returns>true if equal, else false</returns>
        bool Equals(object other);

        /// <summary>
        /// Remove the first instance of a value if it exists.
        /// </summary>
        /// <param name="data">Data to remove</param>
        /// <returns>True if removed else false</returns>
        bool Remove(T data);

        /// <summary>
        /// A property used to access the number of elements in the collection.
        /// </summary>
        int Count
        {
            get;
        }
    }
}
