using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class StoryChapter : MonoBehaviour {

    public Text[] storyLines;
    public int currentLineNumber = 0;
    public int currentPage = 0;
    public int numberOfPages = 1;
    public int linesPerPage;
    public AudioClip musicForStory;
    
    void Start() {

        if (GameManager.instance.levelNumber == 1) {
            return;
        }
        if (GameManager.instance.levelNumber == 9 || GameManager.instance.levelNumber == 12
            || GameManager.instance.levelNumber == 13) {
            GameManager.instance.storyMusicPaused = false;
            GameManager.instance.levelMusicPaused = false;
            GameManager.instance.levelMusicContinue = false;
        }

    
        if (GameManager.instance.levelNumber >= 6 && GameManager.instance.levelNumber <= 9) {
            SoundManager.instance.storyMusicSource.clip = musicForStory;
            SoundManager.instance.storyMusicSource.Play();
            return;
        }

        if (GameManager.instance.storyMusicPaused)
        {
            SoundManager.instance.storyMusicSource.UnPause();
        }

        else {
            SoundManager.instance.storyMusicSource.clip = musicForStory;
            SoundManager.instance.storyMusicSource.Play();
        }
        
    }
	// Use this for initialization
	public bool nextLine () {
        if (numberOfPages > 1) {
            return multiPageNextLine();
        }
        currentLineNumber++;
        if (currentLineNumber < storyLines.Length)
        {
            storyLines[currentLineNumber].gameObject.SetActive(true);
            return true;
        }
        else {
            return false;
        }
    }

    public bool multiPageNextLine() {
        currentLineNumber++;
        int actualLineIndex = (currentPage * linesPerPage) + currentLineNumber;
        Debug.Log(currentLineNumber + ":::" + actualLineIndex);
        if (actualLineIndex < storyLines.Length)
        {
            if (currentLineNumber < linesPerPage)
            {
                storyLines[actualLineIndex].gameObject.SetActive(true);
                return true;
            }
            else {
                Debug.Log("new page");
                for (int i = 0; i < actualLineIndex; i++) {
                    storyLines[i].gameObject.SetActive(false);
                }
                currentLineNumber = 0;
                currentPage++;
                storyLines[actualLineIndex].gameObject.SetActive(true);
                return true;
            }
        }
        else {
            Debug.Log("reach here");
            return false;
        }

    }
    public bool skipAll() {
        if (numberOfPages > 1) {
            return skipPage();
        }
        if(currentLineNumber >= storyLines.Length-1){
            return false;
        }
        for(currentLineNumber = 0; currentLineNumber < storyLines.Length; currentLineNumber++)
        {
            storyLines[currentLineNumber].gameObject.SetActive(true);
        }
        return true;

    }
    public bool skipPage() {
        
        int actualLineIndex;
        actualLineIndex = (currentPage * linesPerPage) + currentLineNumber;
        if(actualLineIndex >= storyLines.Length-1)
        {
            return false;
        }
        if (currentLineNumber >= linesPerPage - 1) {
            for (int i = 0; i < actualLineIndex+1; i++) {
                storyLines[i].gameObject.SetActive(false);
            }
            currentLineNumber = -1;
            currentPage++;
        }
        currentLineNumber++;
        for (currentLineNumber = 0; currentLineNumber < linesPerPage; currentLineNumber++)
        {
            actualLineIndex = (currentPage * linesPerPage) + currentLineNumber;
            storyLines[actualLineIndex].gameObject.SetActive(true);
        }

        currentLineNumber--;
        return true;
    }
	// Update is called once per frame
	void Update () {
	
	}
}
