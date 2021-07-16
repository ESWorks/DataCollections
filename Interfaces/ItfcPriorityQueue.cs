using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollections.Interfaces
{
    public interface ItfcPriorityQueue<T> where T : IComparable<T>
    {
        void Enqueue(T data);

        T Dequeue();

        bool IsEmpty();

        String ToString();
    }
}
