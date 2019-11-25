using UnityEngine;

public class Cell
{
    public CellType Type { get; set; }
    public GameObject Item { get; set; }

    public int XCoordinate { get; set; }
    public int YCoordinate { get; set; }
    public override string ToString()
    {
        return Item + "  of type " + Type + " at " + XCoordinate + " " + YCoordinate;
    }
   public bool Equals(Cell other)
    {   if (this == null || other ==null) return false;
        return XCoordinate == other.XCoordinate && YCoordinate == other.YCoordinate;
    }
}

public enum CellType
{
    Floor,
    Wall,
    TargetSpot,
    Box,
    Gate
}