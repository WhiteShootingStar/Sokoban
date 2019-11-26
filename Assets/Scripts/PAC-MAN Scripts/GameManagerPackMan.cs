using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


//!IMPORTANT!! DUE TO THE FACT THAT IN PROGRAMMING WE FIRST ITERATE MATRIX BY Y AND THEN BY X (THUS INDEXES OF OBJECTS ARE [Y,X]
// BUT REAL MATRICES HAVE INDEXES [X,Y], YOU WILL SEE IN A CODE SOME OCCURENCES THAT X COORIDNATE=Y AND Y COORDINATE =X.
// PLEASE BE CAREFULL
public class GameManagerPackMan : MonoBehaviour
{
    [Header("Map Obstacles")]
    public GameObject Wall, Box, Player, Exit, Floor, Gate;
    private GameObject SuperParentContainer, SuperCoinParentContainer;
    public static PackManPlayer mainPlayer;
    public static Cell[,] mapData;
    public int Score = 0;
    public static Cell EyesCell;
    public Text ScoreText;

    public Text WinText;

    [Header("Ghosts")]
    public Blinky blinky;
    public Pinky pinky;
    public Inky inky;
    public Clyde clyde;
    private List<AbstractGhost> ghosts = new List<AbstractGhost>();

    [Header("Pick-Ups")]
    public Coin coin;
    public SuperCoin superCoin;
    public static Coin[,] coins;
    public static int coinsTotal = 0;

    [Header("Restart")]
    public Button RestartButton;
    public Text YouAreDead;
    public static GameState state = GameState.Alive;
    private float spawnTimer = 0f;
    private float unscareTimer = 10f;
    private bool allowUnscareTimerToDecrease = false;
    // Start is called before the first frame update
    void Start()
    {

        state = GameState.Alive;
        //WinText.enabled = false;
        WinText.gameObject.SetActive(false);
        SuperParentContainer = new GameObject("Container");
        SuperCoinParentContainer = new GameObject("Coin Container");


        CreateField();
        CreateCoins();
        ghosts.Add(inky);
        ghosts.Add(blinky);
        ghosts.Add(pinky);
        ghosts.Add(clyde);
    }

    // Update is called once per frame
    void Update()
    {


        spawnTimer += Time.deltaTime;
        ActivateGhost(blinky, 0f);
        ActivateGhost(pinky, 5f);
        ActivateGhost(inky, 10f);
        ActivateGhost(clyde, 15f);
        if (allowUnscareTimerToDecrease) unscareTimer -= Time.deltaTime;
        if (unscareTimer <= 7f && unscareTimer > 6.5f) GhostsAreBlinking();
        if (unscareTimer <= 0f) GhostsNotScaredAnymore();
        //WinText.gameObject.SetActive(true);

    }


