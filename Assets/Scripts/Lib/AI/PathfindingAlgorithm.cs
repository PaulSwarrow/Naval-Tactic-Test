using System;
using System.Collections.Generic;
using DefaultNamespace.Utils.MissingNetClasses;

namespace Ship.AI.CommandsGraphSearch
{
    public abstract class PathfindingAlgorithm<TNode, TEdge, TContext, TPathElement>
    {
        protected struct EdgeInfo
        {
            public TEdge Edge;
            public TNode Destination;
            public int Cost;
        }
        
        protected PathData<TPathElement> FindBest(TNode start, TContext context, Func<TNode, int> estimator)
        {
            var frontier = new SimplePriorityQueue<TNode>();
            frontier.Enqueue(start, 0);

            var cameFrom = new Dictionary<TNode, (TNode node, TEdge edge)>();
            var costSoFar = new Dictionary<TNode, int>();
            var bestNode = start;
            var bestScore = estimator(start);

            cameFrom[start] = (start, default);
            costSoFar[start] = 0;

            while (frontier.Count > 0)
            {
                var current = frontier.Dequeue();
                var currentEstimationScore = estimator(current);

                // Update the highest estimation node if the current node has a higher estimation score
                if (currentEstimationScore > bestScore)
                {
                    bestNode = current;
                    bestScore = currentEstimationScore;
                    
                }

                foreach (var edgeInfo in GetEdges(current, context))
                {
                    var newCost = costSoFar[current] + edgeInfo.Cost;
                    if (!costSoFar.ContainsKey(edgeInfo.Destination) || newCost < costSoFar[edgeInfo.Destination])
                    {
                        costSoFar[edgeInfo.Destination] = newCost;
                        int priority = newCost - estimator(edgeInfo.Destination); // Priority based on cost and estimation
                        frontier.Enqueue(edgeInfo.Destination, priority);
                        cameFrom[edgeInfo.Destination] = (current, edgeInfo.Edge);
                    }
                }
            }
            
            // Construct and return the path to the node with the highest estimation
            return ReconstructPath(cameFrom, start, bestNode);
        }
        
        protected PathData<TPathElement> FindPathTo(TNode start, TContext context, Predicate<TNode> isDestination, Func<TNode, int, int> heuristic)
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

                if (isDestination(current))
                {
                    return ReconstructPath(cameFrom, start, current);
                }

                foreach (var edgeInfo in GetEdges(current, context))
                {
                    var newCost = costSoFar[current] + edgeInfo.Cost;
                    if (!costSoFar.ContainsKey(edgeInfo.Destination) || newCost < costSoFar[edgeInfo.Destination])
                    {
                        costSoFar[edgeInfo.Destination] = newCost;
                        int priority = heuristic(edgeInfo.Destination, newCost);
                        frontier.Enqueue(edgeInfo.Destination, priority);
                        cameFrom[edgeInfo.Destination] = (current, edgeInfo.Edge);
                    }
                }
            }

            // Path not found
            return null;
        }
        // Function to get neighbors; adapt to your needs
        protected abstract List<EdgeInfo> GetEdges(TNode node, TContext context);

        private PathData<TPathElement> ReconstructPath(Dictionary<TNode, (TNode node, TEdge edge)> cameFrom, TNode start, TNode end)
        {
            PathData<TPathElement> path = new ();
            (TNode node, TEdge edge) current = (end, default);
            while (!current.node.Equals(start))
            {
                path.Add(CreatePathElement(current.node, current.edge));
                current = cameFrom[current.node];
            }
            path.Add(CreatePathElement(current.node, current.edge));
            path.Reverse();
            return path;
        }

        protected abstract TPathElement CreatePathElement(TNode origin, TEdge step);


    }
}