using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.PathFinding
{
    public class Node
    {
        private readonly Coords _coords;
        private readonly GameObject _gameObject;
        private readonly List<Node> _neighbourNodes;

        private Node _parentNode;
        private float _weight = int.MaxValue;
        
        public Node(Vector3 position, GameObject gameObject)
        {
            _neighbourNodes = new List<Node>();

            _gameObject = gameObject;
            _coords = new Coords(Mathf.FloorToInt(position.x),
                Mathf.FloorToInt(position.z));
        }

        public Coords GetCoords()
        {
            return _coords;
        }

        public void AddNeighbourNode(Node neighbour)
        {
            _neighbourNodes.Add(neighbour);
        }

        public List<Node> GetNeighbourNodes()
        {
            return _neighbourNodes;
        }

        public Node GetParentNode()
        {
            return _parentNode;
        }

        public void SetParent(Node parent)
        {
            _parentNode = parent;
        }

        public void SetWeight(float weight)
        {
            _weight = weight;
        }

        public float GetWeight()
        {
            return _weight;
        }

        public GameObject GetGameObject()
        {
            return _gameObject;
        }
    }

    public struct Coords
    {
        private int _x;
        private int _z;

        public Coords(int x, int z)
        {
            _x = x;
            _z = z;
        }

        public int GetX()
        {
            return _x;
        }

        public int GetZ()
        {
            return _z;
        }

        public override string ToString()
        {
            return $"Position: x:{_x}  z:{_z}";
        }
    }
}