using UnityEngine;
using System.Collections;

public class Data  {
    //actual objects
    public int batteryBoxCount = 0;
    public int chickenCount = 0;
    public int burgerCount = 0;

    //currency and other things that are like currency
    public int credits = 0;
    public int electricity = 0;
    public int aggregrateFood = 0;

    public void addBatteryBoxCount(int amount) {
        batteryBoxCount += amount;
    }
    public void addChickenCount(int amount)
    {
        chickenCount += amount;
    }
    public void addBurgerCount(int amount)
    {
        burgerCount += amount;
    }
}
