using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public LevelTile[] tilePrefabs;
    public float tileSpacing = 12;
    public bool addTile = false;

    private int tileCount = 0;
    public int startTileCount = 5;

    public List<LevelTile> generatedTiles = new List<LevelTile>();

    void Start()
    {
        for(int i=0; i<startTileCount; i++)
        {
            GenerateNextTile();
        }
    }

    private void Update()
    {
        if(addTile)
        {
            GenerateNextTile();
            addTile = false;
        }
    }

    public void GenerateNextTile()
    {
        generatedTiles.Add(Instantiate(tilePrefabs[Random.Range(0, tilePrefabs.Length)], transform.position + Vector3.right * tileSpacing * tileCount, transform.rotation));
        tileCount++;
    }
}
