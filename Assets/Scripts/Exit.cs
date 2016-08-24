using UnityEngine;
using System.Collections;

public class Exit : MonoBehaviour {
    public int x, y;

    // Use this for initialization
    void Start () {
        x = (int)GetComponent<Transform>().position.x;
        y = (int)GetComponent<Transform>().position.y;
        GameManager.instance.gameCalculation.actualGrid[y, x].hasEnemy = true;
        GameManager.instance.gameCalculation.actualGrid[y, x].walkable = false;
    }
	
	// Update is called once per frame
	void Update () {
        GameManager.instance.gameCalculation.actualGrid[y, x].hasEnemy = true;
        GameManager.instance.gameCalculation.actualGrid[y, x].walkable = false;
    }
}
