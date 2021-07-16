using DataCollections.Classes;
using DataCollections.Collection;
using DataCollections.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollections.Abstract
{
    public abstract class AbsAdjacencyMatrixGraph<T> : ItfcGraph<T> where T : IComparable<T>
    {
        //Some way to store edged
        protected GraphEdge<T>[,] edgeMatrix;

        protected Dictionary<T, int> vertexIndices;
        //Some way to store vertices
        protected List<GraphVertex<T>> vertices;

        public AbsAdjacencyMatrixGraph()
        {
            this.edgeMatrix = new GraphEdge<T>[0, 0];

            this.vertexIndices = new Dictionary<T, int>();

            this.vertices = new List<GraphVertex<T>>();
        }

        public void AddVertexFor(T data)
        {
            //if this data is already in the graph
            if (this.HasVertexFor(data))
            {
                throw new ApplicationException("Vertex for data " + data.ToString() + " already exists");
            }
            GraphVertex<T> newVertex = new GraphVertex<T>(data);
            this.vertices.Add(newVertex);

            this.AddVertexAdjustEdgeStorage(newVertex);
        }

        protected void AddVertexAdjustEdgeStorage(GraphVertex<T> vertex)
        {
            //Map between the data we're storing and the index at which we will find it in the other two data structures
            GraphEdge<T>[,] newEdgeMatrix = new GraphEdge<T>[this.edgeMatrix.GetLength(0) + 1, this.edgeMatrix.GetLength(1) + 1];

            //Increase the size of the edge matrix
            for (int from = 0; from < this.edgeMatrix.GetLength(0); from++)
            {
                for (int to = 0; to < this.edgeMatrix.GetLength(1); to++)
                {
                    newEdgeMatrix[from, to] = this.edgeMatrix[from, to];
                }
            }

            this.edgeMatrix = newEdgeMatrix;

            this.vertexIndices.Add(vertex.Data, this.edgeMatrix.GetLength(0) - 1);
            //Create a larger 2d array
            //this.edgeMatrix = new Edge<T>[0, 0];

            //copy values from old to new

            //replace the old array

            //new size is old + 1

        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Vertices:\n");
            foreach (GraphVertex<T> v in this.vertices)
            {
                sb.Append(this.vertexIndices[v.Data] + ": " + v.ToString() + "\n");
            }




            sb.Append("\n\nEdge matrix (from in rows, to in columns)\n\t");
            for (int i = 0; i < this.edgeMatrix.GetLength(1); i++)
            {
                sb.Append(i + ":\t");
            }
            sb.Append("\n");
            for (int from = 0; from < this.edgeMatrix.GetLength(0); from++)
            {
                sb.Append(from + ":\t");
                for (int to = 0; to < this.edgeMatrix.GetLength(1); to++)
                {
                    GraphEdge<T> currentEdge = this.edgeMatrix[from, to];
                    if (currentEdge != null)
                    {
                        sb.Append("(" + currentEdge.From + "," + currentEdge.To);
                        if (currentEdge.Weight != Double.PositiveInfinity)
                        {
                            sb.Append("," + currentEdge.Weight);
                        }
                        sb.Append(")\t");
                    }
                    else
                    {
                        sb.Append("\t");
                    }
                }
                sb.Append("\n");
            }
            return sb.ToString();
        }

        /// <summary>
        /// Generate DOT to display the graph, highlighting the important sections which
        /// apply while performing a traversal
        /// </summary>
        /// <param name="currentlyVisiting">The node we're currently visiting</param>
        /// <param name="currentlyInspecting">The neighbor of current we're inspecting</param>
        /// <param name="visitedVertices">The list of all previously visited vertices</param>
        /// <param name="toVisit">The list of neighbors of previously visited vertices</param>
        /// <returns></returns>
        public string ToTraversalDOT(GraphVertex<T> currentlyVisiting, GraphVertex<T> currentlyInspecting, List<GraphVertex<T>> visitedVertices, List<GraphVertex<T>> toVisit)
        {
            IEnumerable<GraphEdge<T>> edges = this.EnumerateEdges();
            IEnumerable<GraphVertex<T>> vertices = this.EnumerateVertices();
            StringBuilder builder = new StringBuilder();
            builder.Append(this.GetDOTType() + " cstgraph { \n");

            foreach (GraphVertex<T> currentVertex in vertices)
            {
                builder.Append("v_" + currentVertex.GetHashCode() + " [label=\"" + currentVertex.ToString());
                List<string> fillColours = new List<string>();
                if (currentVertex.CompareTo(currentlyVisiting) == 0)
                {
                    fillColours.Add("yellow");
                    //builder.Append(" color=yellow style=filled");
                }
                if (visitedVertices.Contains(currentVertex))
                {
                    fillColours.Add("red");
                    //builder.Append(" color=red style=filled");
                }
                if (toVisit.Contains(currentVertex))
                {
                    builder.Append(":" + toVisit.IndexOf(currentVertex));
                    fillColours.Add("green");
                    //builder.Append(" color=green style=filled");
                }
                if (currentlyInspecting != null && currentVertex.CompareTo(currentlyInspecting) == 0)
                {
                    if (fillColours.Count < 1)
                    {
                        fillColours.Add("white");
                    }
                    fillColours.Add("blue");
                    //builder.Append(" color=yellow style=filled");
                }
                if (fillColours.Count < 1)
                {
                    fillColours.Add("white");
                }
                builder.Append("\" fillcolor=\"" + string.Join(":", fillColours.ToArray()) + "\" style=\"filled\" gradientangle=\"0\"");

                builder.Append("]\n");
            }

            foreach (GraphEdge<T> currentEdge in edges)
            {
                builder.Append(this.FormatEdgeAsDOT(currentEdge));
            }
            builder.Append("}");
            return builder.ToString();

        }

        public bool HasVertexFor(T data)
        {
            return this.vertexIndices.ContainsKey(data);
        }

        public void RemoveVertexFor(T data)
        {
            //if this data is already in the graph
            if (this.HasVertexFor(data))
            {
                throw new ApplicationException("Vertex for data " + data.ToString() + " already exists");
            }

            int index = vertexIndices[data];

            GraphVertex<T> vertex = vertices[index];

            this.vertices.Remove(vertex);

            this.vertexIndices.Remove(data);

            foreach (T key in vertexIndices.Where(K => K.Value > index).Select(K=>K.Key))
            {
                vertexIndices[key]--;
            }

            this.RemoveVertexAdjustEdgeStorage(index);
        }

        protected void RemoveVertexAdjustEdgeStorage(int removed)
        {
            //Map between the data we're storing and the index at which we will find it in the other two data structures
            GraphEdge<T>[,] newEdgeMatrix = new GraphEdge<T>[this.edgeMatrix.GetLength(0) - 1, this.edgeMatrix.GetLength(1) - 1];

            //Decrease the size of the edge matrix
            for (int from = 0; from < this.edgeMatrix.GetLength(0); from++)
            {
                if (from != removed) 
                { 
                    int adjustedFrom = from > removed ? from - 1 : from;
                    for (int to = 0; to < this.edgeMatrix.GetLength(1); to++)
                    {
                        if (to != removed)
                        {
                            int adjustedTo = to > removed ? to - 1 : to;
                            newEdgeMatrix[adjustedFrom, adjustedTo] = this.edgeMatrix[from, to];
                        }
                    }
                }
            }

            this.edgeMatrix = newEdgeMatrix;

        }

        public GraphVertex<T> GetVertexFor(T data)
        {
            if (!this.HasVertexFor(data))
            {
                throw new ApplicationException("No vertex for data " + data.ToString());
            }

            //Find the index for the given data
            int index = this.vertexIndices[data];

            //return the vertex at that position
            return this.vertices[index];
        }

        public IEnumerable<GraphVertex<T>> EnumerateVertices()
        {
            return this.vertices;
        }

        public abstract IEnumerable<GraphVertex<T>> EnumerateNeighborsOfVertexFor(T data);

        protected abstract string GetDOTType();


        // DOT represents edges on directed and undirected graphs differently
        protected abstract string FormatEdgeAsDOT(GraphEdge<T> edge);

        /// <summary>
        /// Format the graph using DOT syntax.
        /// DOT syntax consists of a list of vertex identifiers
        /// then a list of edges between those vertices
        /// http://en.wikipedia.org/wiki/DOT_(graph_description_language)
        /// http://www.webgraphviz.com/
        /// </summary>
        /// <returns></returns>
        public string ToDOT()
        {
            IEnumerable<GraphEdge<T>> edges = this.EnumerateEdges();
            IEnumerable<GraphVertex<T>> vertices = this.EnumerateVertices();
            StringBuilder builder = new StringBuilder();
            builder.Append(this.GetDOTType() + " cstgraph { \n");

            foreach (GraphVertex<T> currentVertex in vertices)
            {
                builder.Append("v_" + currentVertex.GetHashCode() + " [label=\"" + currentVertex.ToString() + "\"]\n");
            }

            foreach (GraphEdge<T> currentEdge in edges)
            {
                builder.Append(this.FormatEdgeAsDOT(currentEdge));
            }
            builder.Append("}");
            return builder.ToString();
        }

        public void AddEdge(T from, T to)
        {
            this.AddEdge(from, to, Double.PositiveInfinity);
        }

        public virtual void AddEdge(T from, T to, double weight)
        {
            //check to see if the vertices exist
            if (!HasVertexFor(from) || !HasVertexFor(to))
            {
                throw new ApplicationException("No vertex for " + from.ToString() + " or " + to.ToString());
            }

            //check to see if there is an exisisting edge
            if (HasEdge(from, to))
            {
                throw new ApplicationException("The edge already exists!");
            }

            GraphVertex<T> fromVertex = GetVertexFor(from);
            GraphVertex<T> toVertex = GetVertexFor(to);

            GraphEdge<T> newEdge = new GraphEdge<T>(fromVertex, toVertex, weight);

            addEdge(newEdge);
        }

        protected void addEdge(GraphEdge<T> edge)
        {
            int fromIndex = this.vertexIndices[edge.From.Data];
            int toIndex = this.vertexIndices[edge.To.Data];

            this.edgeMatrix[fromIndex, toIndex] = edge;
        }

        public bool HasEdge(T from, T to)
        {
            if (!this.HasVertexFor(from))
            {
                throw new ApplicationException("No vertex for " + from.ToString());
            }
            if (!this.HasVertexFor(to))
            {
                throw new ApplicationException("No vertex for " + to.ToString());
            }

            int fromIndex = this.vertexIndices[from];
            int toIndex = this.vertexIndices[to];

            return this.edgeMatrix[fromIndex, toIndex] != null;
        }

        public virtual void RemoveEdge(T from, T to)
        {
            if (!this.HasVertexFor(from))
            {
                throw new ApplicationException("No vertex for " + from.ToString());
            }
            if (!this.HasVertexFor(to))
            {
                throw new ApplicationException("No vertex for " + to.ToString());
            }

            int fromIndex = this.vertexIndices[from];
            int toIndex = this.vertexIndices[to];

            this.edgeMatrix[fromIndex, toIndex] = null;
        }

        public GraphEdge<T> GetEdge(T from, T to)
        {
            if (!this.HasEdge(from, to))
            {
                throw new ApplicationException("No edge between " + from.ToString() + " and " + to.ToString());
            }

            int fromIndex = this.vertexIndices[from];
            int toIndex = this.vertexIndices[to];

            return this.edgeMatrix[fromIndex, toIndex];
        }

        public abstract IEnumerable<GraphEdge<T>> EnumerateEdges();

        public void DepthFirstTraversal(T whereToStart, VisitorDelegate<T> whatToDo)
        {
            //Initialize our data structures
            //visitedVertices
            List<GraphVertex<T>> visitedVertices = new List<GraphVertex<T>>();
            //verticesToVisit
            Stack<GraphVertex<T>> verticesToVisit = new Stack<GraphVertex<T>>();

            //get the vertex for where to start
            GraphVertex<T> startVertex = this.GetVertexFor(whereToStart);

            //add vertex to ToVisit
            verticesToVisit.Push(startVertex);

            //while there are more vertices in to visit
            while (verticesToVisit.Count > 0)
            {
                //Get the next vertex from toVisit (In a DFT, this is the most recently added vertex)
                GraphVertex<T> currentVertex = verticesToVisit.Pop();
                //has this vertex been visited?
                //if not
                if (!visitedVertices.Contains(currentVertex))
                {
                    //visit the vertex
                    whatToDo(currentVertex.Data);
                    //mark as visited
                    visitedVertices.Add(currentVertex);
                    //Get the neighbours of the current vertex
                    foreach (GraphVertex<T> neighbor in this.EnumerateNeighborsOfVertexFor(currentVertex.Data))
                    {
                        //if the current neighbour has not been visited and is not in ToVisit
                        if (!visitedVertices.Contains(neighbor) && !verticesToVisit.Contains(neighbor))
                        {
                            //add it to the list of vertices to visit
                            verticesToVisit.Push(neighbor);
                        }
                    }
                }
            }
        }

        public void BreadthFirstTraversal(T whereToStart, VisitorDelegate<T> whatToDo)
        {
            //Initialize our data structures
            //visitedVertices
            List<GraphVertex<T>> visitedVertices = new List<GraphVertex<T>>();
            //verticesToVisit
            Queue<GraphVertex<T>> verticesToVisit = new Queue<GraphVertex<T>>();

            //get the vertex for where to start
            GraphVertex<T> startVertex = this.GetVertexFor(whereToStart);

            //add vertex to ToVisit
            verticesToVisit.Enqueue(startVertex);

            //while there are more vertices in to visit
            while (verticesToVisit.Count > 0)
            {
                //Get the next vertex from toVisit (In a DFT, this is the most recently added vertex)
                GraphVertex<T> currentVertex = verticesToVisit.Dequeue();
                //has this vertex been visited?
                //if not
                if (!visitedVertices.Contains(currentVertex))
                {
                    //visit the vertex
                    whatToDo(currentVertex.Data);
                    //mark as visited
                    visitedVertices.Add(currentVertex);
                    //Get the neighbours of the current vertex
                    foreach (GraphVertex<T> neighbor in this.EnumerateNeighborsOfVertexFor(currentVertex.Data))
                    {
                        //if the current neighbour has not been visited and is not in ToVisit
                        if (!visitedVertices.Contains(neighbor) && !verticesToVisit.Contains(neighbor))
                        {
                            //add it to the list of vertices to visit
                            verticesToVisit.Enqueue(neighbor);
                        }
                    }
                }
            }
        }

        public ItfcGraph<T> MinimumSpanningTree()
        {
            throw new NotImplementedException();
        }

        protected internal class VertexData : IComparable<VertexData>
        {
            public double tentative_distance;
            public GraphVertex<T> vertex;
            public bool visited;
            public VertexData previous;

            public VertexData(GraphVertex<T> vertex)
                : this(vertex, Double.PositiveInfinity)
            {

            }

            public VertexData(GraphVertex<T> vertex, double tentative_distance)
            {
                this.tentative_distance = tentative_distance;
                this.vertex = vertex;
                this.visited = false;
                this.previous = null;
            }

            public int CompareTo(VertexData other)
            {
                return this.tentative_distance.CompareTo(other.tentative_distance);
            }
        }

        public ItfcGraph<T> ShortestWeightedPath(T whereToStart, T whereToEnd)
        {
            GraphVertex<T> startVertex = this.GetVertexFor(whereToStart);
            GraphVertex<T> endVertex = this.GetVertexFor(whereToEnd);

            IEnumerable<GraphVertex<T>> allVertices = this.EnumerateVertices();

            ItfcPriorityQueue<VertexData> vertexQueue = new SimplePriorityQueue<VertexData>();

            //A way to find the vertexData object for a given vertex,
            //since things like evaluate neighbours don't return vertex data objects
            Dictionary<GraphVertex<T>, VertexData> vertexMap = new Dictionary<GraphVertex<T>, VertexData>();

            //for eafh vertex in this AbstractAdjacencyMatrixGraph keep some extra data
            foreach (GraphVertex<T> currentVertex in allVertices)
            {
                VertexData vertexWrapper;
                if (currentVertex == startVertex)
                {
                    //start vwertex is 0 points from the star
                    vertexWrapper = new VertexData(currentVertex, 0);
                }
                else
                {
                    //initially, every other vertex has am unknown weighted difference, so set it to infinity
                    vertexWrapper = new VertexData(currentVertex, Double.PositiveInfinity);
                }
                vertexQueue.Enqueue(vertexWrapper);
                vertexMap.Add(currentVertex, vertexWrapper);
            }

            //while there are vertices left to process (there's soemthing left in the queue)
            while (!vertexQueue.IsEmpty())
            {
                //Get the next vertex from the queue and set it as current
                VertexData vDataCurrent = vertexQueue.Dequeue();
                vDataCurrent.visited = true;

                //If the current vertex has a td of infinity
                if (Double.IsPositiveInfinity(vDataCurrent.tentative_distance))
                {
                    //Error out, throw an exception, because tger is no path to the taget
                    throw new ApplicationException("There is no path to vertex " + endVertex.ToString());
                }
                //if the current vertex is the target
                if (vDataCurrent.vertex == endVertex)
                {
                    //We've found the shortest path so reconstruct the graph representing the shortest path
                    break;
                }

                //get a list of neighbours for the current vertex
                IEnumerable<GraphVertex<T>> neighbours = this.EnumerateNeighborsOfVertexFor(vDataCurrent.vertex.Data);

                //inspect each neighbour of current
                foreach (GraphVertex<T> currentNeighbour in neighbours)
                {
                    VertexData vDataCurrentNeighbour = vertexMap[currentNeighbour];

                    //if the neighbour is unvisited
                    if (!vDataCurrentNeighbour.visited)
                    {
                        GraphEdge<T> edgeBetween = this.GetEdge(vDataCurrent.vertex.Data, currentNeighbour.Data);
                        //check to see if we have a shortesr path to curent than the current
                        //current.TD + edge weight < neighbor TD
                        //If we have a shorter path, uodate neigbour td and previous neigbour
                        if (vDataCurrent.tentative_distance + edgeBetween.Weight < vDataCurrentNeighbour.tentative_distance)
                        {
                            vDataCurrentNeighbour.tentative_distance = vDataCurrent.tentative_distance + edgeBetween.Weight;
                            vDataCurrentNeighbour.previous = vDataCurrent;
                        }
                    }
                }
            }
            return ReconstructGraph(vertexMap[endVertex]);
        }

        protected ItfcGraph<T> ReconstructGraph(VertexData endPoint)
        {
            //create an instance of the child type
            ItfcGraph<T> newGraph = (ItfcGraph<T>)GetType().Assembly.CreateInstance(GetType().FullName);

            //move backwards through the chain of previous pointers, constucting a new graph containing
            //only the vertices and edges of the shortest path.
            UndirectedAdjacencyMatrixGraph<T> shortestPath = new UndirectedAdjacencyMatrixGraph<T>();

            //create list of vertices

            //join vertices together - edges

            return shortestPath;
        }
    }
}
