using UnityEngine;
using System.Collections;

//This is the class for the UI on the left side of the screen which will show all the health bars
//of all the characters
public class VitalsUI : MonoBehaviour {
    //These are all set in the inspector. To do so just drag the health bar image/or ATB bar game object
    //into the VitalBar slot in the inspector, and unity is smart enough to realize you want the VitalBar
    //script to go in.   
    public VitalBar kaliHealthBar;
    public VitalBar kaliATBBar;

    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Display")) {
            kaliATBBar.UpdateVitalBar(100,0);
        }
	}

    public void UpdateKaliHP(int maxHP, int currHP) {
        kaliHealthBar.UpdateVitalBar(maxHP,currHP);
    }
    public void UpdateKaliATB(int maxATB, int currATB)
    {
        kaliATBBar.UpdateVitalBar(maxATB,currATB);
        if (currATB >= 100)
        {
            kaliATBBar.BecomeGreen();
        }
        else if(currATB <= 0) {
            kaliATBBar.BecomeBlue();
        }
    }
}
