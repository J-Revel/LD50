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
    public System.Action<int, int> castleDestroyedDelegate;
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

    private Vector3 pathEndPosition { get { 
        if(generatedTiles.Count == 0)
            return transform.position;
        Transform lastTilePath = generatedTiles[generatedTiles.Count - 1].path;
        return lastTilePath.GetChild(lastTilePath.childCount - 1).position;
    }}

    public void GenerateTile(LevelTile tilePrefab)
    {
        Transform tilePrefabPath = tilePrefab.path;
        Vector3 startPosition = Vector3.zero;
        if(generatedTiles.Count != 0)
            startPosition = pathEndPosition - tilePrefabPath.GetChild(0).localPosition;
        LevelTile tile = Instantiate(tilePrefab, transform.position + startPosition, transform.rotation);
        int tileIndex = tileCount;
        tile.castleBuiltDelegate += (int checkpointIndex) => {
            castleBuiltDelegate?.Invoke(tileIndex, checkpointIndex);
            UnlockNextSection(tileIndex);
        };
        tile.castleDestroyedDelegate += (int checkpointIndex) => {
            castleDestroyedDelegate?.Invoke(tileIndex, checkpointIndex);
            UnlockNextSection(tileIndex);
        };
        tile.sectionIndex = tileCount;
        generatedTiles.Add(tile);
        tileCount++;
        tileGeneratedDelegate?.Invoke();
    }

    public void GenerateNextTile()
    {
        LevelTile tilePrefab = tilePrefabs[Random.Range(0, tilePrefabs.Length)];
        Transform tilePrefabPath = tilePrefab.path;
        LevelTile tile = Instantiate(tilePrefab, transform.position + pathEndPosition - tilePrefabPath.GetChild(0).localPosition, transform.rotation);
        int tileIndex = tileCount;
        tile.castleBuiltDelegate += (int checkpointIndex) => {
            castleBuiltDelegate?.Invoke(tileIndex, checkpointIndex);
            UnlockNextSection(tileIndex);
        };
        tile.castleDestroyedDelegate += (int checkpointIndex) => {
            castleDestroyedDelegate?.Invoke(tileIndex, checkpointIndex);
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
