using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inky : AbstractGhost
{
    private Blinky blinky;
    private Vector3Int vectorBetweenPackManAndBlinky;
    private Cell destinationCell;
    private Cell tempCell;

    public override Cell GetDestination()
    {
        tempCell = GetIntermidiatePosition();
        Vector3Int tempVector = new Vector3Int(tempCell.YCoordinate, 0, tempCell.XCoordinate);

        vectorBetweenPackManAndBlinky = tempVector + (tempVector - Vector3Int.FloorToInt(blinky.transform.position));

        return ConvertToCell(vectorBetweenPackManAndBlinky);
    }

    public override void Move()
    {
        if (state == GhostState.Chase)
        {
            if (Vector3.Distance(transform.position, Player.transform.position) > .5f)
            {
                destinationCell = GetDestination();
                GetOptimalPathOutOfGivenFour(destinationCell);
               
            }
            else Debug.Log("Killed the Player");
        }
        else if (state == GhostState.Frightened)
        {
            if (Vector3.Distance(transform.position, resultPosition) < .1f)
            {
                GetOptimalPathOutOfGivenFourScared();
            }

        }
        transform.position = Vector3.MoveTowards(transform.position, resultPosition, 0.05f);
    }

    // Start is called before the first frame update
    void Start()
    {
        state = GhostState.Disabled;
        directionFrom = DirectionsFromWhichTheGhostCame.noDirection;
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PackManPlayer>();
        blinky = GameObject.FindGameObjectWithTag("Blinky").GetComponent<Blinky>();
        GetOptimalPathOutOfGivenFour(GetDestination());
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    Cell GetIntermidiatePosition()
    {
        if (Player.currentDirection.Equals(Vector3Int.left))
        {
            return ConvertToCell(Vector3Int.FloorToInt(Player.transform.position) + new Vector3Int(-2, 0, -2));
        }
        else return ConvertToCell(Vector3Int.FloorToInt(Player.transform.position) + Player.currentDirection * 2);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        // Gizmos.DrawWireSphere(vectorBetweenPackManAndBlinky, 1f);
        //Gizmos.DrawLine(blinky.transform.position, new Vector3(destinationCell.yCoordinate, 0, destinationCell.xCoordinate));
        Gizmos.DrawWireSphere(new Vector3Int(destinationCell.YCoordinate, 0, destinationCell.XCoordinate), 1f);
    }
}
