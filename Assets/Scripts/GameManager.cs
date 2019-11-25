using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


//!IMPORTANT!! DUE TO THE FACT THAT IN PROGRAMMING WE FIRST ITERATE MATRIX BY Y AND THEN BY X (THUS INDEXES OF OBJECTS ARE [Y,X]
// BUT REAL MATRICES HAVE INDEXES [X,Y], YOU WILL SEE IN A CODE SOME OCCURENCES THAT X COORIDNATE=Y AND Y COORDINATE =X.
// PLEASE BE CAREFULL
public class GameManager : MonoBehaviour
{
    public GameObject Wall, Box, Player, Exit, Floor;
    private GameObject SuperParentContainer;
    private MainPlayer mainPlayer;
    public static Cell[,] mapData;
    public  int Score = 0;
    public  int MaxScore = 1;
    public Text ScoreText;
    public Text PlayerMovementText;
    public Text PlayerPushText;
    public Text WinText;
    
    // Start is called before the first frame update
    void Start()
    {
        
       
        //WinText.enabled = false;
        WinText.gameObject.SetActive(false);
        SuperParentContainer = new GameObject("Container");
        CreateField();
      

    }

    // Update is called once per frame
    void Update()
    {
        ScoreText.text =   Score + " Boxes are is place out of " + MaxScore;
        if (Score >= MaxScore)
        {

            WinText.gameObject.SetActive(true);
        }
    }


    //Down here are all methods to create field and set camera
    void CreateField()
    {


        string levelString = Level.levelString1;
        string[] lines = levelString.Split('\n');

        mapData = new Cell[lines[1].Length, lines.Length];
        for (int i = 1; i < lines.Length; i++)
        {
            lines[i] = lines[i].Trim('\r');
            var chArr = lines[i].ToCharArray();
            for (int j = 0; j < chArr.Length; j++)
            {
                InstantiateCell(chArr[j], i, j);
            }
        }
        // Camera.main.transform.position = new Vector3(1 * lines.Length / 2, 0, 1 * lines[0].Length / 2);
        SetCamera(lines.Length / 2, 8, lines[1].Length / 2);
    }


    void InstantiateCell(char value, int x, int y)
    {
        GameObject cellToInstantiate = null;
        CellType? instantiatedType = null;
        GameObject instantiatedBox = null;
        switch (value)
        {
            case '.':
                {
                    cellToInstantiate= Instantiate(Floor, new Vector3(1 * x, 0, 1 * y), Quaternion.identity);
                    instantiatedType = CellType.Floor;
                    break;
                }
            case 'w':
                {
                    cellToInstantiate= Instantiate(Wall, new Vector3(1 * x, 0, 1 * y), Quaternion.identity);
                    instantiatedType = CellType.Wall;
                    break;
                }
            case 'b':
                {
                    cellToInstantiate = Instantiate(Floor, new Vector3(1 * x, 0, 1 * y), Quaternion.identity);

                    instantiatedBox = Instantiate(Box, new Vector3(1 * x, 0, 1 * y), Quaternion.identity);
                    instantiatedType = CellType.Box;
                    break;
                }
            case 'e':
                {
                    cellToInstantiate= Instantiate(Exit, new Vector3(1 * x, 0, 1 * y), Quaternion.identity);
                    instantiatedType = CellType.TargetSpot;
                    break;
                }
            case 'p':
                {
                    cellToInstantiate=Instantiate(Floor, new Vector3(1 * x, 0, 1 * y), Quaternion.identity, SuperParentContainer.transform);
                    var playerGameobject = Instantiate(Player, new Vector3(1 * x, 0, 1 * y), Quaternion.identity);
                    mainPlayer = playerGameobject.GetComponent<MainPlayer>();
                    instantiatedType = CellType.Floor;
                    mainPlayer.PlayerMovementText = PlayerMovementText;
                    mainPlayer.PlayerPushText = PlayerPushText;
                    mapData[y, x] = new Cell { Item = cellToInstantiate, Type = instantiatedType.Value, XCoordinate = y, YCoordinate = x };
                    mainPlayer.CellPosition = mapData[y, x];
                    Debug.Log(mainPlayer.CellPosition);
                    break;
                }
            default:
                {
                    Debug.Log("Can't instantiate");
                    break;
                }
        }
        mapData[y, x] = new Cell { Item = instantiatedBox, Type = instantiatedType.Value, XCoordinate = y, YCoordinate = x };
        cellToInstantiate.transform.SetParent(SuperParentContainer.transform);

    }

    void SetCamera(int xOffset, int yOffset, int zOffset)
    {
        Camera.main.transform.position = new Vector3(1 * xOffset, yOffset, 1 * zOffset);
        Camera.main.transform.rotation = Quaternion.Euler(65f, -90f, 0f);
        Camera.main.transform.position += Vector3.right * 5;
        Camera.main.fieldOfView += 1.5f*xOffset;
    }


   public  void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

   
}
