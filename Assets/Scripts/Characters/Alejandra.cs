using UnityEngine;
using System.Collections;

public class Alejandra : Character
{

    

    void Start()
    {
        allMoves = new string[1] { "unstoppable" }; //modify this later to include more moves
        allWeapons = new string[1] { "none" };
        currentSpecial = allMoves[0];
        currentWeapon = allWeapons[0];
        if (allMoves.Length <= 1)
        {
            cantSwitchSpecial = true;
        }
    }

    public virtual void ChangeSpecialUp()
    {
        base.ChangeSpecialUp();
        //check if not have this explict return here works, as in is it possible to make
        //the return in the base function propogate and make this function prematurely end as well
        if (cantSwitchSpecial)
        {
            return;
        }
    }

    public virtual void ChangeSpecialDown()
    {
        base.ChangeSpecialDown();
        if (cantSwitchSpecial)
        {
            return;
        }
    }


    //pass in the unit script that did the special attack so you can make the unit change color
    public override bool StartSpecial(Unit self)
    {
        if (startedSpecial)
        {
            return true;
        }
        int specialValue = self.specialGauge.GetSpecialValue();
        int ATB = self.ATB;

        bool returnVal = false;
        
        switch (currentSpecial)
        {
            case "demonstrike":
                if (ATB <= 0)
                {
                    return returnVal;
                }
                GameManager.instance.LeftUI.GetComponent<VitalsUI>().alejandraReduceSpecialValue(90);
                if (self.specialGauge.ReduceSpecialValue(65))
                {
                    SoundManager.instance.PlaySingle(2, startSpecialSound);
                    Debug.Log("demonstrike worked");
                    self.turnBlue();
                    //startedSpecial = true;

                    returnVal = true;
                }
                else {
                    returnVal = false;
                }
                //the following invocation works for both the case where it actually works, and
                //when it doesn't

                break;

            case "unstoppable":
                if (self.ATB > 0) {
                    self.specialGauge.BlinkRed(0.1f);
                    GameManager.instance.LeftUI.GetComponent<VitalsUI>().alejandraSpecial.BlinkRed(0.1f);
                }

                
                if (self.ATB <= 0 && self.specialGauge.ReduceSpecialValue(80))
                {
                    GameManager.instance.LeftUI.GetComponent<VitalsUI>().alejandraReduceSpecialValue(80);
                    //
                    SoundManager.instance.PlaySingle(2, startSpecialSound);
                    self.turnBlue();
                    self.SetATB(100);
                    returnVal = true;
                }
                else {
                    SoundManager.instance.PlaySingle(2, ((Player)self).cancelSound);
                    returnVal = false;
                }
                self.Invoke("becomeNormal", 1f);
                break;
                
            default:
                returnVal = false;
                break;
        }
        return returnVal;
    }

    //Unstoppable doesn't use check if execute special
    //Pass in whoever the target of your special is AKA the unit you may attack or heal
    public override void CheckIfExecuteSpecial(Unit self, Unit target)
    {
        Debug.Log("Polymorphism bitch");
        if (startedSpecial)
        {

            switch (currentSpecial)
            {
                case "demonstrike":
                    if (target is Enemy)
                    {
                        ((Enemy)target).LoseHP(self.attack * 2, self.direction);
                    }
                    break;

                case "charge":
                    break;
                default:
                    break;
            }
            startedSpecial = false;
            self.becomeNormal();
        }
    }

    public override bool fireWeapon(Player shooter, GameObject gridSelector)
    {
        bool retVal = false;
        return retVal;
    }
}
