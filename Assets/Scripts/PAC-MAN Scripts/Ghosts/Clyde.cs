using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clyde : AbstractGhost
{
    private Cell scatterCell;
    private Cell destinationCell;

    public override Cell GetDestination()
    {
        if (Vector3.Distance(transform.position, Player.transform.position) >= Mathf.Sqrt(8f))
        {
            return ConvertToCell(Vector3Int.FloorToInt(Player.transform.position));
        }
        else return scatterCell;
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
            if (Vector3.Distance(transform.position, resultPosition) < 0.0001f)
            {
                GetOptimalPathOutOfGivenFourScared();
            }

        }
        else if (state == GhostState.Eyes)
        {
            if (Vector3.Distance(transform.position, new Vector3Int(EyesCell.YCoordinate, 0, EyesCell.XCoordinate)) >= 0.3f)
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

    // Start is called before the first frame update
    void Start()
    {
        state = GhostState.Disabled;
        directionFrom = DirectionsFromWhichTheGhostCame.noDirection;

        scatterCell = mapData[0, mapData.GetLength(1) - 1];

        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PackManPlayer>();
        GetOptimalPathOutOfGivenFour(GetDestination());
    }

    // Update is called once per frame
    public override void Update()
    {
        Move();
        base.Update();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(new Vector3(destinationCell.YCoordinate, 0, destinationCell.XCoordinate), 1f);
    }
}
