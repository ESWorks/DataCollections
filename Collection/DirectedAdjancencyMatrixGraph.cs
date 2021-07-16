using DataCollections.Abstract;
using DataCollections.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollections.Collection
{
    public class DirectedAdjacencyMatrixGraph<T> : AbsAdjacencyMatrixGraph<T> where T : IComparable<T>
    {
        protected override string GetDOTType()
        {
            return "digraph";
        }

        protected override string FormatEdgeAsDOT(GraphEdge<T> edge)
        {
            string fromID = "v_" + edge.From.GetHashCode();
            string toID = "v_" + edge.To.GetHashCode();
            return fromID + " -> " + toID + "[label=" + edge.Weight + "]\n";
        }

        public override IEnumerable<GraphEdge<T>> EnumerateEdges()
        {
            //list of edges
            List<GraphEdge<T>> listOfEdges = new List<GraphEdge<T>>();

            //for each from
            for (int from = 0; from < this.edgeMatrix.GetLength(0); from++)
            {
                //for each to
                for (int to = 0; to < this.edgeMatrix.GetLength(1); to++)
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
            if (!this.HasVertexFor(data))
            {
                throw new ApplicationException("No vertex for " + data.ToString());
            }

            //list of edges
            List<GraphVertex<T>> listOfNeighbours = new List<GraphVertex<T>>();

            int fromIndex = this.vertexIndices[data];

            //for each to
            for (int to = 0; to < this.edgeMatrix.GetLength(1); to++)
            {
                //if current is not null
                if (edgeMatrix[fromIndex, to] != null)
                {
                    GraphEdge<T> current = this.edgeMatrix[fromIndex, to];
                    //add to list
                    listOfNeighbours.Add(current.To);
                }
            }
            //return list of edges
            return listOfNeighbours;
        }
    }
}
