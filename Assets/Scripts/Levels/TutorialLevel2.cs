using UnityEngine;
using System.Collections;

public class TutorialLevel2 : Level {
    int deadEnemies = 0;

    public TutorialLevel2(int deaths) : base((deaths))
    {

    }

    public override bool updateLevel(string message)
    {
        base.updateLevel(message);

        if (message.Equals("killed_enemy"))
        {
            deadEnemies++;
        }
        if (deadEnemies == 1)
        {
            GameManager.instance.stopAll = true;
            levelDone = true;
            Debug.Log("Done with level 2");
            // Can't do next line here, need monobehavior
            //Invoke("Restart", 1); //this will call the function 1 second after colliding
            GameManager.instance.DoneWithLevel();
        }
        return true;
    }

    public override void turnBehavior()
    {
        if (hintsOn) {
            if (turnCount == 1)
            {
                GameManager.instance.showMessage("Move into an enemy to attack him. Attacking consumes a bit more ATG than moving.");
            }

            if (turnCount == 2)
            {
                GameManager.instance.showMessage("The rate at which an enemy's ATG fills up is different from yours. Study enemy behavior to help bolster your strategy");
            }
            if (turnCount == 3)
            {
                GameManager.instance.showMessage("You can choose what direction you are facing using WSDA, even if your ATG has run out. This will help you defend against back attacks which give you extra damage, but you can do the same to enemies and give them extra damage from the back.");
            }
            if (turnCount == 4)
            {
                GameManager.instance.showMessage("Press Enter to end your turn early while you still have ATG remaining.");
            }
        }
    }
}
