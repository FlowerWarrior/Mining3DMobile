using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMgr : MonoBehaviour
{
    [SerializeField] AudioClip audioUI;
    [SerializeField] AudioClip audioSell;
    [SerializeField] AudioClip audioBuy;
    [SerializeField] AudioClip audioFullBackpack;
    [SerializeField] AudioClip audioDestroyed;
    [SerializeField] AudioClip audioNewLayer;
    [SerializeField] AudioClip[] audioHitArray;
    [SerializeField] AudioSource audioMusic;

    public static AudioMgr instance;

    float _minTemporalDistanceAudioFullBp = 0.3f;
    float _temporalTimer = 0f;

    void Awake()
    {
        instance = this;
        if (PlayerPrefs.GetInt("Music", 0) == 0)
        {
            audioMusic.enabled = false;
        }
    }

    void PlayAudio(AudioClip clip)
    {
        if (PlayerPrefs.GetInt("Sound", 1) == 1)
        {
            AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);
        }
    }

    public void PlayAudioUI()
    {
        PlayAudio(audioUI);
    }

    public void PlayAudioSell()
    {
        PlayAudio(audioSell);
    }

    public void PlayAudioHit()
    {
        int randIndex = Random.Range(0, audioHitArray.Length);
        PlayAudio(audioHitArray[randIndex]);
    }

    public void PlayAudioBlockDestroyed()
    {
        PlayAudio(audioDestroyed);
    }

    public void PlayAudioFullBackpack()
    {
        if (_temporalTimer <= 0)
        {
            PlayAudio(audioFullBackpack);
            _temporalTimer = _minTemporalDistanceAudioFullBp;
        }
    }

    private void Update()
    {
        if (_temporalTimer > 0)
        {
            _temporalTimer -= Time.deltaTime;
        }
    }

    public void PlayAudioNewLayer()
    {
        PlayAudio(audioNewLayer);
    }

    public void PlayAudioBuy()
    {
        PlayAudio(audioBuy);
    }

    public void SettingsUpdated()
    {
        if (PlayerPrefs.GetInt("Music") == 1)
        {
            audioMusic.enabled = true;
        }
        else
        {
            audioMusic.enabled = false;
        }
    }
}
