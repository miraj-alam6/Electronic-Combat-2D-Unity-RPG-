using UnityEngine;
using System.Collections;

public class LogoScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Invoke("LoadTitleScreen",0.1f);
	}

    public void LoadTitleScreen() {

        Debug.Log("What the hell");
        Application.LoadLevel("_Scenes/TitleScreen");
    }	
}
