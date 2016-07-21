using UnityEngine;
using System.Collections;
using System;
public class TutorialLevel1 : Level {
    int brokenPotsCounter = 0;

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
}
