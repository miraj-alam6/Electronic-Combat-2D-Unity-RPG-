using UnityEngine;
using System.Collections;
//this script will simply just check if a GameManger has been instantiate and if not
//then it will instantiate one.
//this script is added to our MainCamera
public class Loader : MonoBehaviour {
    public GameManager gameManager;

	// Use this for initialization
	void Awake () {
        if (GameManager.instance == null) { //here we are using the static variable that we created.
            Instantiate(gameManager); //instantiate the prefab
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
