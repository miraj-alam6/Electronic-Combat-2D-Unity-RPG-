using UnityEngine;
using System.Collections;
using System;
public class TutorialLevel1 : Level {
    int brokenPotsCounter = 0;

    public TutorialLevel1(int deaths) : base((deaths))
    {
       
    }
    public override void updateLevel(string message) {
        base.updateLevel(message);

        if (message.Equals("broke_pot")) {
            brokenPotsCounter++;
        }

        if (brokenPotsCounter == 3) {
            GameManager.instance.stopAll = true;
            levelDone = true;
            Debug.Log("Done with level 1");
            // Can't do next line here, need monobehavior
            //Invoke("Restart", 1); //this will call the function 1 second after colliding
            GameManager.instance.DoneWithLevel();
        }
    }

    public override void turnBehavior()
    {
        if (hintsOn)
        {

            if (turnCount == 1)
            {
                GameManager.instance.showMessage("Use arrow keys to move around. Once you cannot move" +
                    " anymore, press space to end your turn. Also press space to finish reading a message" +
                    " and resume gameplay.");
            }

            if (turnCount == 2)
            {
                GameManager.instance.showMessage("As you have just seen, the ATG fills up, and once it is full" +
                    " the unit whose bar just filled up will get a turn. Every move will lower the ATG" +
                    " and once the ATG becomes 0, the turn will end, and ATG will start filling up again.");
            }

            if (turnCount == 20)
            {
                GameManager.instance.showMessage("What is wrong with you? It has been 20 turns and you still" +
                    " haven't beat the first level?");
            }
        }
    }
}
