using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public PackManPlayer player;
    static Cell[,] mapDate = GameManager.mapData;
    public Cell currentCell;
    // Start is called before the first frame update
    

   virtual public void PickupDot()
    {
        Destroy(gameObject);
    }
}
