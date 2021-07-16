using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollections.Interfaces
{
    public interface ItfcList<T> : ItfcCollection<T> where T : IComparable<T>
    {
        /// <summary>
        /// Returns the element a particular index
        /// </summary>
        /// <param name="index">index of the item to find</param>
        /// <returns>the element at index</returns>
        T ElementAt(int index);

        /// <summary>
        /// Given a data element, return the index of the first item encountered.
        /// </summary>
        /// <param name="data">The item to find</param>
        /// <returns>the index of the item to find</returns>
        int IndexOf(T data);

        /// <summary>
        /// Add a data item at any index in the list
        /// </summary>
        /// <param name="index"></param>
        /// <param name="data"></param>
        void Insert(int index, T data);

        /// <summary>
        /// Removes an element a particular index
        /// </summary>
        /// <param name="index">Index of the item to remove.</param>
        /// <returns>The data item that was removed</returns>
        T RemoveAt(int index);

        /// <summary>
        /// Find a data item at a particular location and replace that data item.
        /// </summary>
        /// <param name="index">Location of the item to replace</param>
        /// <param name="data">Data that replaces existing data</param>
        /// <returns>Existing data</returns>
        T ReplaceAt(int index, T data);
    }
}
