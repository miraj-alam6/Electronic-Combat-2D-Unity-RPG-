using UnityEngine;
using System.Collections;

public class TutorialLevel9 : Level
{

    public int chickens = 0;
    public int goalChickens = 5;
    public int players = 4;
    public TutorialLevel9(int deaths) : base((deaths))
    {
        switch (
        GameManager.instance.difficultyLevel)
        {
            case 0:
                goalChickens = 5;
                break;
            case 1:
                goalChickens = 7;
                break;
            case 2:
                goalChickens = 8;
                break;
        }
    }
    public override bool updateLevel(string message)
    {
        base.updateLevel(message);
        bool returnVal = false;
        if (message.Equals("got_chicken"))
        {
            chickens++;
            GameManager.instance.RefreshMessage();
            returnVal = true;
        }
        if (message.Equals("remove_player"))
        {
            if (chickens >= goalChickens)
            {
                players--;
                returnVal = true;
            }
            else {
                returnVal = false;
            }
        }
        if (chickens >= goalChickens && players <= 0)
        {
            GameManager.instance.stopAll = true;
            levelDone = true;
            Debug.Log("Done with level 9");
            // Can't do next line here, need monobehavior
            //Invoke("Restart", 1); //this will call the function 1 second after colliding
            GameManager.instance.DoneWithLevel();
        }
        return returnVal;
    }

    public override void turnBehavior()
    {
        if (turnCount == 1)
        {
            GameManager.instance.showMessage("SUPER IMPORTANT: The creator of this game was too lazy to fix a bug in the game, but it's easy to deal with. Sometimes when you leave the level with one character the camera will not automatically reset. You can fix camera by pressing tab and moving the grid selector around a bit.");
        }
        if (hintsOn)
        {
            if (turnCount == 2) {
                GameManager.instance.showMessage("There are two types of enemies: Ravagers and Guards. Ravagers will pursue you and hunt you, while Guards will only attack if you are near them. Analyze the names of enemies to understand which type they are.");
            }
            else if (turnCount == 3)
            {
                GameManager.instance.showMessage("Enemies will now attack whatever unit is closest, you can sometimes take advantage of that by luring an enemy out to a specific player unit.");
            }

            else if (turnCount == 4)
            {
                GameManager.instance.showMessage("Annihilators are ravagers.");
            }
            
            if (deathCount > 4 && turnCount == 5)
            {
                GameManager.instance.showMessage("HINT: You do not need to defeat every enemy, you just need to collect as many flowers as said by the objective.");
            }
        }

    }
}
