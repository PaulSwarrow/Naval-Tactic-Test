using System.Collections.Generic;
using DefaultNamespace.Utils.MissingNetClasses;
using UnityEngine.AI;

namespace Ship.AI.CommandsGraphSearch
{
    public class CommandGenerator
    {

        public List<ManeuverNode> FindPath(ManeuverNode start)
        {
            var frontier = new SimplePriorityQueue<ManeuverNode>();
            frontier.Enqueue(start, 0);

            var cameFrom = new Dictionary<ManeuverNode, ManeuverNode>();
            var costSoFar = new Dictionary<ManeuverNode, int>();

            cameFrom[start] = start;
            costSoFar[start] = 0;

            while (frontier.Count > 0)
            {
                var current = frontier.Dequeue();

                if (IsDestination(current))
                {
                    return ReconstructPath(cameFrom, start, current);
                }

                foreach (var edge in GetEdges(current))
                {
                    var newCost = costSoFar[current] + edge.Cost;
                    if (!costSoFar.ContainsKey(edge.Destination) || newCost < costSoFar[edge.Destination])
                    {
                        costSoFar[edge.Destination] = newCost;
                        int priority = newCost; // In a more complex scenario, you might use a heuristic here
                        frontier.Enqueue(edge.Destination, priority);
                        cameFrom[edge.Destination] = current;
                    }
                }
            }

            // Path not found
            return null;
        }

        private float Heuristic(ManeuverNode node)
        {
            //lesser - more priority
            return 1;
        }
        // Function to get neighbors; adapt to your needs
        private List<ManeuverEdge> GetEdges(ManeuverNode node)
        {
            // Example implementation, replace with actual logic to get neighbors
            return new List<ManeuverEdge>();
        }

        private bool IsDestination(ManeuverNode node)
        {
            return false;//TODO
        }

        private List<ManeuverNode> ReconstructPath(Dictionary<ManeuverNode, ManeuverNode> cameFrom, ManeuverNode start, ManeuverNode end)
        {
            List<ManeuverNode> path = new List<ManeuverNode>();
            ManeuverNode current = end;
            while (!current.Equals(start))
            {
                path.Add(current);
                current = cameFrom[current];
            }
            path.Add(start);
            path.Reverse();
            return path;
        }
    }
}