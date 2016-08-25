using UnityEngine;
using System.Collections;

public class TutorialLevel12 : Level
{


    public int players = 4;
    public bool defeatedBoss = false;
    public int turns = 0;
    public TutorialLevel12(int deaths) : base((deaths))
    {
        switch (
        GameManager.instance.difficultyLevel)
        {
            case 0:

                break;
            case 1:

                break;
            case 2:

                break;
        }
    }
    public override bool updateLevel(string message)
    {
        base.updateLevel(message);
        bool returnVal = false;
        if (message.Equals("one_turn"))
        {
            Debug.Log("so bad");
            turns++;
            GameManager.instance.finalBattleStuff.CheckIfSpawn(turns);
        }

        if (message.Equals("defeat_boss"))
        {
            defeatedBoss = true;
            returnVal = true;
        }
        if (message.Equals("remove_player"))
        {
            if (defeatedBoss)
            {
                players--;
                returnVal = true;
            }
            else {
                returnVal = false;
            }
        }
        if (defeatedBoss && players <= 0)
        {
            GameManager.instance.stopAll = true;
            levelDone = true;
            Debug.Log("Done with level 12");
            // Can't do next line here, need monobehavior
            //Invoke("Restart", 1); //this will call the function 1 second after colliding
            GameManager.instance.DoneWithLevel();
        }
        return returnVal;
    }

    public override void turnBehavior()
    {
        if (hintsOn)
        {
            if (turnCount == 2)
            {
                GameManager.instance.showMessage("Final battle, holy shit. Beware: Winoa and Hugo are too weak to damage the final boss in most modes.");
            }
            if (turnCount == 3)
            {
                GameManager.instance.showMessage("Roxanne is very powerful, and furthermore she has a special attack. It is the same Alejandra's. She will get another turn. The special costs 75 so keep an eye on how much special she has.");
            }
            if (turnCount == 4)
            {
                GameManager.instance.showMessage("If Roxanne breaks a wall on the path to get to you, her special will increase a tremendous amount.");
            }
            if (turnCount == 5)
            {
                GameManager.instance.showMessage("Another thing to keep in mind is that Roxanne tries to kill any unit that gets too close(within 5 spaces) to her that isn't Alejandra, but if no other unit except Alejandra is near her, then she will attack Alejandra.");
            }
            if (turnCount == 6)
            {
                GameManager.instance.showMessage("Last advice: late into the battle enemies will start spawning from the left and right, but you can stop them from spawning by blocking off the locations where they spawn. Furthermore, remember to always analyze their stats.");
            }
        }
    }
}