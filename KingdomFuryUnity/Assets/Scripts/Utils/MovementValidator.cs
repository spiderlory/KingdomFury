using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MovementValidator : MonoBehaviour
{
    // Public Vars
    public Tilemap tilemap;
    
    
    // Local Grid vars
    //   Matrix bounds definition
    private int _matrixRad = 2;
    private int _matrixSize;
    
    private Vector3Int _boundPosition;
    private Vector3Int _boundSize;
    private BoundsInt _matrixBounds;

    private TileBase[] _tileBases;
    private Centered3DMatrix<int?> _matrix3D;
    
    
    // Player position
    private Vector3Int _characterPosition;
    
    private void Start()
    {
        _matrixSize = _matrixRad * 2 + 1;
        _boundSize = new Vector3Int(_matrixSize, _matrixSize, _matrixSize);

    }

    /// <summary>
    ///   <para>Generate a Local Matrix representing character's surrounding. It helps with figuring out if the player can move in a specific direction.</para>
    /// </summary> 
    public void GenerateLocalGrid()
    {
        // Matrix bounds definition
        _boundPosition = new Vector3Int(_characterPosition.x - _matrixRad, _characterPosition.y - _matrixRad, _characterPosition.z - _matrixRad);

        _matrixBounds = new BoundsInt(_boundPosition, _boundSize);
        
        // Get the surrounding tiles.
        _tileBases = tilemap.GetTilesBlock(_matrixBounds);
        
        // Iterate over each tile
        int i = 0;
        
        _matrix3D = new Centered3DMatrix<int?>(_matrixRad);
        
        foreach (TileBase tile in _tileBases)
        {
            if (tile is not null)
            {
                CustomRuleTile customRuleTile = tile as CustomRuleTile;

                _matrix3D[i] = customRuleTile.tileData.isWalkable? 1 : 0;
            }
            else
            {
                _matrix3D[i] = null;
            }

            i++;
        }
    }

    /// <summary>
    ///   <para>This method alters the input directions by adding the z component. z is computed by looking at the tiles in the wanted direction. The character can move only if there is a walkable tile and character.z +-1 tile.</para>
    /// </summary>
    public Vector3Int GetValidDirection(Vector3Int position, Vector2Int direction)
    {
        _characterPosition = position;
        
        GenerateLocalGrid();
        
        Vector3Int validDirection = Vector3Int.zero;
        
        validDirection.x = direction.x;
        validDirection.y = direction.y;
        
        // If there is a tile above the player height in the wanted direction return [0, 0, 0].
        if (_matrix3D[direction.y, direction.x, 2] != null)
        {
            return Vector3Int.zero;
        }
        
        // Returns the validDirection if there is a walkable tile, else [0, 0, 0].
        for (int z = 1; z >= -1; z--)
        {
            validDirection.z = z;
            
            switch (_matrix3D[direction.y, direction.x, z])
            {
                case 1:
                    return validDirection;
                case 0:
                    return Vector3Int.zero;
                case null:
                    continue;
            }
        }
        
        return Vector3Int.zero;
    }
}
