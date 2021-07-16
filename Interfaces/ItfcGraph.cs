using DataCollections.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollections.Interfaces
{
    public delegate void VisitorDelegate<T>(T data);

    public interface ItfcGraph<T> where T : IComparable<T>
    {
        #region Vertex related methods
        void AddVertexFor(T data);

        bool HasVertexFor(T data);

        void RemoveVertexFor(T data);

        GraphVertex<T> GetVertexFor(T data);

        IEnumerable<GraphVertex<T>> EnumerateVertices();

        IEnumerable<GraphVertex<T>> EnumerateNeighborsOfVertexFor(T data);

        #endregion

        #region Edge Related Methods
        void AddEdge(T from, T to);

        void AddEdge(T from, T to, double weight);

        bool HasEdge(T from, T to);

        void RemoveEdge(T from, T to);

        GraphEdge<T> GetEdge(T from, T to);

        IEnumerable<GraphEdge<T>> EnumerateEdges();

        #endregion

        #region Graph Operations
        void DepthFirstTraversal(T whereToStart, VisitorDelegate<T> whatToDo);

        void BreadthFirstTraversal(T whereToStart, VisitorDelegate<T> whatToDo);

        ItfcGraph<T> MinimumSpanningTree();

        ItfcGraph<T> ShortestWeightedPath(T whereToStart, T whereToEnd);
        #endregion
    }
}
