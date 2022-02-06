using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.WorldGen
{
    public class WorldGenerator : MonoBehaviour
    {
        public static float TileOffset = 1.1f;

        public int Radius = 10;
        public GameObject tileObject;

        public static List<TileData> GeneratedTiles = new List<TileData>();
        public static event EventHandler NotifyWorldGenerated;

        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(StartWorldGeneration());
        }

        IEnumerator StartWorldGeneration()
        {
            for (int x = 0; x < Radius; x++)
            {
                for (int z = 0; z < Radius; z++)
                {
                    Vector3 pos = new Vector3(x * TileOffset, 0, z * TileOffset);

                    GameObject tile = Instantiate(tileObject, pos, Quaternion.identity, transform);
                    TileData tileData = new TileData(tile, TileStatus.Free);
                    tile.AddComponent<Tile>().TileData = tileData;

                    GeneratedTiles.Add(tileData);
                    yield return new WaitForSeconds(0.02f);
                }
            }

            NotifyWorldGenerated?.Invoke(this, EventArgs.Empty);
        }
    }
}