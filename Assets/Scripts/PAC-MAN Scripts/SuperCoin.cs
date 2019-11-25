using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperCoin : Coin
{
    private GameManagerPackMan gameManager;
    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManagerPackMan>();
    }
    public override void PickupDot()
    {
        Destroy(gameObject);
        gameManager.ScareGhosts();
    }


}
