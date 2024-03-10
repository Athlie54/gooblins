using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager Instance;


    // Start is called before the first frame update
    // Awake, on the other hand, is called just before the Start() function would be
    void Awake()
    {
        Instance = this;
        foreach(Sound s in sounds) {
            //creates an audio source for every sound listed in the AudioManager inspector
            s.source= gameObject.AddComponent<AudioSource>();
            //finding the clip for each audio source
            s.source.clip = s.clip;

            // connecting the volume and pitch values for easier manipulation in the inspector
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    private void Start()
    {
        Play("LevelMusic");
    }

    /* This function finds a sound with the same name as the name entered into the function parameter.
     * It then plays the sound (if a sound with the same name is found).
     */
    public void Play (string name)
    {
        //Finding the sound such that the sound's name is equal to the given name
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
    }
}
