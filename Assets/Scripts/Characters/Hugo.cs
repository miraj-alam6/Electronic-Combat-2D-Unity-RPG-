using UnityEngine;
using System.Collections;


public class Hugo : Character
{


    InfoUI actualInfo;
    void Start()
    {
        actualInfo = GameManager.instance.RightUI.GetComponent<InfoUI>();
        allMoves = new string[1] { "turnaround" }; //modify this later to include more moves
        allWeapons = new string[1] { "analyzer" };
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
        if (startedSpecial) {
            return true;
        }
        int specialValue = self.specialGauge.GetSpecialValue();
        int ATB = self.ATB;

        bool returnVal = false;
        if (ATB <= 0)
        {
            return returnVal;
        }
        switch (currentSpecial)
        {
            case "turnaround":
                //Debug.Log("turnaround is reached");
                if (self.specialGauge.ReduceSpecialValue(50))
                {
                    Debug.Log("turnaround worked");
                    self.turnRed() ;
                    startedSpecial = true;
                    returnVal = true;
                }
                else {
                    returnVal = false;
                }
                //the following invocation works for both the case where it actually works, and
                //when it doesn't
                
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
        if (startedSpecial)
        {

            switch (currentSpecial)
            {
                case "turnaround":
                    if (target is Unit)
                    {
                        
                        target.setDirection(self.direction);
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


        if (currentWeapon.Equals("analyzer"))
        {
            
            
            
            string category = gridSelector.GetComponent<GridSelector>().shortmessage;
            bool analyzedAlready = gridSelector.GetComponent<GridSelector>().analyzedAlready;
            if (analyzedAlready) {
                GameManager.instance.RightUI.GetComponent<InfoUI>().SetMessage(
                gridSelector.GetComponent<GridSelector>().message);
                return true;
            }

            if (category.Equals("player")
                || category.Equals("none"))
            {
                retVal = shooter.specialGauge.ReduceSpecialValue(0);
             }
            else if(category.Equals("enemy"))
            {
                retVal = shooter.specialGauge.ReduceSpecialValue(6);
                
                if (retVal) { 
                    gridSelector.GetComponent<GridSelector>().setAnalyzed();
                    shooter.LoseATB(5);
                }
            }
            else if (category.Equals("wall"))
            {
                retVal = shooter.specialGauge.ReduceSpecialValue(2);

                if (retVal)
                {
                    gridSelector.GetComponent<GridSelector>().setAnalyzed();
                    shooter.LoseATB(5);
                }
            }

            if (retVal) {
                actualInfo.SetMessage(
                gridSelector.GetComponent<GridSelector>().message);
            }
        }
        
        return retVal;
    }
}
