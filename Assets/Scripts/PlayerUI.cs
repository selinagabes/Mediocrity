using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class PlayerUI : MonoBehaviour {

    [SerializeField]
    RectTransform lucidFill;
    [SerializeField]
    Text ammo;
    [SerializeField]
    private float lucidPoints;
    [SerializeField]
    private int teeth;
    [SerializeField]
    GameObject Time;
    [SerializeField]
    Text timeText;
    PlayerController pPlayer;
    PanicManager pManage;

    void Start()
    {
        pPlayer = GameObject.FindGameObjectsWithTag("Player").First(p => p.activeSelf).GetComponent<PlayerController>();

        if (Application.loadedLevelName == "Panic")
        {
            Time.SetActive(true);
            pManage = GameObject.Find("Panic Manager").GetComponent<PanicManager>();
        }
    }
    void Update()
    {
        //if (pPlayer != null)
        //{
        //    Debug.Log("Brap Jayah");
        //    //===================================
        //    // Every Frame, update health, teeh
        //    //===================================
        //    lucidPoints = pPlayer.GetLPs();
        //    lucidPoints *= 20; //For scaling up the bar 
        //    lucidPoints /= 200;
        //    SetLucidity(lucidPoints);

        //    teeth = pPlayer.GetTeeth();
        //    SetTeeth(teeth);
        //}
        //else
        //{
        //    SetLucidity(.5f);
        //    ammo.text = "Game Loading";
        //}

        if (Application.loadedLevelName == "Panic")
        {
            SetTime(pManage.GetSeconds());
        }
    }

    public void SetLucidity(float _lucidity)
    {
        lucidFill.localScale = new Vector3(_lucidity, 1f, 1f);
    }

    public void SetTeeth(int _amt)
    {
        ammo.text = "Teeth: " + _amt;
    }

    public void SetTime(int time)
    {
        int t = (30 - time);
        if (t < 0)
            t = 0;
        timeText.text = "Time Left: " + t.ToString("00");
    }
}
