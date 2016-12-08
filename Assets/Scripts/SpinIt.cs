using UnityEngine;
using System.Collections;

//================================
// Spinning something like teeth
// can be controlled by the client
// because it has no effect on the
// gameplay itself.
//================================

public class SpinIt : MonoBehaviour 
{
    void Update()
    {
        transform.Rotate(Time.deltaTime * 30, Time.deltaTime * 60, Time.deltaTime * 90);
    }
}
