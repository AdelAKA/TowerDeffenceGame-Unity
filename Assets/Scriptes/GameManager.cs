using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool gameIsOver;

    public GameObject gameOverUI;

    void Start()
    {
        gameIsOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("e"))
        {
            EndGame();
        }

        if (gameIsOver)
        {
            return;
        }
        if (PlayerStatus.Lives <= 0)
        {
            EndGame();
        }
    }

    void EndGame()
    {
        gameIsOver = true;
        gameOverUI.SetActive(true);
    }
}
