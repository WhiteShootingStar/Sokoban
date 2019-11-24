using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PackManPlayer : MonoBehaviour
{
    public Cell CellPosition = null;
    public Cell[,] mapData = GameManagerPackMan.mapData;

    public Vector3 destination = Vector3.zero;
    public Vector3 currentDirection = Vector3.zero; // either Vector3 up,down,left,right
    public Vector3 nextDirection = Vector3.zero;

    public Text PlayerMovementText;
    public Text PlayerPushText;
    private int movementScore = 0, pushScore = 0;

    private readonly float precision = 0.0001f; 
    // Start is called before the first frame update
    void Start()
    {
        destination = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        PlayerMovementText.text = movementScore + " moves";
        PlayerPushText.text = pushScore + " pushes";
        Debug.Log(currentDirection);
    }
    void Move()
    {

         transform.position = Vector3.MoveTowards(transform.position, destination, 0.1f);
        CellPosition = mapData[(int)destination.z, (int)destination.x];
      
        if (Input.GetKey(KeyCode.W))
        {
            nextDirection = Vector3.left;
            
        }
        if (Input.GetKey(KeyCode.S))
        {
            nextDirection = Vector3.right;
           
        }
        if (Input.GetKey(KeyCode.A))
        {
            nextDirection = Vector3.back;
           
        }
        if (Input.GetKey(KeyCode.D))
        {
            nextDirection = Vector3.forward;
            
        }
        if (Vector3.Distance(destination, transform.position) < precision)//
        {
            if (CanMove(ConvertToCell(transform.position + nextDirection)))
            {
                destination = transform.position + nextDirection;
                currentDirection = nextDirection;
            }
            else
            {
                
                if (CanMove(ConvertToCell(transform.position + currentDirection)))
                {
                    destination= transform.position + currentDirection;
                }
            }
        }


    }

    bool CanMove(Cell toCheck)
    {

        if (toCheck.type == CellType.Wall)
        {
            return false;
        }


        else return true;
    }
    public  Cell ConvertToCell(Vector3 vector)// Mapping of position to Cell
    {
        
        return mapData[(int)vector.z, (int)vector.x];
    }
   





}
