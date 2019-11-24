using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inky : AbstractGhost
{
    private Blinky blinky;
    private Vector3 vectorBetweenPackManAndBlinky;
    private Cell destinationCell;
    private Cell tempCell;

    public override Cell GetDestination()
    {
         tempCell = getIntermidiatePosition();
         vectorBetweenPackManAndBlinky = blinky.transform.position- new Vector3(tempCell.yCoordinate, 0, tempCell.xCoordinate) ;
        vectorBetweenPackManAndBlinky = (Quaternion.AngleAxis(180, Vector3.up) * vectorBetweenPackManAndBlinky);
        
        return ConvertToCell(vectorBetweenPackManAndBlinky);
    }

    public override void Move()
    {
        if (Vector3.Distance(transform.position, Player.transform.position) > .5f)
        {
            destinationCell = GetDestination();
            GetOptimalPathOutOfGivenFour(destinationCell);
            transform.position = Vector3.MoveTowards(transform.position, resultPosition, 0.05f);
        }
        else Debug.Log("Killed the Player");
    }

    // Start is called before the first frame update
    void Start()
    {
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

    Cell getIntermidiatePosition()
    {
        if (Player.currentDirection.Equals(Vector3.left))
        {
            return ConvertToCell(Player.transform.position + new Vector3Int(-2, 0, -2));
        }
        else return ConvertToCell(Player.transform.position + Player.currentDirection * 2);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(vectorBetweenPackManAndBlinky, 1f);
        //Gizmos.DrawLine(blinky.transform.position, new Vector3(destinationCell.yCoordinate, 0, destinationCell.xCoordinate));
        Gizmos.DrawLine(blinky.transform.position, new Vector3(tempCell.yCoordinate, 0, tempCell.xCoordinate));
    }
}
