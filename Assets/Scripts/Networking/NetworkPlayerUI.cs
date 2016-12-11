using UnityEngine;
using System.Collections;

//===============================
// Here we control everything UI
// related for the network
//===============================
public class NetworkPlayerUI : MonoBehaviour {

    [SerializeField]
    GameObject DCMenu;

    [SerializeField]
    GameObject ScoreBoard;

    private NetworkScoreboard SBScript;

    void Awake()
    {
        DCMenu.SetActive(false);
    }

    void Start()
    {
        NetworkDCMenu.isOn = false;
        SBScript = new NetworkScoreboard();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleDCMenu();
        }

        if(Input.GetKeyDown(KeyCode.Tab))
        {
            //Call the Scoreboard Script
            ScoreBoard.SetActive(true);
            SBScript.TurnOnSB();
        }
        else if (Input.GetKeyUp(KeyCode.Tab))
        {
            //Then turn it off!
            ScoreBoard.SetActive(false);
            SBScript.TurnOffSB();
        }

    }

    void ToggleDCMenu()
    {
        DCMenu.SetActive(!DCMenu.activeSelf);
        NetworkDCMenu.isOn = DCMenu.activeSelf;
    }
}
