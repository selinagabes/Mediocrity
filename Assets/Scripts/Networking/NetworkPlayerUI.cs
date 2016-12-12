using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;

//===============================
// Here we control everything UI
// related for the network
//===============================
public class NetworkPlayerUI : MonoBehaviour {

    [SerializeField]
    GameObject DCMenu;

    [SerializeField]
    GameObject EndGamePanel;

    [SerializeField]
    Text EndGameButton;

    [SerializeField]
    RectTransform healthbar;

    [SerializeField]
    Text PointsText;

    [SerializeField]
    Text TimeText;

    private bool gameOver = false;

    private NetworkToothSpawner GameMaster;

    private NetworkPlayer pPlayer;
    private NetworkPlayer[] pPlayers;

    void Awake()
    {
        DCMenu.SetActive(false);
        EndGamePanel.SetActive(false);
    }

    void Start()
    {
        NetworkDCMenu.isOn = false;
        GameMaster = GameObject.Find("ToothSpawner").GetComponent<NetworkToothSpawner>();
    }

    void Update()
    {
        if (!gameOver)
        {
            //Normally I wouldn't put this here, but for PVP it makes sense
            float h = pPlayer.GetCurrentHealth() / 100;

            //Set the Health Bar
            SetHealthbar(h);

            //Adjust Points
            SetPoints(pPlayer.GetPoints());

            //Gametime stuff.  Looks dirty but feels so clean
            int min = (120 - GameMaster.GetGameTime()) / 60;
            int sec = (120 - GameMaster.GetGameTime()) % 60;
            if (GameMaster.GetGameTime() >= 120)
                sec = 0;

            //Call the end game from here since we have a direct time reference!
            if (min <= 0 && sec <= 0)
            {
                if (!gameOver)
                    EndGUI();
            }

            //Display Time
            SetTimeLeft("Time Left: " + min + ":" + sec.ToString("00"));

            //Disconnect menu
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ToggleDCMenu();
            }
        }
    }

    //Switch The DC Menu on and off!
    void ToggleDCMenu()
    {
        DCMenu.SetActive(!DCMenu.activeSelf);
        NetworkDCMenu.isOn = DCMenu.activeSelf;
    }

    //Our health is relative to the X axis
    void SetHealthbar(float _health)
    {
        healthbar.localScale = new Vector3(_health, 1f, 1f);
    }

    //Set Points in the UI
    void SetPoints(int p)
    {
        PointsText.text = "" + p;
    }

    //Link player and UI
    public void SetPlayer(NetworkPlayer _player)
    {
        pPlayer = _player;
    }

    //UI Time
    void SetTimeLeft(string text)
    {
        TimeText.text = text;
    }

    //Show the End Gui!
    void EndGUI()
    {
        //First, Kill all controls
        pPlayer.GameOver();

        //Calculate Winner
        int[] points = new int[2];
        pPlayers = NetworkGameManager.GetPlayers();
        for (int i = 0; i < pPlayers.Length; i++)
        { 
            points[i] = pPlayers[i].GetPoints();
        }

        if(points[0] > points[1])
        {
            //First player won!
            //Check if its our instance!
            if (pPlayers[0].GetPoints() == pPlayer.GetPoints())
            {
                EndGameButton.text = "Game Over \n You WON!";
            }
            else
            {
                EndGameButton.text = "Game Over \n You LOST!";
            }
        }
        else if(points[1] > points[0])
        {
            if (pPlayers[1].GetPoints() == pPlayer.GetPoints())
            {
                EndGameButton.text = "Game Over \n You WON!";
            }
            else
            {
                EndGameButton.text = "Game Over \n You LOST!";
            }
        }
        else
        {
            //Tie Game
            EndGameButton.text = "Tie Game \n You All Suck";
        }

        //Display UI
        EndGamePanel.SetActive(true);

        //only call this once
        gameOver = true;

        //Call the DC routine
        StartCoroutine(EndAfterTime());
    }

    //Force the client to quit after the game is over!
    IEnumerator EndAfterTime()
    {
        Debug.Log("Game disconnecting in 5 seconds!");
        //6 seconds .. yeah I lied to the players!
        yield return new WaitForSeconds(6f);
        NetworkManager.singleton.StopHost();
    }
}
