using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLoader : MonoBehaviour
{
    public AudioManager theAM;
    private void Awake()
    {
        if (AudioManager.Instance == null)
        {
            AudioManager newAM = Instantiate(theAM);
            AudioManager.Instance = newAM;
            DontDestroyOnLoad(theAM);
        }
    }
}
