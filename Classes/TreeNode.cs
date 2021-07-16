using DataCollections.Sort;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollections.Classes
{
    class TreeNode<T> where T : IComparable<T>
    {
        #region Attributes
        private T tData;
        private TreeNode<T> nLeft;
        private TreeNode<T> nRight;
        #endregion

        #region Constructors

        public TreeNode() : this(default(T)) { }

        public TreeNode(T tData) : this(tData, null, null)
        {
            
        }
        public TreeNode(T tData, TreeNode<T> nLeft, TreeNode<T> nRight)
        {
            this.tData = tData;
            this.nLeft = nLeft;
            this.nRight = nRight;
        }
        #endregion

        #region Properties
        //Note that the default property has both a 'get' and 'set'. You should consider
        //whether you need both or not.
        public T Data
        {
            get { return tData; }
            set { tData = value; }
        }

        public TreeNode<T> Left
        {
            get
            {
                //Business rules go here
                return nLeft;
            }
            set
            {
                //Business rules go here
                nLeft = value;
            }
        }

        public TreeNode<T> Right
        {
            get { return nRight; }
            set { nRight = value; }
        }
        #endregion

        #region Other Functionality

        public bool IsLeaf()
        {
            return Left == null && Right == null;
        }
        #endregion
    }
}
