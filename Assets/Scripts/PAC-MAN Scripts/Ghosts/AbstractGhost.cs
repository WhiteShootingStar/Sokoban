using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractGhost : MonoBehaviour
{
    public PackManPlayer Player;
    public Cell currentCell;
    public Cell[,] mapData = GameManagerPackMan.mapData;
    public DirectionsFromWhichTheGhostCame directionFrom;
   
    // Start is called before the first frame update



    public abstract void Move();
    public abstract Cell GetDestination();

    public Cell ConvertToCell(Vector3 vector)// Mapping of position to Cell
    {

        return mapData[(int)vector.z, (int)vector.x];
    }
}

public enum DirectionsFromWhichTheGhostCame
{
    left, right, up, down,noDirection
}
