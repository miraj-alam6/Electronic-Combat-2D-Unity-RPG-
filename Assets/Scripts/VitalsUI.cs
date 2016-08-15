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
    public SpecialGauge kaliSpecial;
    public VitalBar winoaHealthBar;
    public VitalBar winoaATBBar;
    public SpecialGauge winoaSpecial;
    public VitalBar hugoHealthBar;
    public VitalBar hugoATBBar;
    public SpecialGauge hugoSpecial;
    public VitalBar alejandraHealthBar;
    public VitalBar alejandraATBBar;
    public SpecialGauge alejandraSpecial;
    public GameObject kaliVitals;
    public GameObject winoaVitals;
    public GameObject hugoVitals;
    public GameObject alejandraVitals;
    public int levelNumber = 1;

    // Use this for initialization
    void Start () {
        refreshUI();
	}

    public void refreshUI() {
        levelNumber = GameManager.instance.levelNumber;
        if (kaliVitals) {
            kaliVitals.SetActive(true);
        }
        if (winoaVitals)
        {
            if (levelNumber >= 6)
            {
                winoaVitals.SetActive(true);
            }
            else {
                winoaVitals.SetActive(false);
            }
        }
        if (hugoVitals)
        {
            if (levelNumber >= 7)
            {
                hugoVitals.SetActive(true);
            }
            else {
                hugoVitals.SetActive(false);
            }
        }
        if (alejandraVitals)
        {
            if (levelNumber >= 8)
            {
                alejandraVitals.SetActive(true);
            }
            else {
                alejandraVitals.SetActive(false);
            }

        }

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
    public void KaliReduceSpecialValue (int value)
    {
        kaliSpecial.ReduceSpecialValue(value);
    }
    public void KaliAddSpecialValue(int value)
    {
        kaliSpecial.AddSpecialValue(value);
    }
    public void KaliSetSpecial(int value)
    {
        kaliSpecial.SetSpecialValue(value);
    }

    public void UpdateWinoaHP(int maxHP, int currHP)
    {
        if (levelNumber < 6) {
            return;
        }
        winoaHealthBar.UpdateVitalBar(maxHP, currHP);
    }
    public void UpdateWinoaATB(int maxATB, int currATB)
    {
        if (levelNumber < 6)
        {
            return;
        }
        winoaATBBar.UpdateVitalBar(maxATB, currATB);
        if (currATB >= 100)
        {
            winoaATBBar.BecomeGreen();
        }
        else if (currATB <= 0)
        {
            winoaATBBar.BecomeBlue();
        }
    }
    public void winoaReduceSpecialValue(int value)
    {
        if (levelNumber < 6)
        {
            return;
        }
        winoaSpecial.ReduceSpecialValue(value);
    }
    public void winoaAddSpecialValue(int value)
    {
        if (levelNumber < 6)
        {
            return;
        }
        winoaSpecial.AddSpecialValue(value);
    }
    public void winoaSetSpecial(int value)
    {
        if (levelNumber < 6)
        {
            return;
        }
        winoaSpecial.SetSpecialValue(value);
    }

    public void UpdateHugoHP(int maxHP, int currHP)
    {
        if (levelNumber < 7)
        {
            return;
        }
        hugoHealthBar.UpdateVitalBar(maxHP, currHP);
    }
    public void UpdateHugoATB(int maxATB, int currATB)
    {
        if (levelNumber < 7)
        {
            return;
        }
        hugoATBBar.UpdateVitalBar(maxATB, currATB);
        if (currATB >= 100)
        {
            hugoATBBar.BecomeGreen();
        }
        else if (currATB <= 0)
        {
            hugoATBBar.BecomeBlue();
        }
    }
    public void hugoReduceSpecialValue(int value)
    {
        if (levelNumber < 7)
        {
            return;
        }
        hugoSpecial.ReduceSpecialValue(value);
    }
    public void hugoAddSpecialValue(int value)
    {
        if (levelNumber < 7)
        {
            return;
        }
        hugoSpecial.AddSpecialValue(value);
    }
    public void hugoSetSpecial(int value)
    {
        if (levelNumber < 7)
        {
            return;
        }
        hugoSpecial.SetSpecialValue(value);
    }
    public void UpdateAlejandraHP(int maxHP, int currHP)
    {
        if (levelNumber < 8)
        {
            return;
        }
        alejandraHealthBar.UpdateVitalBar(maxHP, currHP);
    }
    public void UpdateAlejandraATB(int maxATB, int currATB)
    {
        if (levelNumber < 8)
        {
            return;
        }
        alejandraATBBar.UpdateVitalBar(maxATB, currATB);
        if (currATB >= 100)
        {
            alejandraATBBar.BecomeGreen();
        }
        else if (currATB <= 0)
        {
            alejandraATBBar.BecomeBlue();
        }
    }
    public void alejandraReduceSpecialValue(int value)
    {
        if (levelNumber < 8)
        {
            return;
        }
        alejandraSpecial.ReduceSpecialValue(value);
    }
    public void alejandraAddSpecialValue(int value)
    {
        if (levelNumber < 8)
        {
            return;
        }
        alejandraSpecial.AddSpecialValue(value);
    }
    public void alejandraSetSpecial(int value)
    {
        if (levelNumber < 8)
        {
            return;
        }
        alejandraSpecial.SetSpecialValue(value);
    }

}
