using UnityEngine;
using System.Collections;

public class TutorialLevel4 : Level {
    int deadEnemies = 0;

    public TutorialLevel4(int deaths) : base((deaths))
    {

    }
    public override void updateLevel(string message)
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
                GameManager.instance.showMessage("NOTE ABOUT HINTS: If you want to turn off hints and tutorial messages, press backspace when it is your turn and you are not reading a message. If you reach a new level and need to see hints again, press backspace on your first turn. ");
            }
            if (turnCount == 3) {
                GameManager.instance.showMessage("If this is your first time playing, tutorial messages will teach you how to play, but if you get tired of reading the same messages because you keep dying and restarting the same stage, you can turn off hints, and then turn it back on, on your first turn in a new stage.");
            }
            if (turnCount == 4)
            {
                GameManager.instance.showMessage("However, if you die MANY times in the same stage. Turn hints back on, and you will probably get actual hints to beating the level.");
            }
            if (deathCount > 5 && turnCount == 2)
            {
                GameManager.instance.showMessage("HINT: Sometimes it is better to wait.");
            }
        }
    }
}
