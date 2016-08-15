using UnityEngine;
using System.Collections;

public abstract class Character : MonoBehaviour {

    protected string currentSpecial ="none";
    protected string currentWeapon = "none";
    protected bool startedSpecial;
    protected bool cantSwitchSpecial;
    protected  bool executedSpecial = false;
    protected string[] allMoves;
    protected string[] allWeapons;
    public AudioClip startSpecialSound;
	public void ChangeSpecialUp () {
        if (startedSpecial) {
            cantSwitchSpecial = true;
        } 
	}
	
	public void ChangeSpecialDown() {
        if (startedSpecial)
        {
            cantSwitchSpecial = true;
        }
    }

    public abstract bool StartSpecial(Unit self);

    public abstract void CheckIfExecuteSpecial(Unit self, Unit target);
    //call this if turn ended before finishing
    public void wasteSpecial(Unit self)
    {
        if (startedSpecial) { 
            startedSpecial = false;
            self.becomeNormal();
        }
        self.InflictDamage = true;
    }
    public abstract bool fireWeapon(Player shooter, GameObject gridSelector);
}
