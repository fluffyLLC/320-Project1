using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioClip[] audioClips;
    public AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        if (!audioSource.isPlaying) {
            int nextSong = Random.Range(0, audioClips.Length - 1);

            audioSource.clip = audioClips[nextSong];
            audioSource.Play();
        
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
