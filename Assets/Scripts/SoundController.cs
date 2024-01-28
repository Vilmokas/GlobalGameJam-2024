using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    private static SoundController _instance;
    public static SoundController Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("Game Manager is Null");

            }
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }

    [SerializeField] AudioSource audioSource;
    [SerializeField] List<AudioClip> audioClips = new List<AudioClip>();

    public void PlayAudioClip(int clipID)
    {
        audioSource.PlayOneShot(audioClips[clipID]);
    }
}
