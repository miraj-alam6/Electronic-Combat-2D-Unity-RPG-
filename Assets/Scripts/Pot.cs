using UnityEngine;
using System.Collections;

public class Pot : MonoBehaviour {
    public int x, y;
    public int hp = 20; //this is the health of the pot
    public Sprite brokenPot; //the sprite of the wall that will be shown when the player hits it
    private SpriteRenderer spriteRenderer;
    public AudioClip hitPotSound1;
    public AudioClip hitPotSound2;
    public AudioClip potBreakSound;
    private Animator animator;
    private Collider2D collider;
    public GameObject collectible;
    public int weight = 10;
    public bool analyzedAlready = false;
    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
        collider = GetComponent<Collider2D>();
        x = (int)GetComponent<Transform>().position.x;
        y = (int)GetComponent<Transform>().position.y;
        GameManager.instance.gameCalculation.actualGrid[y, x].hasPot = true;
        GameManager.instance.gameCalculation.actualGrid[y, x].walkable = false;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public bool DamagePot(int loss)
    {
        SoundManager.instance.RandomizeSfx(1, hitPotSound1, hitPotSound2);
        animator.SetTrigger("Hit");
        hp -= loss;
        if (hp <= 0)
        {
            GameManager.instance.gameCalculation.actualGrid[y, x].hasPot = false;
            GameManager.instance.gameCalculation.actualGrid[y, x].walkable = true;
            collider.enabled = false;
            //gameObject.SetActive(false);
            animator.SetTrigger("Break");
            SoundManager.instance.PlaySingle(4, potBreakSound);
            GameManager.instance.currentLevel.updateLevel("broke_pot");
            //if the pot contains a collectible, instantiate it
            if (collectible) {
                Instantiate(collectible, new Vector3(x, y, 0), Quaternion.identity);
            }
        }
        return (hp <= 0);
    }
}
