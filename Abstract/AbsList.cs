using DataCollections.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollections.Abstract
{
    public abstract class AbsList<T> : AbsCollection<T>, ItfcList<T> where T : IComparable<T>
    {
        public T ElementAt(int index)
        {
            int count = 0;
            //Get an enumerator
            IEnumerator<T> myEnum = GetEnumerator();
            //Reset the enumerator
            myEnum.Reset();

            //while there are more data items to check AND not at the correct index
            while (myEnum.MoveNext() && count != index)
            {
                //increment index counter
                count++;
            }
            //If the count is beyond the index of the collection
            if (count != index)
            {
                //Throw an indexOutofRange exception
                throw new IndexOutOfRangeException("Invalid index" + index);
            }
            //Throw an indexOutofRange exception

            //Return the enumerators current data item.
            return myEnum.Current;
        }

        public int IndexOf(T data)
        {
            // set index
            int index = 0;
            //Get an enumerator
            IEnumerator<T> myEnum = GetEnumerator();
            //Reset the enumerator
            myEnum.Reset();

            do
            {
                if(myEnum.Current.Equals(data))
                {
                    return index;
                }
                index++;
            } while (myEnum.MoveNext());

            // no comparable data return -1
            return -1;
        }

        #region
        public abstract void Insert(int index, T data);

        public abstract T RemoveAt(int index);

        public abstract T ReplaceAt(int index, T data);
        #endregion
    }
}
