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
            return ConvertToCell(Player.transform.position + new Vector3Int(-4, 0, -4));
        }
        else return ConvertToCell(Player.transform.position + Player.currentDirection * 4);
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
        GetOptimalPathOutOfGivenFour(GetDestination());
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(255, 192, 203);
        Gizmos.DrawWireSphere(new Vector3(destinationCell.yCoordinate, 0, destinationCell.xCoordinate), 1f);
    }
}
