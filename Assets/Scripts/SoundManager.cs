using UnityEngine;
using System.Collections;

//Gonna use the same singleton pattern that we used in GameManager here.
public class SoundManager : MonoBehaviour {
    public AudioSource efxSource;
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
    public void PlaySingle(AudioClip clip)
    {
        efxSource.clip = clip;
        efxSource.Play();
    }

    //This function will take audio clips and randomly play one of them in a random pitch
    //using a narrow window of possibly pitches
    //params parameter lets us pass in a comma seperated list of parameters of the type as specified
    //by the parameter that comes after params keyword
    public void RandomizeSfx(params AudioClip [] clips) {
        int randomIndex = Random.Range(0, clips.Length);
        float randomPitch = Random.Range(lowPitchRange,highPitchRange);
        efxSource.pitch = randomPitch;
        efxSource.clip = clips[randomIndex];
        efxSource.Play();
    }
}
