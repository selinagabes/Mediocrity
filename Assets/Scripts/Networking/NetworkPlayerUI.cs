using UnityEngine;
using System.Collections;

public class NetworkPlayerUI : MonoBehaviour {

    [SerializeField]
    GameObject DCMenu;

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
}
