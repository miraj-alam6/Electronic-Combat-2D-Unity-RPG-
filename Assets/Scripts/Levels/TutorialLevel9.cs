using UnityEngine;
using System.Collections;

public class TutorialLevel9 : Level
{

    public int chickens = 0;
    public int goalChickens = 6;
    public TutorialLevel9(int deaths) : base((deaths))
    {

    }
    public override void updateLevel(string message)
    {
        base.updateLevel(message);

        if (message.Equals("got_chicken"))
        {
            chickens++;
        }
        if (chickens == 6)
        {
            GameManager.instance.stopAll = true;
            levelDone = true;
            Debug.Log("Done with level 9");
            // Can't do next line here, need monobehavior
            //Invoke("Restart", 1); //this will call the function 1 second after colliding
            GameManager.instance.DoneWithLevel();
        }
    }

    public override void turnBehavior()
    {

    }
}
