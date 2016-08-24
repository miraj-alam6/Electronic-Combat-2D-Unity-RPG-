using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour {
    //sounds for when you chop the wall
    public AudioClip chopSound1;
    public AudioClip chopSound2;
    public bool hasDifficulty;
    public Sprite dmgSprite; //the sprite of the wall that will be shown when the player hits it
	// Use this for initialization
	public int hp = 4; //this is the health of the wall
    public int EasyHp = 4;
    public int NormalHp = 4;
    public int HardHp = 4;
    public int x, y;
    public bool analyzedAlready = false;
    private SpriteRenderer spriteRenderer;
    void Start() {
        if (hasDifficulty) {
            if (GameManager.instance.difficultyLevel <= 0) {
                hp = EasyHp;
            }
            else if (GameManager.instance.difficultyLevel == 1)
            {
                hp = NormalHp;
            }
            else if (GameManager.instance.difficultyLevel >= 2)
            {
                hp = HardHp;

            }
        }
        x = (int)GetComponent<Transform>().position.x;
        y = (int)GetComponent<Transform>().position.y;
        GameManager.instance.gameCalculation.actualGrid[y, x].hasWall = true;
        GameManager.instance.gameCalculation.actualGrid[y, x].walkable = false;
    }
    void Awake () {
        spriteRenderer = GetComponent<SpriteRenderer>();    
	
	}
    public bool DamageWall(int loss) {
        SoundManager.instance.RandomizeSfx(1,chopSound1, chopSound2);
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