    //Down here are all methods to create field and set camera
    void CreateField()
    {


        string levelString = Level.PackManString1;
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
        SetCamera(mapData[lines[1].Length / 2, lines.Length / 2]);
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
                    cellToInstantiate = Instantiate(Floor, new Vector3(1 * x, 0, 1 * y), Quaternion.identity);
                    instantiatedType = CellType.Floor;
                    break;
                }
            case 'g':
                {
                    cellToInstantiate = Instantiate(Gate, new Vector3(1 * x, 0, 1 * y), Quaternion.identity);
                    Gate = cellToInstantiate;
                    EyesCell = new Cell { XCoordinate = y, YCoordinate = x - 1 };
                    instantiatedType = CellType.Gate;
                    break;
                }
            case 'w':
                {
                    cellToInstantiate = Instantiate(Wall, new Vector3(1 * x, 0, 1 * y), Quaternion.identity);
                    instantiatedType = CellType.Wall;
                    break;
                }
            case 'b':
                {
                    cellToInstantiate = Instantiate(Floor, new Vector3(1 * x, 0, 1 * y), Quaternion.identity);

                    blinky = Instantiate(blinky, new Vector3(1 * x, 0, 1 * y), Quaternion.identity);
                    instantiatedType = CellType.Floor;
                    mapData[y, x] = new Cell { Item = cellToInstantiate, Type = instantiatedType.Value, XCoordinate = y, YCoordinate = x };
                    blinky.currentCell = mapData[y, x];
                    break;
                }
            case 'p':
                {
                    cellToInstantiate = Instantiate(Floor, new Vector3(1 * x, 0, 1 * y), Quaternion.identity);
                    instantiatedType = CellType.Floor;
                    pinky = Instantiate(pinky, new Vector3(1 * x, 0, 1 * y), Quaternion.identity);
                    mapData[y, x] = new Cell { Item = cellToInstantiate, Type = instantiatedType.Value, XCoordinate = y, YCoordinate = x };
                    pinky.currentCell = mapData[y, x];
                    break;
                }
            case 'i':
                {
                    cellToInstantiate = Instantiate(Floor, new Vector3(1 * x, 0, 1 * y), Quaternion.identity);
                    instantiatedType = CellType.Floor;
                    inky = Instantiate(inky, new Vector3(1 * x, 0, 1 * y), Quaternion.identity);
                    mapData[y, x] = new Cell { Item = cellToInstantiate, Type = instantiatedType.Value, XCoordinate = y, YCoordinate = x };
                    inky.currentCell = mapData[y, x];
                    break;
                }
            case 'c':
                {
                    cellToInstantiate = Instantiate(Floor, new Vector3(1 * x, 0, 1 * y), Quaternion.identity);
                    instantiatedType = CellType.Floor;
                    clyde = Instantiate(clyde, new Vector3(1 * x, 0, 1 * y), Quaternion.identity);
                    mapData[y, x] = new Cell { Item = cellToInstantiate, Type = instantiatedType.Value, XCoordinate = y, YCoordinate = x };
                    clyde.currentCell = mapData[y, x];
                    break;
                }
            case 'P':
                {
                    cellToInstantiate = Instantiate(Floor, new Vector3(1 * x, 0, 1 * y), Quaternion.identity, SuperParentContainer.transform);
                    var playerGameobject = Instantiate(Player, new Vector3(1 * x, 0, 1 * y), Quaternion.identity);
                    mainPlayer = playerGameobject.GetComponent<PackManPlayer>();
                    instantiatedType = CellType.Floor;
                    mainPlayer.ScoreText = ScoreText;
                    mainPlayer.RestartButton = RestartButton;
                    mainPlayer.DeadText = YouAreDead;
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

    void SetCamera(Cell middle)
    {
        Camera.main.transform.position = new Vector3(1 * middle.XCoordinate + 2, 1.5f * Mathf.Sqrt(middle.XCoordinate * middle.YCoordinate), 1 * middle.YCoordinate);
        Camera.main.transform.rotation = Quaternion.Euler(90f, -90f, 0f);
        //Camera.main.transform.position += Vector3.right * 5;
        Camera.main.fieldOfView += Mathf.Sqrt(middle.XCoordinate * middle.YCoordinate);
    }


    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void CreateCoins()
    {
        int counter = 1;
        coins = new Coin[mapData.GetLength(1), mapData.GetLength(0)];
        for (int i = 1; i < mapData.GetLength(0) - 1; i++)
        {
            for (int k = 1; k < mapData.GetLength(1) - 1; k++)
            {
                if (mapData[i, k].Type == CellType.Floor && !IsOccupiedByGhost(mapData[i, k]))
                {
                    Coin createdCoin = null;
                    if (counter % 35 != 0)
                    {
                        createdCoin = Instantiate(coin, new Vector3(k, 0.5f, i), Quaternion.identity, SuperCoinParentContainer.transform);
                        counter++;
                        Debug.Log("coin");
                    }
                    else if (counter % 35 == 0)
                    {
                        createdCoin = Instantiate(superCoin, new Vector3(k, 0.5f, i), Quaternion.identity, SuperCoinParentContainer.transform);
                        counter = 1;
                        Debug.Log("SuperCoin");
                    }
                    createdCoin.currentCell = mapData[i, k];
                    coins[k, i] = createdCoin;
                    coinsTotal += 1;
                }
            }
        }

    }

    bool IsOccupiedByGhost(Cell tocheck)
    {
        return tocheck.Equals(clyde.currentCell) || tocheck.Equals(inky.currentCell) ||
              tocheck.Equals(pinky.currentCell) || tocheck.Equals(blinky.currentCell);
    }

    void ActivateGhost(AbstractGhost ghost, float thresholdTimer)
    {
        Vector3Int spawnPosition = Vector3Int.FloorToInt(Gate.transform.position) + Vector3Int.left;
        if (ghost.state == GhostState.Disabled && spawnTimer >= thresholdTimer)
        {
            ghost.transform.position = spawnPosition;
            ghost.currentCell = ConvertToCell(spawnPosition);
            ghost.state = GhostState.Chase;
        }

    }

    public void ScareGhosts()
    {
        for (int i = 0; i < ghosts.Count; i++)
        {
            if (ghosts[i].state == GhostState.Chase)
            {
                ghosts[i].state = GhostState.Frightened;
            }
        }
        allowUnscareTimerToDecrease = true;
    }
    public void GhostsNotScaredAnymore()
    {
        for (int i = 0; i < ghosts.Count; i++)
        {
            if (ghosts[i].state == GhostState.Frightened)
            {
                ghosts[i].state = GhostState.Chase;


            }
        }
        allowUnscareTimerToDecrease = false;
        unscareTimer = 10f;
    }
    public void GhostsAreBlinking()
    {
        for (int i = 0; i < ghosts.Count; i++)
        {
            if (ghosts[i].state == GhostState.Frightened)
            {

                ghosts[i].Blink();

            }
        }
    }




    public Cell ConvertToCell(Vector3Int vector)// Mapping of position to Cell
    {
        if (vector.z < 0) vector.z = 0;
        if (vector.x < 0) vector.x = 0;
        if (vector.x >= mapData.GetLength(1) - 1) vector.x = mapData.GetLength(1) - 2;
        if (vector.z >= mapData.GetLength(0) - 1) vector.z = mapData.GetLength(0) - 2;
        return mapData[vector.z, vector.x];
    }

    public static bool HasWon()
    {
        return coinsTotal <= 0;
    }
}
public enum GameState
{
    Dead, Alive
}
