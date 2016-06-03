using UnityEngine;
using System.Collections;
using System;

public class Enemy : MovingObject {
    public int playerDamage; //amount of damage inflicted onto the player

    private Animator animator;
    private Transform target; //stores player's position that will tell enemy where to move
    private bool skipMove; //use this to make enemy move every other turn

    public AudioClip enemyAttackSound1;
    public AudioClip enemyAttackSound2;
    //public int MoveCount = 6;

    // Use this for initialization
    protected override void Start () {
        //First off, enemy must add itself to the game manager
        GameManager.instance.AddEnemyToList(this);
        animator = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        base.Start();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //T is the player for this class
    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        movePoints--;
        //Don't have skip move anymore
        //if (skipMove == true) {
        //    skipMove = false;
        //    return;
       // }
        base.AttemptMove<T>(xDir, yDir);
        //skipMove = true;
    }

    //Game manager will call this on each enemy in our enemy list
    public void MoveEnemy() {
        int xDir = 0;
        int yDir = 0;
        //compare target and current position to determine which direction to move in.
        if (Mathf.Abs(target.position.x - transform.position.x) < float.Epsilon)
        {//means same column
            yDir = target.position.y > transform.position.y ? 1 : -1; //if target is higher than current,
            //than move up, else move down.
        }
        else
        {
            xDir = target.position.x > transform.position.x ? 1 : -1; //you should be able to understand this
        }
        AttemptMove<Player>(xDir, yDir);
    }

    //now the abstract function
    protected override void OnCantMove<T>(T component)
    {
        SoundManager.instance.RandomizeSfx(enemyAttackSound1,enemyAttackSound2);
        Player hitPlayer = component as Player;
        animator.SetTrigger("enemyAttack");
        hitPlayer.LoseFood(playerDamage);
    }
}
