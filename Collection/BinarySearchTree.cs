using DataCollections.Abstract;
using DataCollections.Classes;
using DataCollections.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollections.Collection
{
    public class BinarySearchTree<T> : AbsBinarySearchTree<T> where T : IComparable<T>
    {
        /// <summary>
        /// Note that the function is virtual and can be overwritten in a child class
        /// </summary>
        /// <param name="nCurrent"></param>
        /// <returns></returns>
        internal virtual TreeNode<T> Balance(TreeNode<T> nCurrent)
        {
            return nCurrent;
        }

        #region A_BST implementation
        /// <summary>
        /// Finds a specified value within the tree
        /// </summary>
        /// <param name="data"></param>
        /// <returns>returns the value found in the tree</returns>
        public override T Find(T data)
        {
            T tReturn = default(T);

            //if the root is not null run recursive find
            if (nRoot != null)
            {
                return tReturn = RecFind(nRoot, data);
            }

            return tReturn;
        }

        private T RecFind(TreeNode<T> nCurrent, T data)
        {
            //default value of object or primitive passed in
            T tReturn = default(T);

            //if the current node is a null
            if (nCurrent != null)
            {
                // if data is less than nCurrent
                if (data.CompareTo(nCurrent.Data) < 0)
                {
                    return RecFind(nCurrent.Left, data);
                }
                //if nCurrent is equal to data
                else if (nCurrent.Data.CompareTo(data) == 0)
                {
                    return tReturn = data;
                }
                //else data is greater than nCurrent
                else
                {
                    return RecFind(nCurrent.Right, data);
                }
            }

            return tReturn;
        }

        /// <summary>
        /// Count the number of nodes in the tree
        /// </summary>
        /// <returns>the number of nodes in the tree</returns>
        public int CountNodes()
        {
            //Number of nodes in the tree
            int iCount = 0;

            if (nRoot != null)
            {
                iCount = RecCountNodes(nRoot, iCount);
            }

            return iCount;
        }

        private int RecCountNodes(TreeNode<T> nCurrent, int iCount)
        {
            //if left child node exists
            if (nCurrent.Left != null)
            {
                //increment iCount
                iCount = iCount + 1;
                return RecCountNodes(nCurrent.Left, iCount);
            }
            //if right child node exists
            if (nCurrent.Right != null)
            {
                //increment iCount
                iCount = iCount + 1;
                return RecCountNodes(nCurrent.Right, iCount);
            }
            if (nCurrent.IsLeaf())
            {
                return iCount + 1;
            }

            return iCount;
        }

        public override int Height()
        {
            int treeHeight = -1;

            if (nRoot != null)
            {
                treeHeight = recHeight(nRoot);
            }

            return treeHeight;
        }

        internal int recHeight(TreeNode<T> currentNode)
        {
            //Algorithm

            //height left
            //height right
            //height current
            int heightLeft = 0;
            int heightRight = 0;
            int heightCurrent = 0;

            //Recursive Case
            if (!currentNode.IsLeaf())
            {
                //if currents left is not null
                if (currentNode.Left != null)
                {
                    //recursively find height of the left, assign to height left
                    heightLeft = recHeight(currentNode.Left);
                }
                //if currents right is not null
                if (currentNode.Right != null)
                {
                    //recursively find height of the right, assign to height right
                    heightRight = recHeight(currentNode.Right);
                }

                //Ternary method
                //heightCurrent = heightLeft > heightRight ? ++heightLeft : ++heightRight;
                //Normal method
                //if height left > height right
                if (heightLeft > heightRight)
                {
                    //height current = height left + 1
                    heightCurrent = heightLeft + 1;
                }
                //else height left < height right
                else
                {
                    //height current = height right + 1
                    heightCurrent = heightRight + 1;
                }

            }

            //return height current
            return heightCurrent;
        }

        public override void Iterate(ProcessData<T> pd, TRAVERSALORDER to)
        {
            if (nRoot != null)
            {
                RecIterate(nRoot, pd, to);
            }
        }

        private void RecIterate(TreeNode<T> nCurrent, ProcessData<T> pd, TRAVERSALORDER to)
        {

            //Process the current node
            if (to == TRAVERSALORDER.PRE_ORDER)
            {
                pd(nCurrent.Data);
            }

            if (nCurrent.Left != null)
            {
                RecIterate(nCurrent.Left, pd, to);
            }


            //Process the current node
            if (to == TRAVERSALORDER.IN_ORDER)
            {
                pd(nCurrent.Data);
            }

            if (nCurrent.Right != null)
            {
                RecIterate(nCurrent.Right, pd, to);
            }

            //Process the current node
            if (to == TRAVERSALORDER.POST_ORDER)
            {
                pd(nCurrent.Data);
            }
        }

        public override void Add(T data)
        {
            if (nRoot == null)
            {
                nRoot = new TreeNode<T>(data);
            }
            else
            {
                RecAdd(data, nRoot);
                nRoot = Balance(nRoot);
            }
        }

        private void RecAdd(T data, TreeNode<T> nCurrent)
        {
            int iResult = data.CompareTo(nCurrent.Data);

            if (iResult < 0)
            {
                if (nCurrent.Left == null)
                {
                    nCurrent.Left = new TreeNode<T>(data);
                }
                else
                {
                    RecAdd(data, nCurrent.Left);
                    //Balance the left
                    nCurrent.Left = Balance(nCurrent.Left);
                }
            }
            else
            {
                if (nCurrent.Right == null)
                {
                    nCurrent.Right = new TreeNode<T>(data);
                }
                else
                {
                    RecAdd(data, nCurrent.Right);
                    //Balance the right node
                    nCurrent.Right = Balance(nCurrent.Right);
                }
            }
        }

        public override void Clear()
        {
            nRoot = null;
            iCount = 0;
        }

        public override bool Remove(T data)
        {
            bool wasRemoved = false;
            nRoot = recRemove(nRoot, data, ref wasRemoved);
            nRoot = Balance(nRoot);
            return wasRemoved;
        }

        private TreeNode<T> recRemove(TreeNode<T> nCurrent, T data, ref bool wasRemoved)
        {
            //variable used for comparing two data items of type T
            int iCompare = 0;
            if (nCurrent != null)
            {
                iCompare = data.CompareTo(nCurrent.Data);
                //if the item to remove is less than currents data
                if (iCompare < 0)
                {
                    //Recursively remove from currents left subtree
                    nCurrent.Left = recRemove(nCurrent.Left, data, ref wasRemoved);
                    nCurrent.Left = Balance(nCurrent.Left);
                }
                else if (iCompare > 0)
                {
                    //Recursively remove from currents right subtree
                    nCurrent.Right = recRemove(nCurrent.Right, data, ref wasRemoved);
                    nCurrent.Right = Balance(nCurrent.Right);
                }
                //node to remove has been found
                else
                {
                    //indicate that we found the item
                    wasRemoved = true;
                    //check to see if the node is a leaf
                    if (nCurrent.IsLeaf())
                    {
                        //set the leaf to null
                        nCurrent = null;
                        //reduce the count
                        iCount--;
                    }
                    //if not a leaf
                    else
                    {
                        if (nCurrent.Left != null)
                        {
                            //replace current data with the substitute, largest in left sub tree
                            nCurrent.Data = RecFindLargest(nCurrent.Left);
                            //remove the substitute from currents left subtree
                            nCurrent.Left = recRemove(nCurrent.Left, nCurrent.Data, ref wasRemoved);
                            nCurrent.Left = Balance(nCurrent.Left);
                        }
                        else
                        {
                            nCurrent.Data = RecFindSmallest(nCurrent.Right);
                            nCurrent.Left = recRemove(nCurrent.Right, nCurrent.Data, ref wasRemoved);
                            nCurrent.Right = Balance(nCurrent.Right);
                        }
                    }
                }
            }

            return nCurrent;
        }

        public override IEnumerator<T> GetEnumerator()
        {
            return new Enumerator(this);
        }
        #endregion

        #region helperMethods
        /// <summary>
        /// Find the smallest data item in the entire BST
        /// </summary>
        /// <returns>A reference to the smallest item</returns>
        public T FindSmallest()
        {
            if (nRoot == null)
            {
                throw new ApplicationException("Root is null");
            }
            else
            {
                return RecFindSmallest(nRoot);
            }
        }

        private T RecFindSmallest(TreeNode<T> current)
        {
            T tReturn = default(T);
            if (current.Left != null)
            {
                tReturn = RecFindSmallest(current.Left);
            }
            else
            {
                tReturn = current.Data;
            }
            return tReturn;
        }
        /// <summary>
        /// Find the largest data item in the entire BST
        /// </summary>
        /// <returns>A reference to the largest item</returns>
        public T FindLargest()
        {
            if (nRoot == null)
            {
                throw new ApplicationException("Root is null");
            }
            else
            {
                return RecFindLargest(nRoot);
            }
        }

        private T RecFindLargest(TreeNode<T> current)
        {
            T tReturn = default(T);
            if (current.Right != null)
            {
                tReturn = RecFindLargest(current.Right);
            }
            else
            {
                tReturn = current.Data;
            }
            return tReturn;
        }
        #endregion

        #region Enumerator Implementation

        private class Enumerator : IEnumerator<T>
        {

            private BinarySearchTree<T> parent = null;
            private TreeNode<T> nCurrent = null;
            private Stack<TreeNode<T>> sNodes = null;

            //Constructor
            public Enumerator(BinarySearchTree<T> parent)
            {
                this.parent = parent;
                Reset();
            }

            public T Current
            {
                get { return nCurrent.Data; }
            }

            public void Dispose()
            {
                parent = null;
                nCurrent = null;
                sNodes = null;
            }

            object System.Collections.IEnumerator.Current
            {
                get { return nCurrent.Data; }
            }

            //Move next moves to the next element by setting nCurrent
            //Returns true if it moved else false.
            public bool MoveNext()
            {
                bool bMoved = false;
                if (sNodes.Count > 0)
                {
                    bMoved = true;
                    nCurrent = sNodes.Pop();
                    if (nCurrent.Right != null)
                    {
                        sNodes.Push(nCurrent.Right);
                    }
                    if (nCurrent.Left != null)
                    {
                        sNodes.Push(nCurrent.Left);
                    }

                }
                return bMoved;
            }

            public void Reset()
            {
                sNodes = new Stack<TreeNode<T>>();
                //Push the root node on the stack
                if (parent.nRoot != null)
                {
                    sNodes.Push(parent.nRoot);
                }
                nCurrent = null;
            }
        }

        #endregion

        #region printAsTree
        public void PrintAsTree()
        {
            Queue<NodeLevel> q = new Queue<NodeLevel>();
            q.Enqueue(new NodeLevel(nRoot, 1));
            int iCurrentLevel = 1;
            int iInterval = 60;


            while (q.Count > 0)
            {

                char[] cArray = new char[240];
                int iIndex = 0;
                int iLevelStartPosition = (int)Math.Pow(2, (iCurrentLevel - 1));
                while (q.Count > 0 && (int)Math.Log(q.Peek().iPosition, 2) + 1 == iCurrentLevel)
                {

                    NodeLevel nl = q.Dequeue();

                    if (nl.n.Left != null)
                        q.Enqueue(new NodeLevel(nl.n.Left, nl.iPosition * 2));

                    if (nl.n.Right != null)
                        q.Enqueue(new NodeLevel(nl.n.Right, nl.iPosition * 2 + 1));


                    //Print the value on this level
                    String sData = nl.n.Data.ToString();
                    char[] cData = sData.ToArray();
                    if (nl.iPosition != iLevelStartPosition)
                    {
                        iIndex = 2 * iInterval * (nl.iPosition - iLevelStartPosition) + iInterval +
                            (nl.iPosition - iLevelStartPosition) * 2;
                    }
                    else
                    {
                        iIndex = iInterval;
                    }

                    Array.Copy(cData, 0, cArray, iIndex, cData.Length);

                }
                //Start a new level

                string s = new string(cArray);
                Console.WriteLine(s + '\n');

                iCurrentLevel++;
                iInterval = (int)(iInterval / 2);

            }
            Console.WriteLine();
        }

        private class NodeLevel
        {
            internal TreeNode<T> n;
            internal int iPosition;

            public NodeLevel(TreeNode<T> n, int iPosition)
            {
                this.n = n;
                this.iPosition = iPosition;
            }
        }
        #endregion
    }
}
