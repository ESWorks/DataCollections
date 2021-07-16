using DataCollections.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollections.Abstract
{
    public abstract class AbsCollection<T> : ItfcCollection<T> where T : IComparable<T>
    {
        #region Abstract Methods
        public abstract void Add(T data);
        public abstract void Clear();
        public abstract bool Remove(T data);
        #endregion

        #region Implemented Methods
        public bool Contains(T data)
        {
            bool found = false;
            IEnumerator<T> myEnum = GetEnumerator();
            myEnum.Reset();
            while (!found && myEnum.MoveNext())
            {
                found = myEnum.Current.Equals(data);
            }
            
            return found;
        }

        public virtual int Count
        {
            get
            {
                int count = 0;
                foreach (T item in this)
                {
                    count++;
                }
                return count;
            }
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder("[");
            string sep = ", ";
            foreach (T item in this)
            {
                result.Append(item + sep);
            }
            if (Count > 0)
            {
                result.Remove(result.Length - sep.Length, sep.Length);
            }
            result.Append("]");
            return result.ToString();
        }
        #endregion

        #region IEnumerable implementation
        public abstract IEnumerator<T> GetEnumerator();

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion
    }
}
