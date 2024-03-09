using UnityEngine.Audio;
using UnityEngine;


//This means that the class is visible in the Inspector
[System.Serializable]
public class Sound
{
    public AudioClip clip;

    public string name;

    //Range makes a slider with the two parameters as either extreme
    [Range(0f,1f)]
    public float volume;
    [Range(0f,3f)]
    public float pitch;

    //this acts against the System.Serializable command, and hides the following variable from the Inspector
    [HideInInspector]
    public AudioSource source;
}
