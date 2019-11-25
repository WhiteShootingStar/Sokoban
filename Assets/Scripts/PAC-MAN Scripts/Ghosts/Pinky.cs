using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pinky : AbstractGhost
{
    private Cell destinationCell;
    public override Cell GetDestination()
    {
        if (Player.currentDirection.Equals(Vector3.left))
        {
            return ConvertToCell(Vector3Int.FloorToInt(Player.transform.position) + new Vector3Int(-4, 0, -4));
        }
        else return ConvertToCell(Vector3Int.FloorToInt(Player.transform.position) + Player.currentDirection * 4);
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
        GetOptimalPathOutOfGivenFour(GetDestination());
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(255, 192, 203);
        Gizmos.DrawWireSphere(new Vector3(destinationCell.YCoordinate, 0, destinationCell.XCoordinate), 1f);
    }
}
