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
            if (Vector3.Distance(transform.position, Player.transform.position) > .5f )
            {
                destinationCell = GetDestination();
                GetOptimalPathOutOfGivenFour(destinationCell);
               
            }
            
        }
        else if (state == GhostState.Frightened)
        {
            if (Vector3.Distance(transform.position, resultPosition) < 0.0001f)
            {
                GetOptimalPathOutOfGivenFourScared();
            }
           
        }
        else if (state == GhostState.Eyes)
        {
            if (Vector3.Distance(transform.position, new Vector3Int(EyesCell.YCoordinate,0,EyesCell.XCoordinate)) >= 0.3f)
            {
                GetOptimalPathOutOfGivenFour(EyesCell);
            }
            else
            {
                state = GhostState.Chase;
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
   public override void  Update()
    {
        Move();
        base.Update();
    }

   



    private void OnDrawGizmos()
    {
       
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(new Vector3(destinationCell.YCoordinate, 0, destinationCell.XCoordinate), 1f);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(new Vector3Int(EyesCell.YCoordinate, 0, EyesCell.XCoordinate), 1f);
    }
}

