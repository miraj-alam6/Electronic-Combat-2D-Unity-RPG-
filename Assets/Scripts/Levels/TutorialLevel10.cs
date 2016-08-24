using UnityEngine;
using System.Collections;

public class TutorialLevel10 : Level
{

    public int currentElectric = 0;
    public int goalElectric = 10;
    public int players = 4;
    public TutorialLevel10(int deaths) : base((deaths))
    {
        switch (
        GameManager.instance.difficultyLevel)
        {
            case 0:
                goalElectric = 7;
                break;
            case 1:
                goalElectric = 10;
                break;
            case 2:
                goalElectric = 12;
                break;
        }
    }
    public override bool updateLevel(string message)
    {
        base.updateLevel(message);
        bool returnVal = false;
        if (message.Equals("got_battery"))
        {
            currentElectric++;
            GameManager.instance.RefreshMessage();
            returnVal = true;
        }
        if (message.Equals("remove_player"))
        {
            if (currentElectric >= goalElectric)
            {
                players--;
                returnVal = true;
            }
            else {
                returnVal = false;
            }
        }
        if (currentElectric >= goalElectric && players <= 0)
        {
            GameManager.instance.stopAll = true;
            levelDone = true;
            Debug.Log("Done with level 10");
            // Can't do next line here, need monobehavior
            //Invoke("Restart", 1); //this will call the function 1 second after colliding
            GameManager.instance.DoneWithLevel();
        }
        return returnVal;
    }

    public override void turnBehavior()
    {
        if (hintsOn)
        {
            
            if (turnCount == 2)
            {
                GameManager.instance.showMessage("You can analyze pots to see their weight. Based on their weight, you can try to guess what is inside before breaking it by remembering what you find in which types of pots. But keep in mind, two different types of things might weigh the same.");
            }
            
            else if (turnCount == 3)
            {
                GameManager.instance.showMessage("Baby robos have a high chance of breaking pots on the way towards hitting you");
            }

            if (deathCount > 4 && turnCount == 5)
            {
                GameManager.instance.showMessage("HINT: Kill all the baby robos first so that they can't break more pots and bring more enemies into the battle.");
            }

        }
    }
}