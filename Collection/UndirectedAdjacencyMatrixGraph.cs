using DataCollections.Abstract;
using DataCollections.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollections.Collection
{
    public class UndirectedAdjacencyMatrixGraph<T> : AbsAdjacencyMatrixGraph<T> where T : IComparable<T>
    {
        //override some add edge related methods to insert two edges
        public override void AddEdge(T from, T to, double weight)
        {
            base.AddEdge(from, to, weight);
            base.AddEdge(to, from, weight);
        }

        public override void RemoveEdge(T from, T to)
        {
            base.RemoveEdge(from, to);
            base.RemoveEdge(to, from);
        }

        protected override string GetDOTType()
        {
            return "graph";
        }

        protected override string FormatEdgeAsDOT(GraphEdge<T> edge)
        {
            string fromID = "v_" + edge.From.GetHashCode();
            string toID = "v_" + edge.To.GetHashCode();
            return fromID + " -- " + toID + "[label=" + edge.Weight + "]\n";
        }

        public override IEnumerable<GraphEdge<T>> EnumerateEdges()
        {
            //list of edges
            List<GraphEdge<T>> listOfEdges = new List<GraphEdge<T>>();

            //for each from
            for (int from = 0; from < this.edgeMatrix.GetLength(0); from++)
            {
                //for each to
                for (int to = from; to < this.edgeMatrix.GetLength(1); to++)
                {
                    //if current is not null
                    if (edgeMatrix[from, to] != null)
                    {
                        GraphEdge<T> current = this.edgeMatrix[from, to];
                        //add to list
                        listOfEdges.Add(current);
                    }
                }
            }
            //return list of edges
            return listOfEdges;
        }

        public override IEnumerable<GraphVertex<T>> EnumerateNeighborsOfVertexFor(T data)
        {
            //list of edges
            List<GraphVertex<T>> listOfNeighbours = new List<GraphVertex<T>>();

            //for each from
            for (int from = 0; from < this.edgeMatrix.GetLength(0); from++)
            {
                //for each to
                for (int to = from; to < this.edgeMatrix.GetLength(1); to++)
                {
                    //if current is not null
                    if (edgeMatrix[from, to] != null)
                    {
                        GraphEdge<T> current = this.edgeMatrix[from, to];
                        //add to list
                        listOfNeighbours.Add(current.To);
                    }
                }
            }
            //return list of edges
            return listOfNeighbours;
        }
    }
}
