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
        var x = CellPosition.XCoordinate;
        var y = CellPosition.YCoordinate;
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

        if (toCheck.Type == CellType.Wall)
        {
            return false;
        }
        else if (toCheck.Type == CellType.Box)
        {
            if (PotentialNewBoxPlace.Type == CellType.Wall || PotentialNewBoxPlace.Type == CellType.Box)
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
                PotentialNewBoxPlace.Item = toMove.Item;
                PotentialNewBoxPlace.Item.transform.position = new Vector3Int(PotentialNewBoxPlace.YCoordinate, 0, PotentialNewBoxPlace.XCoordinate);
                toMove.Item = null;
                toMove.Type = CellType.Floor;
                PotentialNewBoxPlace.Type = CellType.Box;
                pushScore++;
            }
            transform.position = new Vector3Int(toMove.YCoordinate,0,toMove.XCoordinate);
                CellPosition = toMove;
            movementScore++;
        }
        isMoving = false;
    }

    bool CellHasBox(Cell toCheck)
    {
        if (toCheck.Type == CellType.Box)
        {
            return true;
        }
        return false;
    }


    bool isOccupied(Cell toCheck)
    {
        if (toCheck.Type == CellType.Box|| toCheck.Type==CellType.Wall)
        {
            return true;
        }
        return false;
    }
}
