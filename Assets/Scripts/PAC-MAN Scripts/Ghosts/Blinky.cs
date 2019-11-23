using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blinky : AbstractGhost
{
    public Vector3 resultPosition;
 

    public override Cell GetDestination()
    {
        throw new System.NotImplementedException();
    }

    public override void Move()
    {
        Cell destinationCell = ConvertToCell(Player.transform.position);
        if (Vector3.Distance(transform.position, resultPosition) <1.5f)
        {
            GetOptimalPathOutOfGivenFour(Player.CellPosition);
            transform.position = Vector3.MoveTowards(transform.position, resultPosition, 0.05f);
        }
        else Debug.Log("Killed the Player");
    }

    private void Start()
    {
        directionFrom = DirectionsFromWhichTheGhostCame.noDirection;
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PackManPlayer>();
        GetOptimalPathOutOfGivenFour(Player.CellPosition);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void GetOptimalPathOutOfGivenFour(Cell destinationCell)
    {
        Vector3 up = transform.position + Vector3.left;
        Vector3 down = transform.position + Vector3.right ;

        Vector3 left = transform.position + Vector3.back ;
        Vector3 right = transform.position + Vector3.forward ;

        List<Vector3> directions = new List<Vector3>();// since Blinky has a priority , I've decided to add direction in special order to the list 
        if (directionFrom != DirectionsFromWhichTheGhostCame.right )
        {
            directions.Add(right);
        }
        if (directionFrom != DirectionsFromWhichTheGhostCame.down )
        {
            directions.Add(down);
        }
        if (directionFrom != DirectionsFromWhichTheGhostCame.left )
        {
            directions.Add(left);
        }
        if (directionFrom != DirectionsFromWhichTheGhostCame.up )
        {
            directions.Add(up);
        }



        Vector3? result = null;
        float minDistance = float.MaxValue;

        for (int i = 0; i < directions.Count; i++)
        {

            Cell potentialDirectionCell = ConvertToCell(directions[i]);
            if (potentialDirectionCell.type == CellType.Floor)
            {
                 //float distance = Vector3.Distance(directions[i], Player.transform.position);
               float distance = Vector3.Distance(new Vector3(potentialDirectionCell.yCoordinate, 0, potentialDirectionCell.xCoordinate), new Vector3(destinationCell.yCoordinate, 0, destinationCell.xCoordinate));
                if (distance <= minDistance)
                {
                    minDistance = distance;
                    result = directions[i];
                    Debug.Log("Minimal Distance is " + directions[i]);
                }
            }
            else
            {
              //  Debug.Log(potentialDirectionCell + " is a wall Was Moving in " + directions[i]);
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



    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;   
        Gizmos.DrawWireSphere(resultPosition, 1f);
    }
}

