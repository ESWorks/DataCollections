using DataCollections.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollections.Collection
{
    public class SimplePriorityQueue<T> : ItfcPriorityQueue<T> where T : IComparable<T>
    {
        private List<T> items;
        public SimplePriorityQueue()
        {
            this.items = new List<T>();
        }

        public void Enqueue(T data)
        {
            this.items.Add(data);
        }

        public T Dequeue()
        {
            this.items.Sort();

            T toReturn = items[0];

            this.items.RemoveAt(0);

            return toReturn;
        }

        public bool IsEmpty()
        {
            return this.items.Count < 1;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("[");
            foreach (T item in this.items)
            {
                builder.Append("{");
                builder.Append(item.ToString());
                builder.Append("},");
            }
            builder.Append("]");
            return builder.ToString();
        }
    }
}
