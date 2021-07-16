using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollections.Interfaces
{
    //Define a delegate that will point to a method that will do something to
    //each data member of type T in the tree.
    public delegate void ProcessData<T>(T data);
    //Set up an enumeration to determien the order of iteration.
    public enum TRAVERSALORDER { PRE_ORDER, IN_ORDER, POST_ORDER };

    public interface ItfcBinarySearchTree<T> : ItfcCollection<T> where T : IComparable<T>
    {
        /// <summary>
        /// Given a data element find the corresponding element of equal value to it.
        /// </summary>
        /// <param name="data">A data item to compare to an item in the tree</param>
        /// <returns>A reference to the item if found. Else returns the default value
        /// for the type T</returns>
        T Find(T data);

        /// <summary>
        /// Calculate the height of the tree
        /// </summary>
        /// <returns>Height of the tree</returns>
        int Height();

        /// <summary>
        /// Similar to an enumerator, but more efficient. Also, the iterate method utilizes a
        /// delegate to perform some action on each data item.
        /// </summary>
        /// <param name="pd">A delegate to a fucntion pointer.</param>
        /// <param name="to">The traversal order</param>
        void Iterate(ProcessData<T> pd, TRAVERSALORDER to);
    }
}
