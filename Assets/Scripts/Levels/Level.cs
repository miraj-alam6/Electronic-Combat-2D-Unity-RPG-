using UnityEngine;
using System.Collections;

public abstract class Level{

    public bool levelDone = false;
    public bool prepareToRestart = false;
    public virtual void updateLevel(string message) {
        if (message.Equals("restart"))
        {
            GameManager.instance.GonnaRestartLevel();
            prepareToRestart = true;
        }
    }
}
