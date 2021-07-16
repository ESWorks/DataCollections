using DataCollections.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollections.Collection
{
    public class LinkedList<T> : AbsList<T> where T : IComparable<T>
    {
        #region Attributes
        private Node head;
        #endregion

        #region Constructors
        public LinkedList()
        {
            head = null;
        }
        #endregion

        #region list

        public override bool Remove(T data)
        {
            return RecRemove(ref head, data);
        }

        private bool RecRemove(ref Node current, T data)
        {
            bool found = false;

            if (current != null)
            {
                if (found = current.data.Equals(data))
                {
                    current = current.next;
                }
                else
                {
                    found = RecRemove(ref current.next, data);
                }
            }
            return found;
        }

        public override void Insert(int index, T data)
        {
            if (index > this.Count)
            {
                throw new IndexOutOfRangeException("Screwed up Insert method");
            }

            head = RecInsert(index, data, head);
        }

        private Node RecInsert(int index, T data, Node current)
        {
            if (index == 0)
            {
                Node newNode = new Node(data, current);
                current = newNode;
            }
            else
            {
                current.next = RecInsert(--index, data, current.next);
            }
            return current;
        }

        public override T RemoveAt(int index)
        {
            if (index > this.Count)
            {
                throw new IndexOutOfRangeException("Screwed up removal.");
            }
            if (index >= 0)
            {
                try
                {
                    return RecRemoveAt(ref head, index);
                }
                catch (IndexOutOfRangeException e)
                {
                    Console.WriteLine("out of bound " + e.Message);
                }
            }
            return default(T);
        }
        private T RecRemoveAt(ref Node Current, int index)
        {

            if (index == 0)
            {
                Node repl = Current.next;
                Current = repl;
                return Current.data;
            }
            else
            {
                return RecRemoveAt(ref Current.next, --index);
            }
        }

        public override T ReplaceAt(int index, T data)
        {
            if (index > this.Count)
            {
                throw new IndexOutOfRangeException("Screwed up replace.");
            }
            if (index >= 0)
            {
                return RecReplaceAt(ref head, index, data);
            }
            return default(T);
        }
        private T RecReplaceAt(ref Node Current, int index, T data)
        {
            if (index == 0)
            {
                Current.data = data;
                return Current.data;
            }
            else
            {
                return RecReplaceAt(ref Current.next, --index, data);
            }
        }
        public override void Add(T data)
        {
            head = RecAdd(head, data);
        }

        private Node RecAdd(Node current, T data)
        {
            //Base case
            if (current == null)
            {
                current = new Node(data);
            }

            //Recursive case
            else
            {
                //Recurse to the next node
                current.next = RecAdd(current.next, data);
            }
            return current;

        }
        public override void Clear()
        {
            head = null;
        }
        //Instantiates an enumerator with a reference to the current list
        public override IEnumerator<T> GetEnumerator()
        {
            return new Enumerator(this);
        }

        #endregion

        #region Node class
        private class Node
        {
            #region Attributes
            //A reference to the next node
            public Node next;
            //A reference to the data stored by the node
            public T data;
            #endregion

            #region Constructors
            //Demonstrates constructor chaining
            public Node(T data)
                : this(data, null)
            {

            }
            public Node(T data, Node next)
            {
                this.data = data;
                this.next = next;
            }
            #endregion
        }
        #endregion

        #region Enumerator
        private class Enumerator : IEnumerator<T>
        {

            //A reference to the linked list
            private LinkedList<T> parent;
            private Node lastVisited;//The current node that we visited
            private Node scout;//the next node to visit

            public Enumerator(LinkedList<T> parent)
            {
                this.parent = parent;
                Reset();
            }

            //Generic version of Current
            public T Current
            {
                get { return lastVisited.data; }
            }

            public void Dispose()
            {
                parent = null;
                scout = null;
                lastVisited = null;
            }

            //Object version of Current
            object System.Collections.IEnumerator.Current
            {
                get { return lastVisited.data; }
            }

            public bool MoveNext()
            {
                bool result = false;
                if (scout != null)
                {
                    result = true;
                    lastVisited = scout;
                    scout = lastVisited.next;

                }
                return result;
            }

            public void Reset()
            {
                //Set the node currently being looked at to null
                lastVisited = null;
                //Set the scout to the head of the list
                scout = parent.head;
            }
        }
        #endregion
    }
}
