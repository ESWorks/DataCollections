using DataCollections.Abstract;
using DataCollections.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollections.Collection.Hashtable
{
    public class LinearHashtable<K, V> : AbsOpenAddressing<K, V>
        where V : IComparable<V>
        where K : IComparable<K>
    {
        public override void Remove(K key)
        {
            bool isFound = false;
            int iAttempt = 1;
            //add
            int iInitialHash = HashFunction(key);
            int iCurrentLocation = iInitialHash;
            //if table has more than one value
            if (this.iCount > 1)
            {
                //loop through collision sequence
                while (oDataArray[iCurrentLocation] != null)
                {
                    KeyValue<K, V> kv = (KeyValue<K, V>)oDataArray[iCurrentLocation];
                    //check if the current keys hash is the same as the initial hash
                    if (HashFunction(kv.Key) == iInitialHash)
                    {
                        //if the same replace the initial space with the value
                        oDataArray[iInitialHash] = oDataArray[iCurrentLocation];
                        //print the replacing key
                        Console.WriteLine("Replaced with " + kv.Key);
                    }
                    iCurrentLocation = iInitialHash + GetIncrement(iAttempt++, key);
                    iCurrentLocation %= HTSize;
                }
            }
            //if the table has only one value, dont replace just remove it
            else
            {
                oDataArray[iInitialHash] = null;
            }
        }

        protected override int GetIncrement(int iAttempt, K Key)
        {
            int iIncrement = 1;
            return iIncrement * iAttempt;
        }

    }
}
