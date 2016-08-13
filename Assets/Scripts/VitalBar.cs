using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class VitalBar : MonoBehaviour {
    public string type; //Will either be HP or ATB
    RectTransform transform;
   // int HP = 4000;
    void Start () {
        transform = GetComponent<RectTransform>();
        UpdateVitalBar(1,1);
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

        transform.localScale = new Vector3((float)currVal/maxVal,1.0f,1.0f);
        //Debug.Log(transform.rect.width);
    }
    //Call this whenever you change health
    public void BecomeBlue()
    {
        transform.gameObject.GetComponent<RawImage>().color = Color.blue;
    }
    //Call this whenever you change health
    public void BecomeGreen()
    {
        transform.gameObject.GetComponent<RawImage>().color = Color.green;
    }
}
