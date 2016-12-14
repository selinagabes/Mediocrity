using UnityEngine;
using System.Collections;

public class PanicManager : MonoBehaviour {

    PlayerController pPlayer;
    [SerializeField]
    float gameTimer = 0f;
    int seconds = 0;
    bool gameOn = false;

    public bool IsGameOn()
    {
        return gameOn;
    }

    public float GetGameTime()
    {
        return gameTimer;
    }

    public int GetSeconds()
    {
        return seconds;
    }

    void Start()
    {
        StartCoroutine(GetPlayer());
        gameTimer = Time.time;
    }

    IEnumerator GetPlayer()
    {
        yield return new WaitForSeconds(1f);
        pPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    void FixedUpdate()
    {
        //Start game
        if(pPlayer != null)
        {
            gameOn = true;
        }

        if(gameOn)
        {
            //Increment Timer
            if(Time.time >= gameTimer + 1)
            {
                gameTimer = Time.time;
                seconds++;
                Debug.Log(seconds);
            }
        }

        if(seconds >= 30)
        {
            gameOn = false;
            EndGame();
        }
    }

    void EndGame()
    {
        Debug.Log("Game Over");
    }
}
