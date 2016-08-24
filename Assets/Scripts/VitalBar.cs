using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class VitalBar : MonoBehaviour {
    public string type; //Will either be HP or ATB
    RectTransform transform;
    public bool isUI;
    public Text HealthText;
    public Text ATBText;
    public RawImage background;
   // int HP = 4000;
    void Start () {
        transform = GetComponent<RectTransform>();
        if (type.Equals("ATB"))
        {
            UpdateVitalBar(1, 0);
        }
        else
        { 
            UpdateVitalBar(1,1);
        }
    }
	
	// Update is called once per frame
	void Update () {
       
        //   Debug.Log("what  what");
        //   UpdateVitalBar(20, 10);
        //       HP--; 
        //       UpdateVitalBar(4000, HP);
    }

    //Call this whenever you change health
    public void UpdateVitalBar(int maxVal, int currVal) {
        // Debug.Log(transform.rect.width);
        // Debug.Log("vat");

        if (transform) { 
        transform.localScale = new Vector3((float)currVal/maxVal,1.0f,1.0f);
        }
        if (background) {
            if (type.Equals("ATB")) {
                background.color = Color.black;
                if (currVal == 0) { 
                    background.color = Color.white;
                }
            }
        }
        //Debug.Log(transform.rect.width);
    }
    //Call this whenever you change health
    public void BecomeBlue()
    {
        if (transform) { 
            transform.gameObject.GetComponent<RawImage>().color = Color.blue;
        }
        
    }
    //Call this whenever you change health
    public void BecomeGreen()
    {
        if (transform) { 
            transform.gameObject.GetComponent<RawImage>().color = Color.green;
        }
    }
}
