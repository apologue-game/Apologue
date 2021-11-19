using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    string oldAudioState;
    GameObject soundGameObject;
    GameObject[] soundGameObjectArray;

    private void Start()
    {
        soundGameObjectArray = new GameObject[sounds.Length];
        for (int i = 0; i < sounds.Length; i++)
        {
            soundGameObject = new GameObject();
            //Instantiate(soundGameObject);
            soundGameObject.name = sounds[i].soundName;
            soundGameObject.AddComponent<AudioSource>();
            soundGameObject.GetComponent<AudioSource>().clip = sounds[i].clip;
            soundGameObject.GetComponent<AudioSource>().volume = sounds[i].volume;
            soundGameObject.GetComponent<AudioSource>().pitch = sounds[i].pitch;
            soundGameObject.GetComponent<AudioSource>().loop = sounds[i].isLooping;
            soundGameObjectArray[i] = soundGameObject;
        }

        PlaySound("darkVillageTheme");
    }

    public void PlaySound(string soundName)
    {
        GameObject sound = Array.Find(soundGameObjectArray, sound => sound.name == soundName);
        if (sound == null || soundName == oldAudioState)
        {
            return;
        }

        sound.GetComponent<AudioSource>().Play();

        oldAudioState = soundName;
    }
    public void PlaySoundVolume(string soundName)
    {
        GameObject sound = Array.Find(soundGameObjectArray, sound => sound.name == soundName);
        if (sound == null || soundName == oldAudioState)
        {
            return;
        }
        sound.GetComponent<AudioSource>().volume = 1;

        oldAudioState = soundName;
    }


    public void StopSound(string soundName)
    {
        GameObject sound = Array.Find(soundGameObjectArray, sound => sound.name == soundName);
        if (sound == null || soundName == oldAudioState)
        {
            return;
        }

        sound.GetComponent<AudioSource>().Stop();

        oldAudioState = soundName;
    }
}
