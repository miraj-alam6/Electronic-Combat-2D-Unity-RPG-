using UnityEngine;
using System.Collections;

public class TutorialLevel6 : Level {

    int deadEnemies = 0;
    [HideInInspector]public bool winoaHasEnough = false;
    [HideInInspector]public bool kaliHasEnough = false;
    bool winoaNotTaught = true;
    bool kaliNotTaught = true;
    public TutorialLevel6(int deaths) : base((deaths))
    {

    }
    public override void updateLevel(string message)
    {
        base.updateLevel(message);

        if (message.Equals("killed_enemy"))
        {
            deadEnemies++;
        }
        if (deadEnemies == 7)
        {
            GameManager.instance.stopAll = true;
            levelDone = true;
            Debug.Log("Done with level 6");
            // Can't do next line here, need monobehavior
            //Invoke("Restart", 1); //this will call the function 1 second after colliding
            GameManager.instance.DoneWithLevel();
        }
    }

    public override void turnBehavior()
    {
        if (hintsOn)
        {

            if (turnCount == 2)
            {
                GameManager.instance.showMessage("Doing, specials is an intricate process. First try to get Winoa's special upto 75, or Kali's special upto 80. Winoa's special recharges much faster. Killing enemies are always the best way to charge your special really quickly.");
            }
            if (turnCount == 3)
            {
                GameManager.instance.showMessage("Winoa is weaker than Kali and her ATG depletes faster, but her ATG also fills up very quickly. Kali's special, called Halt, can be used to destroy all accumulated ATG of a unit.");
            }
            else if (turnCount > 2 && winoaHasEnough && winoaNotTaught)
            {
                GameManager.instance.showMessage("Winoa now has enough special to do her Special Move: Repair. Press z to start it, and then move into a unit that you want to heal to heal him/her/it/unicorn.");
                winoaNotTaught = false;
            }
            else if (kaliHasEnough && kaliNotTaught)
            {
                GameManager.instance.showMessage("Kali now has enough special to do his Special Move: Halt. Press z to start it, and then move into a unit whose ATG you want to obliterate to 0.");
                kaliNotTaught = false;
            }
            
            else if (deathCount > 6 && turnCount == 1)
            {
                GameManager.instance.showMessage("HINT: Even though Winoa is weak, she is very fast, and if she gets the last hit to kill an enemy, she will get a lot of special gauge for it");
            }
        }
    }
}
