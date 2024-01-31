using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsManager : MonoBehaviour
{
    public bool muteAudio = false;
    private AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        audioManager = GetComponent<AudioManager>();
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
