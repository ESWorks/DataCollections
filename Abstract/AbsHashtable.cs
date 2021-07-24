using DataCollections.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollections.Abstract
{
    public abstract class AbsHashtable<K, V> : ItfcHashtable<K, V>
        where V : IComparable<V>
        where K : IComparable<K>
    {
        // in the case of chaining, this array will store ArrayList object references. in addressing + probing, where 
        // array will directly store key value pairs.
        protected object[] oDataArray;

        // store the number of elements in the array
        protected int iCount;

        // determine the load factor used for knowing when to expand the array, used internally don't need props for it.
        protected double dLoadFactor = 0.72;

        // want to maintain amount of collisions for funsies and science, not really part of a hashtable.
        private int iNumCollisions = 0;

        public int INumCollisions
        {
            get => iNumCollisions;
            set => iNumCollisions = value;
        }

        #region Properties

        public int Count
        {
            get { return iCount; }
        }


        public int HTSize // probably doesn't need to be public, but this way we can check it out any time for science.
        {
            get { return oDataArray.Length; }
        }

        

        #endregion

        /// <summary>
        /// Returns an integer value representing the indice into the hash table array.
        /// </summary>
        /// <param name="key">Item that the hash function bases its calculation on</param>
        /// <returns>An int from 0 to one less than the hashtable size</returns>
        protected int HashFunction(K key)
        {
            // doing this takes a big number (up to 4 billion) and map it to a range between 1 and the hashtable size-1 inclusively.
            // gethashcode is built into every c# object (yours or the systems). returns an integer value which we mod by the hashtable
            // size to map the number to the range of the indices of the array. also take the abs value to make sure it is positive.
            return Math.Abs(key.GetHashCode() % HTSize);
        }



        // all of these methods rely on some implemented find method -- can't do here. also
        // because the find methods are different dependant on how it's being used in the implementation.
        public abstract V Get(K key);

        public abstract void Add(K key, V value);

        public abstract void Remove(K key);

        // do this
        public void Clear()
        {
            oDataArray = new object[0];
        }

        public abstract IEnumerator<V> GetEnumerator();

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        
    }
}
