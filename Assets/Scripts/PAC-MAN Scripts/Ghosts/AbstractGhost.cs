using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractGhost : MonoBehaviour
{
    public PackManPlayer Player;
    public Cell currentCell;
    public Cell[,] mapData = GameManagerPackMan.mapData;
    public DirectionsFromWhichTheGhostCame directionFrom;
    public GhostState state;
    public Vector3 resultPosition;

    // Start is called before the first frame update



    public abstract void Move();
    public abstract Cell GetDestination();

    public Cell ConvertToCell(Vector3Int vector)// Mapping of position to Cell
    {
        if (vector.z < 0) vector.z = 0;
        if (vector.x < 0) vector.x = 0;
        if (vector.x >= mapData.GetLength(1) - 1) vector.x = mapData.GetLength(1) - 2;
        if (vector.z >= mapData.GetLength(0) - 1) vector.z = mapData.GetLength(0) - 2;
        return mapData[vector.z, vector.x];
    }


    public void GetOptimalPathOutOfGivenFour(Cell destinationCell) // main algorithm to determine the next cell
    {
        Vector3Int up = Vector3Int.FloorToInt(transform.position) + Vector3Int.left; // since the axis are  [x,z] instaed of [x,y], vectors are different
        Vector3Int down = Vector3Int.FloorToInt(transform.position) + Vector3Int.right;

        Vector3Int left = Vector3Int.FloorToInt(transform.position) + new Vector3Int(0, 0, -1);
        Vector3Int right = Vector3Int.FloorToInt(transform.position) + new Vector3Int(0, 0, 1);

        List<Vector3Int> directions = new List<Vector3Int>();// since Blinky has a priority , I've decided to add direction in special order to the list 

        //if Ghosts came from the same direction, we don't add this direction
        if (directionFrom != DirectionsFromWhichTheGhostCame.right)
        {
            directions.Add(right);
        }
        if (directionFrom != DirectionsFromWhichTheGhostCame.down)
        {
            directions.Add(down);
        }
        if (directionFrom != DirectionsFromWhichTheGhostCame.left)
        {
            directions.Add(left);
        }
        if (directionFrom != DirectionsFromWhichTheGhostCame.up)
        {
            directions.Add(up);
        }



        Vector3? result = null;
        float minDistance = float.MaxValue;
        //finding direction with minimal distance
        for (int i = 0; i < directions.Count; i++)
        {

            Cell potentialDirectionCell = ConvertToCell(directions[i]);
            if (potentialDirectionCell.Type == CellType.Floor)
            {
                float distance = Vector3.Distance(directions[i], new Vector3Int(destinationCell.YCoordinate, 0, destinationCell.XCoordinate));
                // float distance = Vector3.Distance(new Vector3(potentialDirectionCell.yCoordinate, 0, potentialDirectionCell.xCoordinate), new Vector3(destinationCell.yCoordinate, 0, destinationCell.xCoordinate));
                if (distance <= minDistance)
                {
                    minDistance = distance;
                    result = directions[i];
                    // Debug.Log("Minimal Distance is " + directions[i]);
                }
            }

        }

        AssignDirection(transform.position, result.Value);

        resultPosition = result.Value;
    }


    void AssignDirection(Vector3 currentPosition, Vector3 nextPosition)
    {
        if (nextPosition.z - currentPosition.z >= 0.5f)
        {
            directionFrom = DirectionsFromWhichTheGhostCame.left;
        }
        else if (nextPosition.z - currentPosition.z <= -0.5f)
        {
            directionFrom = DirectionsFromWhichTheGhostCame.right;
        }
        else if (nextPosition.x - currentPosition.x >= 0.5f)
        {
            directionFrom = DirectionsFromWhichTheGhostCame.up;
        }
        else if (nextPosition.x - currentPosition.x <= -0.5f)
        {
            directionFrom = DirectionsFromWhichTheGhostCame.down;
        }
    }

    public void GetOptimalPathOutOfGivenFourScared()
    {
        Vector3Int up = Vector3Int.FloorToInt(transform.position) + Vector3Int.left; // since the axis are  [x,z] instaed of [x,y], vectors are different
        Vector3Int down = Vector3Int.FloorToInt(transform.position) + Vector3Int.right;

        Vector3Int left = Vector3Int.FloorToInt(transform.position) + new Vector3Int(0, 0, -1);
        Vector3Int right = Vector3Int.FloorToInt(transform.position) + new Vector3Int(0, 0, 1);

        List<Vector3Int> directions = new List<Vector3Int>();// since Blinky has a priority , I've decided to add direction in special order to the list 
        if (!IsWall(up)&& directionFrom != DirectionsFromWhichTheGhostCame.up)
        {
            directions.Add(up);
        }
        if (!IsWall(down) && directionFrom != DirectionsFromWhichTheGhostCame.down)
        {
            directions.Add(down);
        }
        if (!IsWall(left) && directionFrom != DirectionsFromWhichTheGhostCame.left)
        {
            directions.Add(left);
        }
        if (!IsWall(right)&& directionFrom != DirectionsFromWhichTheGhostCame.right)
        {
            directions.Add(right);
        }
        var nextDirection = directions[Random.Range(0, directions.Count)];

        AssignDirection(transform.position, nextDirection);
        resultPosition = nextDirection;
    }
    bool IsWall(Vector3Int vector)
    {
        return ConvertToCell(vector).Type == CellType.Wall;
    }
}

public enum DirectionsFromWhichTheGhostCame
{
    left, right, up, down, noDirection
}
public enum GhostState
{
    Chase,
    Frightened,
    Disabled
}