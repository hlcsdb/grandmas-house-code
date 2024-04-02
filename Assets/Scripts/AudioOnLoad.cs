using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioOnLoad : MonoBehaviour
{
    public AudioClip audioOnActive;
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Awake()
    {
        audioSource = GameObject.Find("Audio Source").GetComponent<AudioSource>();
        audioSource.PlayOneShot(audioOnActive);
    }
    
    internal void SetAudioclip(AudioClip custom)
    {
        audioOnActive = custom;
    }

    public void PlayAudio()
    {
        StartCoroutine(PlayOnDelay());
        IEnumerator PlayOnDelay()
        {
            yield return new WaitUntil(() => audioOnActive != null);
            GameObject.Find("Audio Source").GetComponent<AudioSource>().PlayOneShot(audioOnActive);
        }
    }
}
