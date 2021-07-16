using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollections.Classes
{
    public class GraphVertex<T> : IComparable<GraphVertex<T>> where T : IComparable<T>
    {
        public T Data { get; private set; }


        public GraphVertex(T data)
        {
            Data = data;
        }

        public int CompareTo(GraphVertex<T> other)
        {
            return Data.CompareTo(other.Data);
        }

        public override string ToString()
        {
            return Data.ToString();
        }
    }
}
