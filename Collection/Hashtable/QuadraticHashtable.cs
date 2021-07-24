using DataCollections.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollections.Collection.Hashtable
{
    public class QuadraticHashtable<K, V> : AbsOpenAddressing<K, V>
        where V : IComparable<V>
        where K: IComparable<K>
    {

        protected override int GetIncrement(int iAttempt, K Key)
    {
        double c1 = 0.5;
        double c2 = 0.5;

        return (int)(c1 * iAttempt + c2 * Math.Pow(iAttempt, 2)) % HTSize;
    }

    public QuadraticHashtable()
    {
        this.dLoadFactor = 0.5;
    }
}
}
