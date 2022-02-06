using System.Collections.Generic;

namespace _Scripts.PathFinding.Interface
{
    /**
     * Interface will make it easier to upgrade to A*.
     */
    public interface IPathFinder
    {
        public List<Node> FindPath(Node startingNode, Node endingNode);
        public void SetAvailableTiles(List<Node> availableNodes);
    }
}