using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public LevelTile[] startTilePrefabs;
    public int initialAdditionalTiles = 1;
    public LevelTile[] tilePrefabs;
    public float tileSpacing = 12;
    public bool addTile = false;

    private int tileCount = 0;
    public int startTileCount = 5;

    public List<LevelTile> generatedTiles = new List<LevelTile>();
    public System.Action<int, int> castleBuiltDelegate;
    public System.Action tileGeneratedDelegate;

    void Start()
    {
        for(int i=0; i<startTilePrefabs.Length; i++)
        {
            GenerateTile(startTilePrefabs[i]);
        }
        for(int i=0; i<initialAdditionalTiles; i++)
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

    public void GenerateTile(LevelTile tilePrefab)
    {
        LevelTile tile = Instantiate(tilePrefab, transform.position + Vector3.right * tileSpacing * tileCount, transform.rotation);
        int tileIndex = tileCount;
        tile.castleBuiltDelegate += (int checkpointIndex) => {
            castleBuiltDelegate?.Invoke(tileIndex, checkpointIndex);
            UnlockNextSection(tileIndex);
        };
        tile.sectionIndex = tileCount;
        generatedTiles.Add(tile);
        tileCount++;
        tileGeneratedDelegate?.Invoke();
    }

    public void GenerateNextTile()
    {
        LevelTile tile = Instantiate(tilePrefabs[Random.Range(0, tilePrefabs.Length)], transform.position + Vector3.right * tileSpacing * tileCount, transform.rotation);
        int tileIndex = tileCount;
        tile.castleBuiltDelegate += (int checkpointIndex) => {
            castleBuiltDelegate?.Invoke(tileIndex, checkpointIndex);
            UnlockNextSection(tileIndex);
        };
        tile.sectionIndex = tileCount;
        generatedTiles.Add(tile);
        tileCount++;
        tileGeneratedDelegate?.Invoke();
    }

    private void UnlockNextSection(int sectionIndex)
    {
        if(sectionIndex == tileCount - 1)
        {
            GenerateNextTile();
        }
    }
}
