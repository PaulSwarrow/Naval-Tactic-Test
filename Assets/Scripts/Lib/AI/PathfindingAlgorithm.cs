using System.Collections.Generic;
using DefaultNamespace.Utils.MissingNetClasses;

namespace Ship.AI.CommandsGraphSearch
{
    public abstract class PathfindingAlgorithm<TNode, TEdge>
    {
        protected struct EdgeInfo
        {
            public TEdge Edge;
            public TNode Destination;
            public int Cost;
        }
        
        public List<(TNode node, TEdge edge)> FindPath(TNode start)
        {
            var frontier = new SimplePriorityQueue<TNode>();
            frontier.Enqueue(start, 0);

            var cameFrom = new Dictionary<TNode, (TNode node, TEdge edge)>();
            var costSoFar = new Dictionary<TNode, int>();

            cameFrom[start] = (start, default);
            costSoFar[start] = 0;

            while (frontier.Count > 0)
            {
                var current = frontier.Dequeue();

                if (IsDestination(current))
                {
                    return ReconstructPath(cameFrom, start, current);
                }

                foreach (var edgeInfo in GetEdges(current))
                {
                    var newCost = costSoFar[current] + edgeInfo.Cost;
                    if (!costSoFar.ContainsKey(edgeInfo.Destination) || newCost < costSoFar[edgeInfo.Destination])
                    {
                        costSoFar[edgeInfo.Destination] = newCost;
                        int priority = Heuristic(edgeInfo.Destination, newCost);
                        frontier.Enqueue(edgeInfo.Destination, priority);
                        cameFrom[edgeInfo.Destination] = (current, edgeInfo.Edge);
                    }
                }
            }

            // Path not found
            return null;
        }

        protected abstract int Heuristic(TNode node, int totalCost);
        // Function to get neighbors; adapt to your needs
        protected abstract List<EdgeInfo> GetEdges(TNode node);
        protected abstract bool IsDestination(TNode node);

        private List<(TNode node, TEdge edge)> ReconstructPath(Dictionary<TNode, (TNode node, TEdge edge)> cameFrom, TNode start, TNode end)
        {
            List<(TNode node, TEdge edge)> path = new List<(TNode node, TEdge edge)>();
            (TNode node, TEdge edge) current = (end, default);
            while (!current.node.Equals(start))
            {
                path.Add(current);
                current = cameFrom[current.node];
            }
            path.Add(current);
            path.Reverse();
            return path;
        }
    }
}