using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//===============================
// Here we control everything UI
// related for the network
//===============================
public class NetworkPlayerUI : MonoBehaviour {

    [SerializeField]
    GameObject DCMenu;

    [SerializeField]
    RectTransform healthbar;

    [SerializeField]
    Text PointsText;

    [SerializeField]
    Text TimeText;

    private bool gameOver = false;

    private NetworkToothSpawner GameMaster;

    private NetworkPlayer pPlayer;

    void Awake()
    {
        DCMenu.SetActive(false);
    }

    void Start()
    {
        NetworkDCMenu.isOn = false;
        GameMaster = GameObject.Find("ToothSpawner").GetComponent<NetworkToothSpawner>();
    }

    void Update()
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
            if(!gameOver)
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

        //Display UI

        //Kill Connection

        //only call this once
        gameOver = true;
    }
}
