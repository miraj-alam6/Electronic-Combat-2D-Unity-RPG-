using UnityEngine;
using System.Collections;

public class TutorialLevel6 : Level {

    int deadEnemies = 0;
    [HideInInspector]public bool winoaHasEnough = false;
    [HideInInspector]public bool kaliHasEnough = false;
    public bool showedFirst = false;
    public bool showedSecond = false;
    public bool showedThird = false;
    public bool showedFourth = false;
    bool winoaNotTaught = true;
    bool kaliNotTaught = true;
    public TutorialLevel6(int deaths) : base((deaths))
    {

    }
    public override bool updateLevel(string message)
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
        return true;
    }

    public override void turnBehavior()
    {
        if (hintsOn)
        {

            if (turnCount == 1)
            {
                if(!showedFirst)
                GameManager.instance.showMessage("Winoa is weaker than Kali in both melee and range, and her ATG depletes faster, but her ATG fills up very quickly, and she regenerates health every turn. Most units will try to hit Kali first, and Winoa only if they cannot hit Kali (Exception are the baby robos)");
                showedFirst = true;
            }
            if (turnCount == 2)
            {
                if (!showedSecond)
                    GameManager.instance.showMessage("Doing, specials is an intricate process. First try to get Winoa's special upto 75, or Kali's special upto 70. Winoa's special recharges much faster. Killing enemies are always the best way to charge your special really quickly.");
                showedSecond = true;
            }
            if (turnCount == 5)
            {
                if (!showedThird)
                    GameManager.instance.showMessage("Winoa's special called Repair, can heal a unit other than herself. Kali's special, called Halt, can be used to destroy all accumulated ATG of a unit. However, Kali's special points may be better spent on bullets. ");
                showedThird = true;
            }

            if (turnCount == 6)
            {
                if (!showedFourth)
                    GameManager.instance.showMessage("Winoa's bullet costs 13 to use, Kali's bullet cost 25 to use. Winoa's bullet, just like Kali's gives more damage than Melee, but Kali's melee and range are stronger than Winoa. ");
                showedFourth = true;
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
