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

    private NetworkPlayer pPlayer;

    void Awake()
    {
        DCMenu.SetActive(false);
    }

    void Start()
    {
        NetworkDCMenu.isOn = false;
    }

    void Update()
    {
        //Normally I wouldn't put this here, but for PVP it makes sense
        float h = pPlayer.GetCurrentHealth() / 100;
        Debug.Log(h);
        SetHealthbar(h);

        //Disconnect menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleDCMenu();
        }
    }

    void ToggleDCMenu()
    {
        DCMenu.SetActive(!DCMenu.activeSelf);
        NetworkDCMenu.isOn = DCMenu.activeSelf;
    }

    //Our health is relative to the X axis
    void SetHealthbar(float _health)
    {
        //if (_health <= 0)
        //    _health = 0.01f;
        healthbar.localScale = new Vector3(_health, 1f, 1f);
    }

    //Link player and UI
    public void SetPlayer(NetworkPlayer _player)
    {
        pPlayer = _player;
    }
}
