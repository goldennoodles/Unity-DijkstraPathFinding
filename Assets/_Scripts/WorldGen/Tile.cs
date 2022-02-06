using System;
using UnityEngine;

namespace _Scripts.WorldGen
{
    public enum TileStatus
    {
        Free,
        Occupied
    }
    
    public class Tile : MonoBehaviour
    {
        public TileData TileData;
    }
    
    [Serializable]
    public class TileData
    {
        [SerializeField] private GameObject tile;
        [SerializeField] private TileStatus tileStatus;
        [SerializeField] private Vector3 tilePosition;

        public TileData(GameObject tile, TileStatus tileStatus)
        {
            this.tile = tile;
            this.tileStatus = tileStatus;
            tilePosition = GetTilePosition;
        }

        public GameObject GetTile => tile;
        public Vector3 GetTilePosition => tile.transform.position;
        public TileStatus GetTileStatus => tileStatus;
        public void SetTileStatus(TileStatus tileStatus) => this.tileStatus = tileStatus;
    }
}