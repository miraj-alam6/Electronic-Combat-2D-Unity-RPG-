using UnityEngine;
using System.Collections;

public class Collectible : MonoBehaviour {

    public string type;
    public int quantity;

    public void gainCollectible(){
        string message = "";
        
        
        switch (type) {
            case "Battery":
                Debug.Log("You got " + quantity + "battery box.");
                message = "You got " + quantity + "\nbattery box";
                GameManager.instance.gameData.addBatteryBoxCount(quantity);
                if (GameManager.instance.currentLevel is TutorialLevel9)
                {
                    message += ("You have a total of " +
                        (((TutorialLevel10)GameManager.instance.currentLevel).currentElectric+ quantity) +
                        " battery boxes.");
                }
                //Debug.Log("You have a total of "+ GameManager.instance.gameData.batteryBoxCount + " battery box.");
                GameManager.instance.currentLevel.updateLevel("got_battery");
                break;
            case "Chicken":
                message = "You got " + quantity + " chicken flower.";
                if (GameManager.instance.currentLevel is TutorialLevel9) {
                    message += ("You have a total of "+ 
                        (((TutorialLevel9)GameManager.instance.currentLevel).chickens + quantity) + 
                        " chicken flowers.");
                }
                //Debug.Log("You got a chicken flower.");
                GameManager.instance.currentLevel.updateLevel("got_chicken");
                break;
            case "Burger":
                message = "You got " + quantity + "\nburger.";
                Debug.Log("You got " + quantity + " burger");
                GameManager.instance.gameData.addBurgerCount(quantity);
                Debug.Log("You have a total of " + GameManager.instance.gameData.burgerCount + " burger.");
                break;

        }
        GameManager.instance.showMessage(message);
        gameObject.SetActive(false);
    }
}
