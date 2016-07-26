using UnityEngine;
using System.Collections;
using System;

public class Kali : Character {



    void Start() {
        allMoves = new string[1] {"halt"}; //modify this later to include more moves
        allWeapons = new string[1] { "green" };
        currentSpecial = allMoves[0];
        currentWeapon = allWeapons[0];
        if (allMoves.Length <= 1) {
            cantSwitchSpecial = true;
        }
    }

    public virtual void ChangeSpecialUp()
    {
        base.ChangeSpecialUp();
        //check if not have this explict return here works, as in is it possible to make
        //the return in the base function propogate and make this function prematurely end as well
        if (cantSwitchSpecial) {
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
        Debug.Log("This is also polymorphism, bitch");
        int specialValue = self.specialGauge.GetSpecialValue();
        int ATB = self.ATB; 

        bool returnVal = false;
        if (ATB <= 0) {
            return returnVal;
        }
        switch (currentSpecial) {
            case "halt":
                 Debug.Log("halt is reached");
                if (specialValue >= 100)
                {
                    Debug.Log("halt worked");
                    self.turnBlue();
                    startedSpecial = true;
                    returnVal = true;
                }
                else { 
                    returnVal = false;
                }
                //the following invocation works for both the case where it actually works, and
                //when it doesn't
                self.specialGauge.ReduceSpecialValue(100);
                break;
                
            case "charge":
                 
                if (specialValue >= 100)
                {

                    returnVal = true;
                }
                else {
                    returnVal = false;
                }
                break;
            default:
                returnVal = false;
                break;
        }
        return returnVal;
    }

    //Pass in whoever the target of your special is AKA the unit you may attack or heal
    public override void CheckIfExecuteSpecial(Unit self, Unit target)
    {
        Debug.Log("Polymorphism bitch");
        if (startedSpecial) {
            
            switch (currentSpecial) {
                case "halt":
                    if (target is Enemy) {
                        target.LoseATB(100);
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
        if (currentWeapon.Equals("green"))
        {
            retVal = shooter.specialGauge.ReduceSpecialValue(25);
            if (retVal)
            {
                shooter.LoseATB(shooter.ATBCost);
                GameObject bullet = Instantiate(shooter.bullet, 
                    new Vector3(shooter.x, shooter.y, 0), Quaternion.identity)
                    as GameObject;
                
                bullet.GetComponent<Bullet>().tempDestX = gridSelector.GetComponent<GridSelector>().x;
                bullet.GetComponent<Bullet>().tempDestY = gridSelector.GetComponent<GridSelector>().y;
                bullet.GetComponent<Bullet>().destinationSet = true;
            }
        }
        return retVal;
    }
}
