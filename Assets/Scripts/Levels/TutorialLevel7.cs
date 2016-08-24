using UnityEngine;
using System.Collections;

public class TutorialLevel7 : Level
{

    int deadEnemies = 0;
    public TutorialLevel7(int deaths) : base((deaths))
    {

    }
    public override bool updateLevel(string message)
    {
        base.updateLevel(message);

        if (message.Equals("killed_enemy"))
        {
            deadEnemies++;
        }
        if (deadEnemies == 4)
        {
            GameManager.instance.stopAll = true;
            levelDone = true;
            Debug.Log("Done with level 7");
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

            if (turnCount == 2) {
                GameManager.instance.showMessage("You now have three characters: Kali, Winoa, and Hugo. Make sure you are careful about where you end your turn so that you don't block the path of the character that will go next.");
            }
            if (turnCount == 4)
            {
                GameManager.instance.showMessage("Hugo does not have guns but he can analyze units, walls, etc(same controls as guns). You need both special and ATG to analyze. There is no cost to analyzing players and stuff you already analyzed before. Analyzing an enemy for first time costs more than analyzing objects.");
            }
            if (turnCount == 7)
            {
                GameManager.instance.showMessage("Stat Guide: ATG Rate is how fast ATG bar fills(speed). ATG bar fills at a speed rate that is between the given range. ATG Cost is how much a normal move costs, the lower this is, the more moves the unit can do on her turn. Use a search engine if you don't know HP, ATK, DEF");
            }
            if (turnCount == 10)
            {
                GameManager.instance.showMessage("Hugo's special attack is called Turn Around. It will make a unit turn around, thus it can be helpful for doing back attacks. Use it just like you would Kali's special attack.");
            }

            if (turnCount == 13)
            {
                GameManager.instance.showMessage("CAUTION: Friendly fire exists for bullets and special attacks.");
            }
            if (deathCount > 8 && turnCount == 3)
            {
                GameManager.instance.showMessage("HINT: Some enemies are so ruthless, that it is better to kill them before they even get one turn.");
            }
            if (deathCount > 4 && turnCount == 1)
            {
                GameManager.instance.showMessage("HINT: If you analyze everything you are bound to understand new strategies.");
            }

        }
    }

}
