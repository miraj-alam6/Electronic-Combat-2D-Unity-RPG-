using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour {
    //sounds for when you chop the wall
    public AudioClip chopSound1;
    public AudioClip chopSound2;

    public Sprite dmgSprite; //the sprite of the wall that will be shown when the player hits it
	// Use this for initialization
	public int hp = 4; //this is the health of the wall

    private SpriteRenderer spriteRenderer;

    void Awake () {
        spriteRenderer = GetComponent<SpriteRenderer>();    
	
	}
    public void DamageWall(int loss) {
        SoundManager.instance.RandomizeSfx(chopSound1, chopSound2);
        spriteRenderer.sprite = dmgSprite;
        hp -= loss;
        if (hp <= 0) {
            gameObject.SetActive(false);
        }
    }	
}
