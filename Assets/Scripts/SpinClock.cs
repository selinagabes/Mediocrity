using UnityEngine;
using System.Collections;

public class SpinClock : MonoBehaviour {

    public float _speed;

    void Update()
    {
        transform.Rotate(0,0, Time.deltaTime * 180);
    }
}
