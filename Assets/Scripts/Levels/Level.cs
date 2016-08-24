using UnityEngine;
using System.Collections;

public abstract class Level{

    public bool levelDone = false;
    public bool prepareToRestart = false;
    public int turnCount = 0;
    public int deathCount = 0;
    public bool hintsOn = true;
    public bool kaliHere = true;
    public bool winoaHere = false;
    public bool hugoHere = false;
    public bool alejandraHere = false;
    public Level(int deaths) {
        deathCount = deaths;
        
        if (GameManager.instance.levelNumber >=8) {
            alejandraHere = true;
        }
        if (GameManager.instance.levelNumber >= 7)
        {
            hugoHere = true;
        }
        if (GameManager.instance.levelNumber >= 6)
        {
            winoaHere = true;
        }
    }
    public virtual bool updateLevel(string message) {
        if (message.Equals("restart"))
        {
            deathCount++;
            GameManager.instance.GonnaRestartLevel();
            prepareToRestart = true;
        }
        return true;
    }
    public void retainHintsSetting(bool hints) {
        hintsOn = hints;
    }
    public void incrementTurnCount() {
        turnCount++;
        turnBehavior();
    }
    //Behavior of level will be different based on what turn it is, and which level it is
    public abstract void turnBehavior();
}
