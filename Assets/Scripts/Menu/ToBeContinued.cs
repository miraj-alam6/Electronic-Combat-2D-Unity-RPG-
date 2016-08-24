using UnityEngine;
using System.Collections;

public class ToBeContinued : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GameManager.instance.Invoke("goBackToTitleScreen", 5.0f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
