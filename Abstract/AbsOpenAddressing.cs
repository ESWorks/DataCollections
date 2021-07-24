using DataCollections.Classes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollections.Abstract
{
    public abstract class AbsOpenAddressing<K, V> : AbsHashtable<K, V>, IEnumerable, IEnumerator<V>
        where V : IComparable<V>
        where K : IComparable<K>
        
    {
        protected abstract int GetIncrement(int iAttempt, K Key);

        readonly PrimeNumber pm = new();


        public AbsOpenAddressing()
        {
            oDataArray = new object[pm.GetNextPrime()];
        }

        public override V Get(K key)
        {
            V val = default(V);
            bool isFound = false;
            int iAttempt = 1;
            //add
            int iInitialHash = HashFunction(key);
            //the current location
            int iCurrentLocation = iInitialHash;
            //loop through collision sequence
            while (oDataArray[iCurrentLocation] != null && !isFound)
            {
                if (oDataArray[iCurrentLocation].GetType() == typeof(KeyValue<K, V>))
                {
                    KeyValue<K, V> kv = (KeyValue<K, V>)oDataArray[iCurrentLocation];
                    if (kv.Key.CompareTo(key) == 0)
                    {
                        val = kv.Value;
                        isFound = true;
                    }
                }
                iCurrentLocation = iInitialHash + GetIncrement(iAttempt++, key);
                iCurrentLocation %= HTSize;
            }
            if (!isFound)
            {
                Console.WriteLine("No Value Found!");
            }
            return val;
        }
        public override void Add(K key, V value)
        {

            //number of attempts you have made
            int iAttempt = 1;
            //Get the initial hash position
            int iInitialHash = HashFunction(key);

            //current location we are checking
            int iCurrentLocation = iInitialHash;

            //Position we add at
            int iPositionToAdd = -1;

            //Boolean to help track collisions
            Boolean bCollided = false;

            //loop through the current collision sequence if it exists
            while (oDataArray[iCurrentLocation] != null)
            {
                //if the current value is a key, value pair
                if (oDataArray[iCurrentLocation].GetType() == typeof(KeyValue<K, V>))
                {
                    //compare the current value with the value to add
                    KeyValue<K, V> kv = (KeyValue<K, V>)oDataArray[iCurrentLocation];

                    if (kv.Key.CompareTo(key) == 0)
                    {
                        throw new ApplicationException("Error Error, Item already Exists *BOOM*");
                    }
                }
                //else if it is a tombstone
                else
                {
                    //if it is the first tombstone, mark it's location
                    if (iPositionToAdd == -1)
                    {
                        iPositionToAdd = iCurrentLocation;
                    }
                }
                //move to the next location
                iCurrentLocation = iInitialHash + GetIncrement(iAttempt++, key);
                //Wrap around to the start of the table if at the end
                iCurrentLocation %= HTSize;
                if (!bCollided)
                {
                    INumCollisions++;
                    bCollided = true;
                }
            }
            //we are at a null location
            //did we find a tombstone
            if (iPositionToAdd == -1)
            {
                //set the position to add at the end of the collision sequence
                iPositionToAdd = iCurrentLocation;
            }
            //add the item
            oDataArray[iPositionToAdd] = new KeyValue<K, V>(key, value);
            iCount++;

            //check to see if we need to expand
            if (IsOverLoaded())
            {
                ExpandHashTable();
            }
        }


        private void ExpandHashTable()
        {
            object[] oldArray = oDataArray;
            oDataArray = new object[pm.GetNextPrime()];
            iCount = 0;
            INumCollisions = 0;

            for (int i = 0; i < oldArray.Length; i++)
            {
                if (oldArray[i] != null)
                {
                    if (oldArray[i].GetType() == typeof(KeyValue<K, V>))
                    {
                        KeyValue<K, V> kv = (KeyValue<K, V>)oldArray[i];
                        Add(kv.Key, kv.Value);

                    }
                }

            }
        }

        private bool IsOverLoaded()
        {
            return iCount / (double)HTSize > dLoadFactor;
        }

        public override void Remove(K key)
        {
            bool isFound = false;
            int iAttempt = 1;
            //add
            int iInitialHash = HashFunction(key);
            //the current location
            int iCurrentLocation = iInitialHash;
            //loop through collision sequence
            while (oDataArray[iCurrentLocation] != null && !isFound)
            {
                if (oDataArray[iCurrentLocation].GetType() == typeof(KeyValue<K, V>))
                {
                    KeyValue<K, V> kv = (KeyValue<K, V>)oDataArray[iCurrentLocation];
                    if (kv.Key.CompareTo(key) == 0)
                    {
                        oDataArray[iCurrentLocation] = new Tombstone();
                        isFound = true;
                        Console.WriteLine("{" + iCurrentLocation + "} Object was removed.");
                    }
                }
                iCurrentLocation = iInitialHash + GetIncrement(iAttempt++, key);
                iCurrentLocation %= HTSize;
            }
            if (!isFound)
            {
                Console.WriteLine("No Value Found!");
            }
        }

        public override IEnumerator<V> GetEnumerator()
        {
            return (IEnumerator<V>)this;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < oDataArray.Length; i++)
            {
                sb.Append("Bucket " + i + ": ");
                if (oDataArray[i] != null)
                {
                    if (oDataArray[i].GetType() == typeof(Tombstone))
                    {
                        sb.Append("Tombstone");
                    }
                    else
                    {
                        KeyValue<K, V> kv = (KeyValue<K, V>)oDataArray[i];
                        sb.Append(kv.Value.ToString());
                    }

                }
                sb.Append("\n");
            }
            return sb.ToString();
        }



        #region IEnumerable

        protected int position = -1;

        public virtual object Current => (oDataArray[position] as KeyValue<K,V>);

        V IEnumerator<V>.Current => (oDataArray[position] as KeyValue<K, V>).Value;

        public virtual bool MoveNext()
        {
            position++;
            return (position < oDataArray.Length);
        }

        public virtual void Reset()
        {
            position = 0;
        }

        public void Dispose()
        {
            position = -1;
        }
        #endregion
    }
}
