using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Box : MonoBehaviour
{
    public Cell[,] mapData = GameManager.mapData;
    private List<Cell> finishCells = new List<Cell>();
    public Text Score;
    private bool isInsidePlacementPoint = false;
    GameManager Manager;
    // Start is called before the first frame update
    void Start()
    {

        for (int i = 1; i < mapData.GetLength(1); i++)
        {
            for (int j = 0; j < mapData.GetLength(0) - 1; j++)
            {

                if (mapData[j, i].type == CellType.TargetSpot)
                    finishCells.Add(mapData[j, i]);
            }
        }
        for (int i = 0; i < finishCells.Count; i++)
        {
            Debug.Log(finishCells[i]);
        }
        Manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        Manager.MaxScore = finishCells.Count;
    }

    // Update is called once per frame
    void Update()
    {
        var inFinishPlace = finishCells.Exists(cell => cell.yCoordinate == transform.position.x && cell.xCoordinate == transform.position.z);
        if (inFinishPlace && !isInsidePlacementPoint)
        {
            Manager.Score++;
            isInsidePlacementPoint = true;
        }
        if (isInsidePlacementPoint)
        {
            if (!inFinishPlace)
            {
                Manager.Score--;
                isInsidePlacementPoint = false;
            }
        }
    }
}
