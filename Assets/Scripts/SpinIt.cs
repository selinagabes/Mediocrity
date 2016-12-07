using UnityEngine;
using System.Collections;

public class SpinIt : MonoBehaviour {
    public float _speed;

    void Update()
    {
        transform.Rotate(Time.deltaTime * 30, Time.deltaTime * 60, Time.deltaTime * 90);
    }
}
