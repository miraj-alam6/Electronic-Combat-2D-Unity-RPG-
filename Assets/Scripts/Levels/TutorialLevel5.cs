﻿using UnityEngine;
using System.Collections;

public class TutorialLevel5 : Level
{
    int deadEnemies = 0;

    public override void updateLevel(string message)
    {
        base.updateLevel(message);

        if (message.Equals("killed_enemy"))
        {
            deadEnemies++;
        }
        if (deadEnemies == 6)
        {
            GameManager.instance.stopAll = true;
            levelDone = true;
            Debug.Log("Done with level 5");
            // Can't do next line here, need monobehavior
            //Invoke("Restart", 1); //this will call the function 1 second after colliding
            GameManager.instance.DoneWithLevel();
        }
    }
}
