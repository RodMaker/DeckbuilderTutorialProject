using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsManager : MonoBehaviour
{
    private AudioManager audioManager;
    
    public bool muteAudio = false;

    // Start is called before the first frame update
    void Start()
    {
        audioManager = GameManager.Instance.AudioManager;
    }

    // Update is called once per frame
    void Update()
    {
        if (muteAudio)
        {
            audioManager.enabled = false;
        }
    }
}
