using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioButtonUI : MonoBehaviour
{
    AudioSource audSource;
    Button audButton;
    public AudioClip customAudClip;
    
    // Start is called before the first frame update
    void Start()
    {
        audSource = GameObject.Find("Audio Source").GetComponent<AudioSource>();
        audButton = GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        if (audSource.isPlaying)
        {
            audButton.interactable = false;
        }
        else { audButton.interactable = true; }
    }

    public void StopOnClick()
    {
        audSource.Stop();
    }

    internal void SetAudioClip(AudioClip custom)
    {
        customAudClip = custom;
    }
    public void PlayOtherAud()
    {
        audSource.PlayOneShot(customAudClip);
    }
}
