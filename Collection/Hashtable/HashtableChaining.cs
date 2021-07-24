using DataCollections.Abstract;
using DataCollections.Classes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollections.Collection.Hashtable
{
    public class HashtableChaining<K, V> : AbsHashtable<K, V>, IEnumerable, IEnumerator<V>
        where V : IComparable<V>
        where K : IComparable<K>
    {

        public override V Get(K key)
        {

            V vReturn = default(V);
            try
            {
                // set current kv and greater kv to defaults
                KeyValue<K, V> kv = new KeyValue<K, V>(default(K), default(V));
                KeyValue<K, V> greaterKV = new KeyValue<K, V>(default(K), default(V));
                KeyValue<K, V> returnKV = new KeyValue<K, V>(default(K), default(V));
                // arraylist for current key
                ArrayList al = (ArrayList)oDataArray[HashFunction(key)];
                int i = 0;
                if (al != null)
                {
                    // get index of key
                    i = al.IndexOf(new KeyValue<K, V>(key, default(V)));
                    kv = (KeyValue<K, V>)al[i];
                    // increment priority
                    kv.Priority++;
                    // returns the current kv may have be changed
                    // if its greater
                    // we set return to kv
                    returnKV = kv;
                    // loop backwards from current position in array
                    for (int j = i; j >= 0; j--)
                    {
                        greaterKV = (KeyValue<K, V>)al[j];
                        if (kv.Priority > greaterKV.Priority)
                        {
                            //swap Key Values if its greater
                            // old kv value
                            KeyValue<K, V> temp = kv;
                            // current kv becomes older greater value
                            kv = greaterKV;
                            // greater value becomes temp (current value)
                            greaterKV = temp;
                        }
                    }
                    //returns the current kv may have been changed
                    // we return the returnKV
                    vReturn = returnKV.Value;
                }
                if (al == null || i == -1)
                {
                    Console.WriteLine("Key does not exist!!! |OR| Was Removed :: Bucket Count :" + iBucketCount);
                }
            }
            catch (ArgumentOutOfRangeException ex) { Console.WriteLine("Key does not exist!!! |OR| Was Removed :: Bucket Count :" + iBucketCount); }


            return vReturn;
        }

        private void ExpandHashTable()
        {
            object[] oldArray = oDataArray;
            oDataArray = new object[HTSize * 2];
            iCount = 0;
            INumCollisions = 0;
            for (int i = 0; i < oldArray.Length; i++)
            {
                if (oldArray[i] != null)
                {
                    ArrayList ar = (ArrayList)oldArray[i];
                    foreach (KeyValue<K, V> kv in ar)
                    {
                        kv.Priority = 0;
                        Add(kv.Key, kv.Value);
                    }
                }

            }
        }
        private void shrinkHashTable()
        {
            object[] oldArray = oDataArray;
            int ShrinkSize = (int)(oDataArray.Length / dLoadFactor);
            oDataArray = new object[ShrinkSize];

            for (int i = 0; i < oldArray.Length; i++)
            {
                if (oldArray[i] != null)
                {
                    ArrayList ar = (ArrayList)oldArray[i];
                    foreach (KeyValue<K, V> kv in ar)
                    {
                        kv.Priority = 0;
                        Add(kv.Key, kv.Value);
                    }
                }

            }
        }

        #region non-assignment methods
        // intial size for oDataArray
        private const int iInitialSize = 4;

        // how many buckets (locations in the array) are occupied by ArrayLists
        // not the same as count, becuase we need to know the amount of arraylists
        // to calculate our current load factor. then we can use 
        // bucketcount / htsize > loadfactor to increase
        private int iBucketCount = 0;

        public HashtableChaining()
        {
            // set up the data array
            this.oDataArray = new object[iInitialSize];

        }

        public HashtableChaining(int iInitialSize, double dLoadFactor)
        {
            this.oDataArray = new object[iInitialSize];
            this.dLoadFactor = dLoadFactor;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            //Loop through each bucket
            for (int i = 0; i < HTSize; i++)
            {
                //Add the bucket number to the string
                sb.Append("Bucket " + i.ToString() + ": ");
                //check if an arraylist exists at this location
                if (oDataArray[i] != null)
                {
                    ArrayList alCurrent = (ArrayList)oDataArray[i];
                    foreach (KeyValue<K, V> kv in alCurrent)
                    {
                        sb.Append(kv.Value.ToString() + " => ");
                    }
                    sb.Remove(sb.Length - 5, 5);
                }
                sb.Append("\n");

            }
            return sb.ToString();
        }



        public override void Add(K key, V value)
        {
            //hashcode <-- hash of the key 
            //create the key value pair object
            //if the hashcode location of the key is null
            // create an arraylist and store it's reference at that location
            // increment a bucket count
            //else
            // increment the number of collisions
            //get a reference to the arraylist at the hashed location
            //if the arraylist contains the current kv pair
            // throw an exception that the key exists already
            //else
            // add the kv value to the arraylist
            // increment the count



            KeyValue<K, V> kv = new KeyValue<K, V>(key, value);
            int hashCode = HashFunction(key);
            if (oDataArray[hashCode] == null)
            {
                oDataArray[hashCode] = new ArrayList();
                iBucketCount++;
            }
            else
            {
                INumCollisions++;
            }
            ArrayList list = (ArrayList)oDataArray[hashCode];
            if (list.Contains(kv))
            {
                throw new Exception("That key/value exists");
            }
            else
            {
                list.Add(kv);
                iCount++;
            }
            if (IsOverLoaded())
            {
                ExpandHashTable();
            }


        }
        /// <summary>
        /// expands the hash table by a factor of two
        /// 
        /// note that each element is rehased into the new array. the reason is that HTSize is 
        /// involved in adding, thus changes how the items are hashed into the new array.
        /// </summary>


        private bool IsOverLoaded()
        {
            return (double)iBucketCount / HTSize > dLoadFactor ? true : false;
        }

        public override void Remove(K key)
        {
            Console.WriteLine("Key Will Be Removed::Bucket Count\r\n" +
                                iBucketCount);
            int hashCode = HashFunction(key);
            if (oDataArray[hashCode] != null)
            {
                oDataArray[HashFunction(key)] = new ArrayList();
                iBucketCount--;
                Console.WriteLine("Key Was Removed::Bucket Count\r\n" +
                    iBucketCount);
                shrinkHashTable();
            }
            else
            {
                Console.WriteLine("Key does not exist!!!::Bucket Count\r\n" +
                                    iBucketCount);
            }
        }



        public override IEnumerator<V> GetEnumerator() => (IEnumerator<V>)this;
        #endregion

        #region IEnumerable

        protected int positionKey = -1;

        protected int arrayIndex = -1;

        V IEnumerator<V>.Current => ((oDataArray[positionKey] as ArrayList)[arrayIndex] as KeyValue<K, V>).Value;

        public object Current => ((oDataArray[positionKey] as ArrayList)[arrayIndex] as KeyValue<K, V>);

        public bool MoveNext()
        {
            if (positionKey < 0 || arrayIndex >= (oDataArray[positionKey] as ArrayList).Count - 1)
            {
                arrayIndex = -1;
                do
                {
                    positionKey++;
                    if (positionKey >= oDataArray.Length) return false;
                } while (oDataArray[positionKey] == null);
            }

            arrayIndex++;

            return (positionKey < oDataArray.Length);
        }

        public void Reset()
        {
            positionKey = 0;
            arrayIndex = 0;
        }

        public void Dispose()
        {
            positionKey = -1;
            arrayIndex = -1;
        }

        #endregion
    }
}

