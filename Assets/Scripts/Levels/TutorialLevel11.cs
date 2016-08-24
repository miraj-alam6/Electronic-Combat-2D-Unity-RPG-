using UnityEngine;
using System.Collections;

public class TutorialLevel11 : Level
{

    public int currentElectric = 0;
    public int goalElectric = 10;
    public int players = 4;
    public TutorialLevel11(int deaths) : base((deaths))
    {
        
    }
    public override bool updateLevel(string message)
    {
        base.updateLevel(message);
        bool returnVal = false;
        
        if (message.Equals("remove_player"))
        {
            
            players--;
            returnVal = true;
            
        }
        if (players <= 0)
        {
            GameManager.instance.stopAll = true;
            levelDone = true;
            Debug.Log("Done with level 11");
            GameManager.instance.DoneWithLevel();
        }
        return returnVal;
    }

    public override void turnBehavior()
    {
        if (hintsOn)
        {

            if (turnCount == 2) {
                GameManager.instance.showMessage("Many enemies will not attack you, if you keep a safe distance.");
            }
            if (turnCount == 3)
            {
                GameManager.instance.showMessage("Be aware of true annihilators.");
            }

            if (deathCount > 4 && turnCount == 5)
            {
                GameManager.instance.showMessage("HINT: If a true annihilator gets a turn, you most likely will die, so make sure you kill them or end the level before they get a single turn.");
            }
        }
    }
}