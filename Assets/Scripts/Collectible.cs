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
                Debug.Log("You have a total of "+ GameManager.instance.gameData.batteryBoxCount + " battery box.");
                break;
            case "Chicken":
                message = "You got" + quantity + "\nchicken flower.";
                Debug.Log("You got a chicken flower.");
                break;
            case "Burger":
                message = "You got " + quantity + "\nburger.";
                Debug.Log("You got " + quantity + " burger");
                GameManager.instance.gameData.addBurgerCount(quantity);
                Debug.Log("You have a total of " + GameManager.instance.gameData.burgerCount + " burger.");
                break;

        }
        GameManager.instance.RightUI.GetComponent<MessageUI>().SetMessage(message);
        gameObject.SetActive(false);
    }
}
