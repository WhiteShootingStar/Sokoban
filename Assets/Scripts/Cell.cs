using UnityEngine;

public class Cell
{
    public CellType type { get; set; }
    public GameObject item { get; set; }
    
    public int xCoordinate { get; set; }
    public int yCoordinate { get; set; }
    public override string ToString()
    {
        return item + "  of type " + type + " at " + yCoordinate+ " " +xCoordinate;
    }
}

public enum CellType
{
    Floor,
    Wall,
    TargetSpot,
    Box
}