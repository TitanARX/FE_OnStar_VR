using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class AudioModelProperty
{
    [Header("Audio Clip Key")]
    public string _key = "";

    [Header("Audio Properties")]
    public AudioDataProperty _audioModel;
}

[System.Serializable]
public class AudioDataProperty
{
    [Header("Audio Clip")]
    public AudioClip _audioClip;

    [Header("Audio Mixer Group")]
    public AudioMixerGroup _mixerGroup;

    [Header("Properties")]
    public bool _fadeClip, _loopclip, _isSpatial;

    public float _volume, _minDistance, _maxDistane;

    [Header("Spatial Scene Object")]
    public Transform _spatialTransform;
    
    [Header("Assigned AudioSource")]
    public AudioSource _audioSource;
}


public class AudioController : MonoBehaviour
{
    [Header("Debug")]
    private bool ProvidedKeyFound = false;
    private bool globalMute = false;

    [Header("Audio Mixer")]
    public AudioMixer GetAudioMixer;

    [Header("Audio Model")]
    public List<AudioModelProperty> AudioModelProperties = new List<AudioModelProperty>();

    private Dictionary<string, AudioDataProperty> AudioDictionary = new Dictionary<string, AudioDataProperty>();

    [Header("Generated Audio Sources")]
    private List<AudioSource> CachedAudioSources = new List<AudioSource>();

    private Dictionary<string, AudioSource> CreatedAudioSources = new Dictionary<string, AudioSource>();


    private void Awake()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        SetSpatialMode();
    }

    private void Init()
    {
        foreach (AudioModelProperty modelProperty in AudioModelProperties)
        {
            AudioDictionary.Add(modelProperty._key.ToString(), modelProperty._audioModel);
        }
    }

    private void SetSpatialMode()
    {
        //TODO
    }

    public void Play(string _dictionaryKey)
    {
        //Temp Audio Source
        AudioSource cachedAudioSource;

        //Cahce Condition In Bool
        ProvidedKeyFound = CreatedAudioSources.ContainsKey(_dictionaryKey);

        if(ProvidedKeyFound == false)
        {
            if(AudioDictionary[_dictionaryKey]._spatialTransform)
            {
                cachedAudioSource = AudioDictionary[_dictionaryKey]._spatialTransform.gameObject.AddComponent<AudioSource>();
            }
            else
            {
                cachedAudioSource = this.gameObject.AddComponent<AudioSource>();
            }

            foreach (AudioModelProperty audioModelProperty in AudioModelProperties)
            {
                if(audioModelProperty._key == _dictionaryKey)
                {
                    audioModelProperty._audioModel._audioSource = cachedAudioSource;
                }
            }

            CachedAudioSources.Add(cachedAudioSource);

            SetAudioSourceProperties(_dictionaryKey, cachedAudioSource);

            CreatedAudioSources.Add(_dictionaryKey, cachedAudioSource);

            cachedAudioSource.clip = ReturnedAudiosource(_dictionaryKey);

            if(cachedAudioSource.loop == false)
            {
                cachedAudioSource.PlayOneShot(cachedAudioSource.clip);
            }
            else
            {
                float targetVolume = AudioDictionary[_dictionaryKey]._volume;

                bool fade = AudioDictionary[_dictionaryKey]._fadeClip;

                if(fade)
                {
                    AudioDictionary[_dictionaryKey]._volume = 0;

                    cachedAudioSource.DOFade(0, 0).OnComplete(() =>
                     {
                         DOTween.To(() => AudioDictionary[_dictionaryKey]._volume, x => AudioDictionary[_dictionaryKey]._volume = x, targetVolume, 10.0f);

                         cachedAudioSource.Play();
                     });
                }
                else
                {
                    cachedAudioSource.volume = targetVolume;

                    cachedAudioSource.Play();
                }


            }
        }
        else
        {
            PriorAudioSources(_dictionaryKey).Play();
        }
    }

    private void SetAudioSourceProperties(string dictionaryKey, AudioSource audioSource)
    {
        audioSource.mute = globalMute;

        audioSource.playOnAwake = false;

        audioSource.outputAudioMixerGroup = AudioDictionary[dictionaryKey]._mixerGroup;

        audioSource.loop = AudioDictionary[dictionaryKey]._loopclip;

        if(audioSource.loop)
        {
            audioSource.volume = AudioDictionary[dictionaryKey]._fadeClip ? audioSource.volume = 0 : audioSource.volume = 1;
        }
        else
        {
            AudioDictionary[dictionaryKey]._fadeClip = false;

            audioSource.volume = 1;
        }
    }


    private AudioClip ReturnedAudiosource(string dictionaryKey)
    {

        if(AudioDictionary.TryGetValue(dictionaryKey, out AudioDataProperty audioData))
        {
            return audioData._audioClip;
        }

        Debug.Log("Provided Key " + dictionaryKey + " does not exist..");

        return null;

    }


    private AudioSource PriorAudioSources(string dictionaryKey)
    {
        if(CreatedAudioSources.TryGetValue(dictionaryKey, out AudioSource source))
        {
            return source;
        }

        Debug.Log("Provided Key " + dictionaryKey + " does not exist..");

        return null;
    }


    public void Mute(string dictionaryKey)
    {
        AudioSource source = PriorAudioSources(dictionaryKey);

        if(source == null)
        {
            Debug.Log("Mute call ignored. The key" + dictionaryKey + " does not exxist");
        }
        else
        {
            source.mute = true;
        }
    }

    public void UnMute(string dictionaryKey)
    {
        AudioSource source = PriorAudioSources(dictionaryKey);

        if (source == null)
        {
            Debug.Log("Mute call ignored. The key" + dictionaryKey + " does not exxist");
        }
        else
        {
            source.mute = false;
        }
    }

    public void StopAudio(string dictionaryKey)
    {
        AudioSource source = PriorAudioSources(dictionaryKey);

        if (source == null)
        {
            Debug.Log("Stop call ignored. The key" + dictionaryKey + " does not exxist");
        }
        else
        {
            source.DOFade(0, 1.75f).OnComplete(() => CreatedAudioSources.Remove(dictionaryKey));
        }
    }

    public void GlobalStop()
    {
        foreach (AudioSource source in CachedAudioSources)
        {
            if(source)
            {
                source.DOFade(0, 0.75f).OnComplete(() => source.Stop());
            }
        }
    }


}
