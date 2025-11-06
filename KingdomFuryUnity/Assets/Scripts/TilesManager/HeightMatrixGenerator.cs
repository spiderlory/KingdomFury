using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HeightMatrixGenerator : MonoBehaviour
{
    // Public Inputs
    public GameObject grid;
    public int matrixRadius = 5;

    // Private fields
    private List<GameObject> _tileMaps;
    
    private int _minTileHeight;
    private int _maxTileHeight;
    

    private void Start()
    {
        GenerateGraph(new Vector2Int(0, -10));

        _minTileHeight = 0;
        _maxTileHeight = _tileMaps.Count;
    }

    /// <summary>
    ///   <para>Given the player grid position, return a matrix representing world tiles height.</para>
    /// </summary>
    CenteredMatrix<int> GenerateGraph(Vector2Int playerPosition)
    {
        // Initialization of height matrix
        CenteredMatrix<int> centeredMatrix = new CenteredMatrix<int>(matrixRadius);
        centeredMatrix.init(-1);

        // Matrix bounds definition
        Vector3Int boundPosition = new Vector3Int(-matrixRadius + playerPosition.x, -matrixRadius + playerPosition.y, 0);
        Vector3Int boundSize = new Vector3Int(centeredMatrix.NCols, centeredMatrix.NCols, 1);

        BoundsInt matrixBounds = new BoundsInt(boundPosition, boundSize);

        // Iterate over tilemaps layers
        foreach (Transform child in grid.transform)
        {
            print(child.name);
            // Get current tilemap
            Tilemap tilemap = child.gameObject.GetComponent<Tilemap>();
            int height = child.gameObject.GetComponent<TilemapRenderer>().sortingOrder;
            
            print(height);

            // Get all tiles inside the defined Bounds
            TileBase[] tileBases = tilemap.GetTilesBlock(matrixBounds);

            
            // Iterate over each tile
            int i = 0;
            foreach (TileBase tile in tileBases)
            {
                if (tile != null)
                {
                    CustomRuleTile customRuleTile = tile as CustomRuleTile;

                    if (customRuleTile.tileData.isWalkable)
                    {
                        print(customRuleTile.tileData.name);
                        centeredMatrix[i] = Mathf.Max(height, centeredMatrix[i]);
                    }
                    else
                    {
                        centeredMatrix[i] = -1;
                    }
                }

                i++;
            }
        }
        
        centeredMatrix[centeredMatrix.Center] = 9;
        
        print(centeredMatrix);

        return centeredMatrix;
    }

    public bool CanWalk(Vector3Int initialPosition, Vector3Int targetPosition)
    {
        int minLayer = Mathf.Max(_minTileHeight, initialPosition.z);
        return true;
    }
}
