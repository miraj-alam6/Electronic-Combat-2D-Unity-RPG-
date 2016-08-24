using UnityEngine;
using System.Collections;

public class TutorialLevel3 : Level {
    int deadEnemies = 0;
    public TutorialLevel3(int deaths) : base((deaths))
    {

    }

    public override bool updateLevel(string message)
    {
        base.updateLevel(message);
        if (message.Equals("killed_enemy"))
        {
            deadEnemies++;
        }

        if (deadEnemies == 2)
        {
            GameManager.instance.stopAll = true;
            levelDone = true;
            Debug.Log("Done with level 3");
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
                GameManager.instance.showMessage("Press tab to aim. Aiming will let you see parts of the map that you haven't seen yet. Press shift to stop aiming. ");
            }

            if (turnCount == 4) {
                GameManager.instance.showMessage("When aiming, press tab again to shoot. Bullets are stronger than melee. You can only shoot if you have enough special, it is the number next to your vitals. ATB must also be greater than 0.");
            }
            if (turnCount == 5)
            {
                GameManager.instance.showMessage("You get a little bit of Special back each turn, you get more if you break pots and walls, and you get even more if you defeat an enemy.");
            }

            if (deathCount > 2 && turnCount == 2)
            {
                GameManager.instance.showMessage("HINT: Going headfirst into battle is not always the best strategy.");
            }
        }
    }
}
