using UnityEngine;
using System.Collections;

public class TutorialLevel8 : Level {

    int deadEnemies = 0;
    public TutorialLevel8(int deaths) : base((deaths))
    {

    }
    public override bool updateLevel(string message)
    {
        base.updateLevel(message);

        if (message.Equals("killed_enemy"))
        {
            deadEnemies++;
        }
        if (deadEnemies == 10)
        {
            GameManager.instance.stopAll = true;
            levelDone = true;
            Debug.Log("Done with level 8");
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

            if (turnCount == 3) {
                GameManager.instance.showMessage("Alejandra has an incredibly slow ATG Gain Rate, but she has the highest ATK and the higest DEF. She cannot use any guns.");
            }
            if (turnCount == 5)
            {
                GameManager.instance.showMessage("Alejandra has a high(which is bad) ATG cost as well, but if you get 80 special AND once you run out of ATG for the turn, you can activate your special attack to get a second turn. Press z to activate it.");
            }
            if (deathCount > 8 && turnCount == 2)
            {
                GameManager.instance.showMessage("HINT: However, Alejandra is not invincible, make sure you heal her as needed.");
            }
            if (deathCount > 4 && turnCount == 1)
            {
                GameManager.instance.showMessage("HINT: Alejandra is the key to winning.");
            }

        }
    }
}
