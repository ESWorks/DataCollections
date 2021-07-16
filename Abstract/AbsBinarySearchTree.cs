using DataCollections.Classes;
using DataCollections.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollections.Abstract
{
    public abstract class AbsBinarySearchTree<T> : AbsCollection<T>, ItfcBinarySearchTree<T> where T : IComparable<T>
    {
        #region Attributes
        //A reference to the root node
        internal TreeNode<T> nRoot;
        //A counter to keep track of the number of data items in the tree
        internal int iCount;
        #endregion

        public override int Count
        {
            get
            {
                return iCount;
            }
        }
        #region I_BST implementation
        public abstract T Find(T data);
        public abstract int Height();
        public abstract void Iterate(ProcessData<T> pd, TRAVERSALORDER to);
        #endregion
    }
}
