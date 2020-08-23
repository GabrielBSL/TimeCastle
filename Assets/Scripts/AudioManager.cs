using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager instance;

    private bool reverseBGM;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }

        reverseBGM = false;
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
            return;

        if (!s.source.isPlaying)
            s.source.Play();
    }

    public void Volume(string name, float volume)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
            return;

        s.source.volume = volume;
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if(s.source.isPlaying)
            s.source.Stop();

        else
        {
            name = name + "_reversed";
            Sound r = Array.Find(sounds, sound => sound.name == name);

            r.source.Stop();
        }

    }

    public void Pitch(string name, float pitch)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (pitch < 0 && !reverseBGM)
        {
            name = name + "_reversed";

            Sound r = Array.Find(sounds, sound => sound.name == name);

            r.source.Play();
            r.source.time = r.source.clip.length - s.source.time;

            s.source.Stop();
            r.source.pitch = pitch * -1f;

            reverseBGM = true;
            return;
        }

        else if (pitch < 0 && reverseBGM)
        {
            name = name + "_reversed";

            Sound r = Array.Find(sounds, sound => sound.name == name);

            r.source.pitch = pitch * -1f;
            return;
        }

        else if (pitch >= 0 && reverseBGM)
        {
            name = name + "_reversed";

            Sound r = Array.Find(sounds, sound => sound.name == name);

            s.source.Play();
            s.source.time = s.source.clip.length - r.source.time;

            r.source.Stop();
            s.source.pitch = pitch;

            reverseBGM = false;
            return;
        }

        s.source.pitch = pitch;
    }

    public bool MusicIsPlaying(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        return s.source.isPlaying;
    }

    public void TickTock(bool slowing)
    {
        Sound s = Array.Find(sounds, sound => sound.name == "TickTock");

        if (!s.source.isPlaying)
        {
            if (slowing)
            {
                s.source.Play();
                s.source.pitch = 0.5f;
            }
            else
            {
                s.source.Play();
                s.source.pitch = 1f;
            }
        }
    }

    public void PlaySound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (!s.source.isPlaying)
        {
            s.source.Play();
            s.source.pitch = 1f;
        }
    }
}