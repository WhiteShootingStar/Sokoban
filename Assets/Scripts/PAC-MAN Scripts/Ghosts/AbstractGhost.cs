using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractGhost : MonoBehaviour
{
    public PackManPlayer Player;
    public Cell currentCell;
    public  Cell[,] mapData = GameManagerPackMan.mapData;
    public DirectionsFromWhichTheGhostCame directionFrom;

    public Vector3 resultPosition;
    // Start is called before the first frame update



    public abstract void Move();
    public abstract Cell GetDestination();

    public Cell ConvertToCell(Vector3 vector)// Mapping of position to Cell
    {
        if (vector.z < 0) vector.z = 0;
        if(vector.x < 0) vector.x = 0;
        if (vector.x >= mapData.GetLength(1)-1) vector.x = mapData.GetLength(1) - 2;
        if (vector.z >= mapData.GetLength(0)-1) vector.z = mapData.GetLength(0) - 2;
        return mapData[Mathf.FloorToInt(vector.z), Mathf.FloorToInt(vector.x)];
    }


    public void GetOptimalPathOutOfGivenFour(Cell destinationCell) // main algorithm to determine the next cell
    {
        Vector3 up = transform.position + Vector3.left; // since the axis are  [x,z] instaed of [x,y], vectors are different
        Vector3 down = transform.position + Vector3.right;

        Vector3 left = transform.position + Vector3.back;
        Vector3 right = transform.position + Vector3.forward;

        List<Vector3> directions = new List<Vector3>();// since Blinky has a priority , I've decided to add direction in special order to the list 

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
            if (potentialDirectionCell.type == CellType.Floor)
            {
                float distance = Vector3.Distance(directions[i], new Vector3(destinationCell.yCoordinate, 0, destinationCell.xCoordinate));
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



}

public enum DirectionsFromWhichTheGhostCame
{
    left, right, up, down,noDirection
}
