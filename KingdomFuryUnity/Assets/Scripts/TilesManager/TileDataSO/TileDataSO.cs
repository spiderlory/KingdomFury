using UnityEngine;

[CreateAssetMenu(fileName = "TileDataSO", menuName = "Scriptable Objects/TileDataSO")]
public class TileDataSO : ScriptableObject
{
    // Enum Definition
    public enum TerrainType
    {
        WATER,
        GRASS,
        ROCK,
    }

    public enum SlopeOrientation
    {
        UP,
        DOWN,
        NONE
    }
    
    public bool isWalkable = true;
    
    public TerrainType terrainType;
    public SlopeOrientation slopeOrientation;

    public override string ToString()
    {
        return base.ToString();
    }
}
