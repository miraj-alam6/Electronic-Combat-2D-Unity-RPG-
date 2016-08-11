using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MessageUI : MonoBehaviour {
    public Text actualMessage;
    // Use this for initialization
    void Start () {
        
	
	}

    public void SetMessage(string message) {
        message = LineizeString(message, 30);
        actualMessage.text = message;
    }

    public string LineizeString(string original, int sublength) {
        string result = "";
        string temp = original;
        while (temp.Length > sublength) {
            //need an inner loop to find real sublength AKA the cutoff so that one word doesn't get
            //cut in the middle
            int realSubLength = sublength;
            while (!(temp[realSubLength-1].Equals(' '))) {
                realSubLength--;
            }
            result += temp.Substring(0, realSubLength);
            result += "\n";
            temp = temp.Substring(realSubLength);
        }
        //whatever remains in the temp may be things other than empty string because loop
        //stop as soon as string becomes smaller than sublength.
        if (temp[0].Equals(' '))
        {
            result += temp.Substring(1);
        }
        else { 
            result += temp;
        }
        result += "\n";
        return result;
    }
    
    // Update is called once per frame
    void Update () {
	
	}
}
