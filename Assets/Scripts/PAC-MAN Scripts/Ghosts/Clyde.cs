using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clyde : AbstractGhost
{
    private Cell scatterCell;
    private Cell destinationCell;

    public override Cell GetDestination()
    {
        if (Vector3.Distance(transform.position, Player.transform.position) >= 8f)
        {
            return ConvertToCell(Player.transform.position);
        }
        else return scatterCell;
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

        scatterCell = mapData[0, mapData.GetLength(1)-1];

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
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(new Vector3(destinationCell.yCoordinate, 0, destinationCell.xCoordinate), 1f);
    }
}
