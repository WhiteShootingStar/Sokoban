using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blinky : AbstractGhost
{

    private Cell destinationCell;
   

    public override Cell GetDestination()
    {
        return ConvertToCell(Vector3Int.FloorToInt( Player.transform.position));
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

    private void Start()
    {
        state = GhostState.Disabled;
        directionFrom = DirectionsFromWhichTheGhostCame.noDirection;
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PackManPlayer>();
        GetOptimalPathOutOfGivenFour(GetDestination());
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

   



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(new Vector3(destinationCell.YCoordinate, 0, destinationCell.XCoordinate), 1f);
    }
}

