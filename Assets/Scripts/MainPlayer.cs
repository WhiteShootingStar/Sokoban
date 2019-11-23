using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPlayer : MonoBehaviour
{
    public Cell CellPosition = null;
    public Cell[,] mapData = GameManager.mapData;
    private bool isMoving = false;

    public Text PlayerMovementText;
    public Text PlayerPushText;
    private int movementScore = 0, pushScore = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Move();
        PlayerMovementText.text = movementScore + " moves";
        PlayerPushText.text = pushScore + " pushes";
    }
    void Move()
    {
        var x = CellPosition.xCoordinate;
        var y = CellPosition.yCoordinate;
        Cell newCell = null;
        Cell PotentialBoxCell = null;
        if (Input.GetKeyDown(KeyCode.W))
        {
            newCell = GameManager.mapData[x, y - 1];
            PotentialBoxCell = GameManager.mapData[x, y - 2];
            isMoving = true;

           
           
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            newCell = GameManager.mapData[x, y + 1];
            PotentialBoxCell = GameManager.mapData[x, y + 2];
            isMoving = true;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            newCell = GameManager.mapData[x - 1, y];
            PotentialBoxCell = GameManager.mapData[x - 2, y];
            isMoving = true;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            newCell = GameManager.mapData[x + 1, y];
            PotentialBoxCell = GameManager.mapData[x + 2, y];
            isMoving = true;
        }
        if (isMoving)
            MoveToCell(newCell, PotentialBoxCell);
        
    }

    bool canMove(Cell toCheck, Cell PotentialNewBoxPlace)
    {

        if (toCheck.type == CellType.Wall)
        {
            return false;
        }
        else if (toCheck.type == CellType.Box)
        {
            if (PotentialNewBoxPlace.type == CellType.Wall || PotentialNewBoxPlace.type == CellType.Box)
            {
                return false;
            }
            else return true;
        }
        else return true;
    }

    void MoveToCell(Cell toMove, Cell PotentialNewBoxPlace)
    {
        if (canMove(toMove, PotentialNewBoxPlace))
        {
            if (CellHasBox(toMove))
            {  
                PotentialNewBoxPlace.item = toMove.item;
                PotentialNewBoxPlace.item.transform.position = new Vector3Int(PotentialNewBoxPlace.yCoordinate, 0, PotentialNewBoxPlace.xCoordinate);
                toMove.item = null;
                toMove.type = CellType.Floor;
                PotentialNewBoxPlace.type = CellType.Box;
                pushScore++;
            }
            transform.position = new Vector3Int(toMove.yCoordinate,0,toMove.xCoordinate);
                CellPosition = toMove;
            movementScore++;
        }
        isMoving = false;
    }

    bool CellHasBox(Cell toCheck)
    {
        if (toCheck.type == CellType.Box)
        {
            return true;
        }
        return false;
    }


    bool isOccupied(Cell toCheck)
    {
        if (toCheck.type == CellType.Box|| toCheck.type==CellType.Wall)
        {
            return true;
        }
        return false;
    }
}
