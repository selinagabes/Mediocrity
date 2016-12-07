using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {
    public AudioSource musicSource;
    public static SoundManager instance = null;

    // Use this for initialization
    void Awake() {

        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
	}
	
    public void PlaySingle(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.Play();
    }
	
	
}
