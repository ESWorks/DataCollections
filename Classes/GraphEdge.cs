using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollections.Classes
{
    public class GraphEdge<T> : IComparable<GraphEdge<T>> where T : IComparable<T>
    {
        
        public GraphVertex<T> To { get; private set; }
        
        public GraphVertex<T> From { get; private set; }

        public double Weight { get; private set; }


        public GraphEdge(GraphVertex<T> from, GraphVertex<T> to, double weight)
        {
            To = to;
            From = from;
            Weight = weight;
        }

        public GraphEdge(GraphVertex<T> from, GraphVertex<T> to) : this(from, to, double.PositiveInfinity) { }
        

        public override string ToString()
        {
            return this.From.ToString() + "->" + Weight.ToString() + "->" + To.ToString();
        }

        public int CompareTo(GraphEdge<T> other)
        {
            int value = Weight.CompareTo(other.Weight);
            if (value == 0)
            {
                value = To.CompareTo(other.To);
                if (value == 0)
                {
                    value = To.CompareTo(other.To);
                }

            }
            return value;
        }
    }
}
