using UnityEngine;
using System.Collections;

public class TutorialLevel5 : Level
{
    int deadEnemies = 0;
    public TutorialLevel5(int deaths) : base((deaths))
    {

    }

    public override bool updateLevel(string message)
    {
        base.updateLevel(message);

        if (message.Equals("killed_enemy"))
        {
            deadEnemies++;
        }
        if (deadEnemies == 6)
        {
            GameManager.instance.stopAll = true;
            levelDone = true;
            Debug.Log("Done with level 5");
            // Can't do next line here, need monobehavior
            //Invoke("Restart", 1); //this will call the function 1 second after colliding
            GameManager.instance.DoneWithLevel();
        }
        return true;
    }

    public override void turnBehavior()
    {
        if (hintsOn)
        {

            if (turnCount == 1)
            {
                GameManager.instance.showMessage("Different types of enemies have different stats. Even different units of the same enemy type can have slightly different stats.");
            }

            if (deathCount > 5 && turnCount == 2)
            {
                GameManager.instance.showMessage("HINT: Range attacks are important af sometimes.");
            }
        }
    }
}
