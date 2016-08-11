using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class SpecialGauge : MonoBehaviour {
    public Text specialValueText;
    int specialValue; //Highest in can normally go up to is 100, possibly more late in game
    public int startingValue;
    // Use this for initialization
    void Start () {
        specialValue = startingValue;
        specialValueText.color = Color.yellow;
        UpdateGauge();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void AddSpecialValue(int value)
    {
        specialValue += value;
        if (specialValue > 100) {
            specialValue = 100;
        }
        UpdateGauge();
    }

    //This returns true if you can afford removing as much value as
    //is asked for by the function. This is because when you do an attack you must have enough
    //special value to be able to carry out the action. This is different from moves, whereas in
    //moves, if you only have 1 move point left you can do a move that takes 20 move points,
    //for special attacks you can only do a 20 special point attack if you have at least 20 special points.
    public bool ReduceSpecialValue(int value)
    {
        if (specialValue - value < 0) {
            BlinkRed(0.1f);
            return false;
        }
        specialValue -= value;
        UpdateGauge();
        return true;
    }
    public void UpdateGauge()
    {
        specialValueText.text = "" + specialValue;
    }
    public void BlinkRed(float time)
    {
        specialValueText.color = Color.red;
        Invoke("BackToNormal",time);
    }
    public void BackToNormal() {
        specialValueText.color = Color.yellow;
    }
    public int GetSpecialValue() {
        return specialValue;
    }

}
