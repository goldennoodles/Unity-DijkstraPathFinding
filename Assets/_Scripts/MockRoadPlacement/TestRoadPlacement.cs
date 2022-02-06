using System.Collections.Generic;
using _Scripts.PathFinding;
using _Scripts.PathFinding.Interface;
using _Scripts.WorldGen;
using UnityEngine;

namespace _Scripts.MockRoadPlacement
{
    public class TestRoadPlacement : MonoBehaviour
    {
        [SerializeField] private GameObject roadTile;
        [SerializeField] private GameObject positionStart;
        [SerializeField] private GameObject positionEnd;

        private Node _startPos;
        private Node _endPos;

        public Material pathMaterial;
        public Material resetMaterial;

        public List<Node> finalPath;
        private List<Node> _roadNodes;

        private IPathFinder _path;

        void Start()
        {
            _roadNodes = new List<Node>();
            _path = new Path();
            WorldGenerator.NotifyWorldGenerated += (sender, args) => { GenerateMockStartAndEnd(); };
        }

        void GenerateMockStartAndEnd()
        {
            Vector3 randStart = WorldGenerator.GeneratedTiles[WorldGenerator.GeneratedTiles.Count / 3].GetTilePosition;
            Vector3 randEnd = WorldGenerator.GeneratedTiles[0].GetTilePosition;

            randStart.y = 4;
            randEnd.y = 4;

            GameObject roadStart = Instantiate(positionStart, randStart, Quaternion.identity);
            GameObject roadEnd = Instantiate(positionEnd, randEnd, Quaternion.identity);


            _startPos = new Node(roadStart.transform.position, roadStart);
            _endPos = new Node(roadEnd.transform.position, roadEnd);

            roadStart.name = "Start: " + _startPos.GetCoords();
            roadEnd.name = "End: " + _endPos.GetCoords();

            _roadNodes.Add(_startPos);
            _roadNodes.Add(_endPos);
        }

        public void Update()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                Tile t = hit.collider.GetComponent<Tile>();

                if (Input.GetMouseButton(0) && t != null)
                {
                    if (t.TileData.GetTileStatus == TileStatus.Occupied)
                        return;

                    Vector3 tileObjectPosition = t.TileData.GetTilePosition;
                    tileObjectPosition.y = 3.5f;

                    GameObject road = Instantiate(roadTile, tileObjectPosition, Quaternion.identity, transform);

                    Node roadNode = new Node(tileObjectPosition, road);

                    road.name = $"{roadNode.GetCoords().ToString()}";
                    _roadNodes.Add(roadNode);

                    t.TileData.SetTileStatus(TileStatus.Occupied);
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                finalPath = new List<Node>();
                _path.SetAvailableTiles(_roadNodes);

                finalPath = _path.FindPath(_startPos, _endPos);
                AmendMaterialForPath(false);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                AmendMaterialForPath(true);
            }
        }

        private void AmendMaterialForPath(bool isReset)
        {
            if (!isReset)
                foreach (var fNode in finalPath)
                {
                    fNode.GetGameObject().GetComponent<MeshRenderer>().sharedMaterial = pathMaterial;
                }
            else
                foreach (var rNode in _roadNodes)
                {
                    rNode.GetGameObject().GetComponent<MeshRenderer>().sharedMaterial = resetMaterial;
                }
        }
    }
}