using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AudioClips { Click, BackClick, OnComplite, OnFail, OnEnter }

public class AudioManager : MonoBehaviour {

    public AudioClip OnClick;
    public AudioClip OnBackClick;
    public AudioClip OnComplite;
    public AudioClip OnFail;
    public AudioClip OnEnter;

    public AudioClip Background;

    private AudioSource source;

    #region Singleton
    private static AudioManager audioController;

    public static AudioManager Instance()
    {
        if (audioController == null)
            audioController = FindObjectOfType<AudioManager>();

        return audioController;
    }
    #endregion

    void Awake () {
        source = GetComponent<AudioSource>();
        source.PlayOneShot(Background);
        StartCoroutine(RestartBackground());
	}

    IEnumerator RestartBackground()
    {
        yield return new WaitForSeconds(Background.length - 3);
        source.PlayOneShot(Background);

        StartCoroutine(RestartBackground());
    }

    public void Play(AudioClips clip)
    {
        switch (clip)
        {
            case (AudioClips.Click): source.PlayOneShot(OnClick); break;
            case (AudioClips.BackClick): source.PlayOneShot(OnBackClick); break;
            case (AudioClips.OnComplite): source.PlayOneShot(OnComplite); break;
            case (AudioClips.OnFail): source.PlayOneShot(OnFail); break;
            case (AudioClips.OnEnter): source.PlayOneShot(OnEnter); break;

        }
    }
	

}
