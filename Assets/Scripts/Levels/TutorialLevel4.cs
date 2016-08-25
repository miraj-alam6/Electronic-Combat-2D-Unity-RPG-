using UnityEngine;
using System.Collections;

public class TutorialLevel4 : Level {
    int deadEnemies = 0;

    public TutorialLevel4(int deaths) : base((deaths))
    {

    }
    public override bool updateLevel(string message)
    {
        base.updateLevel(message);

        if (message.Equals("killed_enemy"))
        {
            deadEnemies++;
        }
        if (deadEnemies == 3)
        {
            GameManager.instance.stopAll = true;
            levelDone = true;
            Debug.Log("Done with level 4");
            // Can't do next line here, need monobehavior
            //Invoke("Restart", 1); //this will call the function 1 second after colliding
            GameManager.instance.DoneWithLevel();
        }
        return false;
    }
    public override void turnBehavior() { 
       if (hintsOn)
        {

            if (turnCount == 1)
            {
                GameManager.instance.showMessage("When it is your turn, you can observe how much ATB has filled up for your enemies to get a feel of how quick they are.");
            }
            if (turnCount == 2)
            {
                GameManager.instance.showMessage("NOTE ABOUT HINTS: If you want to turn off hints and tutorial messages, press ESCAPE to open the menu. However, keep messages on if you don't know controls");
            }
            if (turnCount == 3) {
                GameManager.instance.showMessage("If  you get tired of reading the same messages because you keep dying and restarting the same stage, you can turn off hints, and then turn it back on, in the end level screen before the next level begins.");
            }
            if (turnCount == 4)
            {
                GameManager.instance.showMessage("However, if you die MANY times in the same stage. Turn hints back on, and you may get a clue on how to beat the level.");
            }
            if (turnCount == 5)
            {
                GameManager.instance.showMessage("You can use the menu to quit the level and change the difficulty, and then you can retry the level.");
            }
            if (deathCount > 5 && turnCount == 2)
            {
                GameManager.instance.showMessage("HINT: Sometimes it is better to wait.");
            }
        }
    }
}
