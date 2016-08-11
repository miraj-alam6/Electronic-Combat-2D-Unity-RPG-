using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour {
    //sounds for when you chop the wall
    public AudioClip chopSound1;
    public AudioClip chopSound2;

    public Sprite dmgSprite; //the sprite of the wall that will be shown when the player hits it
	// Use this for initialization
	public int hp = 4; //this is the health of the wall
    public int x, y;
    public bool analyzedAlready = false;
    private SpriteRenderer spriteRenderer;
    void Start() {
        x = (int)GetComponent<Transform>().position.x;
        y = (int)GetComponent<Transform>().position.y;
        GameManager.instance.gameCalculation.actualGrid[y, x].hasWall = true;
        GameManager.instance.gameCalculation.actualGrid[y, x].walkable = false;
    }
    void Awake () {
        spriteRenderer = GetComponent<SpriteRenderer>();    
	
	}
    public bool DamageWall(int loss) {
        SoundManager.instance.RandomizeSfx(chopSound1, chopSound2);
        spriteRenderer.sprite = dmgSprite;
        hp -= loss;
        if (hp <= 0) {
            GameManager.instance.gameCalculation.actualGrid[y, x].hasWall = false;
            GameManager.instance.gameCalculation.actualGrid[y, x].walkable = true;
            gameObject.SetActive(false);
 
        }
        return (hp <= 0);
    }	
}
