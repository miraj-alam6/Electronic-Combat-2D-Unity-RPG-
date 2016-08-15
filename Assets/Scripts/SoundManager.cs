using UnityEngine;
using System.Collections;

//Gonna use the same singleton pattern that we used in GameManager here.
public class SoundManager : MonoBehaviour {
    public AudioSource efxSource1;
    public AudioSource efxSource2;
    public AudioSource efxSource3;
    public AudioSource efxSource4;
    public AudioSource musicSource;
    public static SoundManager instance = null;

    public float lowPitchRange = .95f;
    public float highPitchRange = 1.05f;
    //the two above represent + or - 5 % of our original pitch

    // Use this for initialization
    void Awake () {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this) {
            Destroy(gameObject);
        }
        
        DontDestroyOnLoad(gameObject); //The music does not get reset as we switch levels because
        //we do dontdestroyonload. This may super helpful for future reference

	}

    //Will be called from our other scripts
    //AudioClips are assets that contain digital recordings
    //sounds will be played in 1 of 4 different channels
    public void PlaySingle(int channel, AudioClip clip)
    {
        switch (channel) {
            case 1:
                efxSource1.clip = clip;
                efxSource1.Play();
                break;
            case 2:
                efxSource2.clip = clip;
                efxSource2.Play();
                break;
            case 3:
                efxSource3.clip = clip;
                efxSource3.Play();
                break;
            case 4:
                efxSource4.clip = clip;
                efxSource4.Play();
                break;
        }
        
    }

    //This function will take audio clips and randomly play one of them in a random pitch
    //using a narrow window of possibly pitches
    //params parameter lets us pass in a comma seperated list of parameters of the type as specified
    //by the parameter that comes after params keyword
      
    public void RandomizeSfx(int channel, params AudioClip[] clips)
    {
        int randomIndex = Random.Range(0, clips.Length);
        float randomPitch = Random.Range(lowPitchRange, highPitchRange);


        switch (channel)
        {
            case 1:
                efxSource1.clip = clips[randomIndex];
                efxSource1.pitch = randomPitch;
                efxSource1.Play();
                break;
            case 2:
                efxSource2.clip = clips[randomIndex];
                efxSource2.pitch = randomPitch;
                efxSource2.Play();
                break;
            case 3:
                efxSource3.clip = clips[randomIndex];
                efxSource3.pitch = randomPitch;
                efxSource3.Play();
                break;
            case 4:
                efxSource4.clip = clips[randomIndex];
                efxSource4.pitch = randomPitch;
                efxSource4.Play();
                break;
        }
       
    }
}
