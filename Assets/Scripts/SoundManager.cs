using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    private AudioSource audioSource;
    public bool sound;
    private void Awake() 
    {
        makeSingleton();
        audioSource=GetComponent<AudioSource>();
    }
    void Start()
    {
        
    }
     private void makeSingleton()
    {
        if(instance!=null)
        {
            Destroy(gameObject);
        }else
        {
            instance=this;
            DontDestroyOnLoad(this);
        }
    }
    
    void Update()
    {
        
    }
    public void SoundOnOff()
    {
        sound =!sound;
    }
    public void playSoundFX(AudioClip clip,float volume)
    {
        if(sound)
        {
            audioSource.PlayOneShot(clip,volume);
        }
    }
}
