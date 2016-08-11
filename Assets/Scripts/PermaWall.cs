using UnityEngine;
using System.Collections;

public class PermaWall : MonoBehaviour {

    public int x, y;
    public bool analyzedAlready = false;

    // Use this for initialization
    void Start () {
        
        x = (int)GetComponent<Transform>().position.x;
        y = (int)GetComponent<Transform>().position.y;
        GameManager.instance.gameCalculation.actualGrid[y, x].hasPermaWall = true;
        GameManager.instance.gameCalculation.actualGrid[y, x].walkable = false;
    }
	

}
