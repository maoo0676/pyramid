using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("#BGM")]
    public AudioClip[] bgmClip;
    public float bgmVolume;
    public int bgmChannels;
    AudioSource[] bgmPlayer;
    AudioHighPassFilter bgmEffect;

    [Header("#SFX")]
    public AudioClip[] sfxClips;
    public float sfxVolume;
    public int sfxChannels;
    AudioSource[] sfxPlayers;
    int channelIndex;

    public enum Sfx { Attack, Hit, Get, Active}

    void Awake()
    {
        instance = this;
        Init();
    }

    void Init()
    {
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;
        bgmPlayer = new AudioSource[bgmChannels];

        for (int i = 0; i < bgmPlayer.Length; i++)
        {
            bgmPlayer[i] = bgmObject.AddComponent<AudioSource>();
            bgmPlayer[i].playOnAwake = false;
            bgmPlayer[i].loop = true;
            bgmPlayer[i].volume = bgmVolume;
            bgmPlayer[i].clip = bgmClip[i];
            bgmEffect = Camera.main.GetComponent<AudioHighPassFilter>();
        }

        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[sfxChannels];

        for (int index = 0; index < sfxPlayers.Length; index++) {
            sfxPlayers[index] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[index].playOnAwake = false;
            sfxPlayers[index].bypassListenerEffects = true;
            sfxPlayers[index].volume = sfxVolume;
        }
    }

    public void PlayBgm(bool isPlay)
    {
        if (isPlay && GameManager.Instance.mapId == 0)
        {
            bgmPlayer[1].Stop();
            bgmPlayer[0].Play();
        }
        else if (isPlay && GameManager.Instance.mapId != 0)
        {
            bgmPlayer[0].Stop();
            bgmPlayer[1].Play();

            if (GameManager.Instance.mapId == 4)
            {
                bgmPlayer[1].pitch = -1.2f;
            }
            else bgmPlayer[1].pitch = 1f;
        }
        else {
            bgmPlayer[0].Stop();
        }
    }

    public void EffectBgm(bool isPlay)
    {
        bgmEffect.enabled = isPlay;
    }

    public void PlaySfx(Sfx sfx)
    {
        for (int index = 0; index < sfxPlayers.Length; index++) {
            int loopIndex = (index + channelIndex) % sfxPlayers.Length;

            if (sfxPlayers[loopIndex].isPlaying)
                continue;
            
            channelIndex = loopIndex;
            sfxPlayers[loopIndex].clip = sfxClips[(int)sfx];
            sfxPlayers[loopIndex].Play();
            break;
        }

    }
}
