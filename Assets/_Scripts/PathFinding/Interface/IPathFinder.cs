using System.Collections.Generic;

namespace _Scripts.PathFinding.Interface
{
    public interface IPathFinder
    {
        public List<Node> FindPath(Node startingNode, Node endingNode);
        public void SetAvailableTiles(List<Node> availableNodes);
    }
}