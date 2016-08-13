using UnityEngine;
using System.Collections;
using UnityEngine.UI;
//This is a general ChoiceScreen thing
//DRAWBACK of current code: with the current way that toggling works, there can only be 1 
//choice at most that is toggled in a
public class ChoiceScreen : MonoBehaviour {

    public Text finishedLevel;
    public string typeMenu;
    public Image[] allButtons;
    public int currentIndex = -1;
    public int toggledChoiceIndex = -1; //often a choice will be toggled a different color to show it is
    //selected
    public int totalChoices = 0;


	// Use this for initialization
	void Start () {
        totalChoices = allButtons.Length;
        if (GameManager.instance.hintsOn && GameManager.instance.whichMenu.Equals("level_done") && GameManager.instance.inMenuScreen) {
            setToggledChoice(3,false);
        }
        if (GameManager.instance.hintsOn && !GameManager.instance.inMenuScreen && totalChoices >2)
        {
            setToggledChoice(1, false);
        }
        if (finishedLevel) { 
        finishedLevel.text = "Finished Level " + (GameManager.instance.levelNumber - 1);
        }
        updateChoiceScreen();

	}

    //The return value represent if the thing is toggled as in if it is highlighted, as in
    //if it got turned on or off
  
    public bool setToggledChoice(int index, bool actuallyAToggleSwitch) {
        if (actuallyAToggleSwitch)
        {
            if (toggledChoiceIndex == -1)
            {
                toggledChoiceIndex = index;
                return true;
            }
            else {
                toggledChoiceIndex = -1;
                return false;
            }
        }
        else {
            toggledChoiceIndex = index;
            return true;
        }
    }
    public void moveUp() {
        if (currentIndex == -1)
        {
            startChoosing();
            return;
        }
        if (currentIndex == 0)
        {
            currentIndex = totalChoices - 1;
        }
        else {
            currentIndex--;
        }
        updateChoiceScreen();
    }

    public void moveDown() {
        if (currentIndex == -1)
        {
            startChoosing();
            return;
        }
        if (currentIndex == totalChoices-1)
        {
            currentIndex = 0;
        }
        else {
            currentIndex++;
        }
        updateChoiceScreen();
    }

    public void updateChoiceScreen() {
       
        for (int i = 0; i < totalChoices; i++) {
            if (toggledChoiceIndex == i)
            {
                allButtons[i].color = Color.green;
            }
            else
            { 
                allButtons[i].color = new Color32(30, 20, 20, 120);
            }
        }
        if (currentIndex >= 0) { 
            allButtons[currentIndex].color = Color.red;
        }
    }

    public void startChoosing()
    {
        currentIndex = 0;
        updateChoiceScreen();
    }

    public int chooseChoice() {
        //Debug.Log("crush " + currentIndex);
        if (currentIndex == -1)
        {
            startChoosing();
            return -1;
        }
        return currentIndex;
    }

    public void resetChoiceMemory() {
        currentIndex = -1;
        updateChoiceScreen();
    }
    
}
