﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PackManPlayer : MonoBehaviour
{
    public Cell CellPosition = null;
    public Cell[,] mapData = GameManagerPackMan.mapData;
    public Coin[,] coins = GameManagerPackMan.coins;

    public Vector3 destination = Vector3Int.zero;
    public Vector3Int currentDirection = Vector3Int.zero; // either Vector3 up,down,left,right
    public Vector3Int nextDirection = Vector3Int.zero;


    public Text ScoreText,DeadText;
    public Button RestartButton;
    private int score = 0;
    private bool WIN=false;

    private readonly float precision = 0.0001f;
    // Start is called before the first frame update
    void Start()
    {
        destination = transform.position;
        RestartButton.gameObject.SetActive(false);
        DeadText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManagerPackMan.state == GameState.Alive)
        {
            Move();
        }
        else
        {
            RestartButton.gameObject.SetActive(true);
            DeadText.gameObject.SetActive(true);
        }
        if (WIN)
        {
            ScoreText.text = "You won" ;

        }
        else
        {


            ScoreText.text = score + "";
        }

       
    }
    void Move()
    {

        transform.position = Vector3.MoveTowards(transform.position, destination, 4f *Time.deltaTime);
        CellPosition = mapData[(int)destination.z, (int)destination.x];

        if (Input.GetKey(KeyCode.W))
        {
            nextDirection = Vector3Int.left;

        }
        if (Input.GetKey(KeyCode.S))
        {
            nextDirection = Vector3Int.right;

        }
        if (Input.GetKey(KeyCode.A))
        {
            nextDirection = new Vector3Int(0, 0, -1);

        }
        if (Input.GetKey(KeyCode.D))
        {
            nextDirection = new Vector3Int(0, 0, 1);

        }
        if (Vector3.Distance(destination, transform.position) <= precision)//
        {
            CollectCoin();
            if (CanMove(ConvertToCell(transform.position + nextDirection)))
            {
                destination = transform.position + nextDirection;
                currentDirection = nextDirection;
            }
            else
            {

                if (CanMove(ConvertToCell(transform.position + currentDirection)))
                {
                    destination = transform.position + currentDirection;
                }
            }

        }


    }

    bool CanMove(Cell toCheck)
    {

        if (toCheck.Type == CellType.Floor)
        {
            return true;
        }


        else return false;
    }
    public Cell ConvertToCell(Vector3 vector)// Mapping of position to Cell
    {

        return mapData[(int)vector.z, (int)vector.x];
    }

    void CollectCoin()
    {
        if (GameManagerPackMan.coins[CellPosition.YCoordinate, CellPosition.XCoordinate] != null)
        {
            GameManagerPackMan.coins[CellPosition.YCoordinate, CellPosition.XCoordinate].PickupDot();

            // GameManagerPackMan.coins[CellPosition.YCoordinate, CellPosition.XCoordinate] = null;
            GameManagerPackMan.coinsTotal -= 1;
            score += 10;
            WIN=GameManagerPackMan.HasWon();
        }
    }





}
