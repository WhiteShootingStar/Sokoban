using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blinky : AbstractGhost
{

    private Cell destinationCell;

    public override Cell GetDestination()
    {
        return ConvertToCell(Player.transform.position);
    }

    public override void Move()
    {
      
        if (Vector3.Distance(transform.position, Player.transform.position) >.5f)
        {
            destinationCell = GetDestination();
            GetOptimalPathOutOfGivenFour(destinationCell);
            transform.position = Vector3.MoveTowards(transform.position, resultPosition, 0.05f);
        }
        else Debug.Log("Killed the Player");
    }

    private void Start()
    {
        directionFrom = DirectionsFromWhichTheGhostCame.noDirection;
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PackManPlayer>();
        GetOptimalPathOutOfGivenFour(GetDestination());
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

   



    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(new Vector3(destinationCell.yCoordinate, 0, destinationCell.xCoordinate), 1f);
    }
}

