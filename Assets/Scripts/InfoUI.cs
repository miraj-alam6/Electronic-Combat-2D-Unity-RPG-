using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InfoUI : MonoBehaviour
{
    public Text turnMessage;
    public Text actualMessage;
    public Text actualObjective;
    // Use this for initialization
    void Start()
    {


    }

    public void SetMessage(string message)
    {
        actualMessage.text = message;
    }
    public void SetActualObjective(string message)
    {
        actualObjective.text = message;
    }
    public void SetTurnMessage(string message)
    {
        turnMessage.text = message;
    }
    // Update is called once per frame
    void Update()
    {

    }
}
