using UnityEngine;
using System.Collections;

public class StoryTeller : MonoBehaviour {

    public StoryChapter currentChapter;
    public StoryChapter chapter1;
    public StoryChapter chapter2;
    public StoryChapter chapter3;
    public StoryChapter chapter6;
    public StoryChapter chapter7;
    public StoryChapter chapter8;
    public StoryChapter chapter9;
    public StoryChapter chapter12;
    public StoryChapter ending;
    public StoryChapter postCredit;
    public GameObject credits;
    // Use this for initialization
    void Start () {
        setChapter();
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetButtonDown("Submit")) {
            continueStory();
            //SoundManager.instance.storyMusicSource.UnPause();
        }
        if (Input.GetButtonDown("Start"))
        {
            skipAll();
            //SoundManager.instance.storyMusicSource.Pause();
        }
    }
    public void setChapter() {

        switch (GameManager.instance.levelNumber) {
            case 1:
                currentChapter = chapter1;
                break;
            case 2:
                break;
            case 3:
                currentChapter = chapter3;
                break;
            case 4:
                break;
            case 5:
                break;
            case 6:
                currentChapter = chapter6;
                break;
            case 7:
                currentChapter = chapter7;
                break;
            case 8:
                currentChapter = chapter8;
                break;
            case 9:
                currentChapter = chapter9;
                break;
            case 12:
                currentChapter = chapter12;
                break;
            case 13:
                currentChapter = ending;
                break;
            case 15:
                currentChapter = postCredit;
                break;


        }
        chapter1.gameObject.SetActive(false);
        chapter3.gameObject.SetActive(false);
        chapter6.gameObject.SetActive(false);
        currentChapter.gameObject.SetActive(true);
    }


    public void continueStory() {
        if (currentChapter) { 
            if (!currentChapter.nextLine()) {
                StartLevel();
            }
        }
    }
    public void skipAll() {
        if (currentChapter) {
            if (!currentChapter.skipAll()) {
                StartLevel();
            }
        }
    }

    public void StartLevel() {
        if (GameManager.instance.levelNumber == 13) {
            GameManager.instance.LoadLevelByNumber(14);
            //ending.gameObject.SetActive(false);
            //credits.SetActive(true);
            return;
        }
        if (GameManager.instance.levelNumber == 15)
        {
            GameManager.instance.LoadLevelByNumber(16);
            return;
        }
        if (GameManager.instance.levelNumber >= 1 && GameManager.instance.levelNumber <= 3) { 
            GameManager.instance.noMoreMenuOnNextLoad = true;
            GameManager.instance.storyMusicPaused = true;
            SoundManager.instance.storyMusicSource.Pause();
            
            if (GameManager.instance.levelMusicPaused)
            {
                
                SoundManager.instance.musicSource.UnPause();
                GameManager.instance.levelMusicPaused = false;
            }

            else {
                SoundManager.instance.musicSource.clip = GameManager.instance.levels1to3Music;
                SoundManager.instance.musicSource.Play();
            }
        }
        else if (GameManager.instance.levelNumber >= 4 && GameManager.instance.levelNumber <= 8)
        {
            GameManager.instance.noMoreMenuOnNextLoad = true;
            GameManager.instance.storyMusicPaused = true;
            SoundManager.instance.storyMusicSource.Pause();

        }

        GameManager.instance.LoadLevelByNumber(GameManager.instance.levelNumber);
    }
}
