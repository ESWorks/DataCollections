using DataCollections.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollections.Collection
{
    class AVLTree<T> : BinarySearchTree<T> where T : IComparable<T>
    {
        #region Balance
        internal override TreeNode<T> Balance(TreeNode<T> nCurrent)
        {
            TreeNode<T> nNewRoot = nCurrent;
            //if current is not null
            if (nCurrent != null)
            {
                //heightdiff <-- get height diff of the current node
                int heightDif = GetHeightDifference(nCurrent);
                //if the tree is unbalanced to the right
                if (heightDif < -1)
                {
                    //RightChildTreeDiff <-- get height diff of currents right child
                    int rightChildDif = GetHeightDifference(nCurrent.Right);
                    //if the child is left heavy
                    if (rightChildDif > 0)
                    {
                        //nNewRoot <-- doubleLeft on current node
                        nNewRoot = DoubleLeft(nCurrent);
                    }
                    //else
                    else
                    {
                        //nNewRoot <-- Single left on current node
                        nNewRoot = SingleLeft(nCurrent);
                    }
                }
                //else if the BinarySearchTree is unblalanced to the left
                else if (heightDif > 1)
                {
                    //leftChildTreeDif <-- get height dif of currents left child
                    int leftChildDif = GetHeightDifference(nCurrent.Left);
                    //if the child is left heavy
                    if (leftChildDif < 0)
                    {
                        //nNewRoot <-- doubleRight on current node
                        nNewRoot = DoubleRight(nCurrent);
                    }
                    //else
                    else
                    {
                        //nNewRoot <-- Single right on current node
                        nNewRoot = SingleRight(nCurrent);
                    }
                }

            }
            return nNewRoot;
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// This method returns the height difference betweenthe children
        /// A positive number indicates a left heavy tree
        /// A negative number indicates a right heavy tree
        /// </summary>
        /// <param name="nCurrent">Node to determine height dif of</param>
        /// <returns>height dif as + or -</returns>
        private int GetHeightDifference(TreeNode<T> nCurrent)
        {
            int iHeightLeft = -1;
            int iHeightRight = -1;
            int iHeightDiff = 0;

            if (nCurrent != null)
            {
                if (nCurrent.Right != null)
                {
                    iHeightLeft = recHeight(nCurrent.Right);
                }

                if (nCurrent.Right != null)
                {
                    iHeightRight = recHeight(nCurrent.Left);
                }

            }
            else
            {
                throw new ApplicationException("Specified Node is Null, cant get height difference.");
            }

            iHeightDiff = iHeightLeft - iHeightRight;

            return iHeightDiff;
        }
        #endregion

        #region Rotation Methods

        /// <summary>
        /// pass in the old root, do the rotation, and return the new root
        /// assumes that the current subtree requires a single left rotation
        /// </summary>
        /// <param name="nOldRoot"></param>
        /// <returns></returns>
        private TreeNode<T> SingleLeft(TreeNode<T> nOldRoot)
        {
            //Step one
            TreeNode<T> nNewRoot = nOldRoot.Right;
            //Step two
            nOldRoot.Right = nNewRoot.Left;
            //Step three
            nNewRoot.Left = nOldRoot;
            //return the new root
            return nNewRoot;
        }

        private TreeNode<T> SingleRight(TreeNode<T> nOldRoot)
        {
            //Step one
            TreeNode<T> nNewRoot = nOldRoot.Left;
            //Step two
            nOldRoot.Left = nNewRoot.Right;
            //Step three
            nNewRoot.Right = nOldRoot;
            //return the new root
            return nNewRoot;
        }

        private TreeNode<T> DoubleLeft(TreeNode<T> nOldRoot)
        {
            nOldRoot.Right = SingleRight(nOldRoot.Right);
            return SingleLeft(nOldRoot);
        }

        private TreeNode<T> DoubleRight(TreeNode<T> nOldRoot)
        {
            nOldRoot.Left = SingleLeft(nOldRoot.Left);
            return SingleRight(nOldRoot);
        }
        #endregion
    }
}
