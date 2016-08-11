using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;
public class ScavengerEnemy : Enemy {
   
    public int playerDamage; //amount of damage inflicted onto the player

    private Animator animator;
    private Transform target; //stores player's position that will tell enemy where to move
    private bool skipMove; //use this to make enemy move every other turn
    
    public AudioClip enemyAttackSound1;
    public AudioClip enemyAttackSound2;
    public List<int> movesToDo;
   // private bool dyingAnim = false;
    //public int MoveCount = 6;

    // Use this for initialization
    protected override void Start () {
        //First off, enemy must add itself to the game manager
        isPlayer = false;
        GameManager.instance.AddEnemyToList(this);
        animator = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        base.Start();
        GameManager.instance.gameCalculation.actualGrid[y, x].hasEnemy = true;
        GameManager.instance.gameCalculation.actualGrid[y, x].walkable = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (dead)
        {
            GameManager.instance.currentLevel.updateLevel("killed_enemy");
            GameManager.instance.RemoveEnemyFromList(this);
            Destroy(this.gameObject);
        }
	}

    //This will generate a path to the player which will be used by the enemy
    public void Think() {
        Debug.Log("Begin Think");
        
        movesToDo = new List<int>();

        // int destinationX = (int) GameObject.FindGameObjectWithTag("Player").transform.position.x;
        // int destinationY = (int)GameObject.FindGameObjectWithTag("Player").transform.position.y;
        int playerIndex = Random.Range(0,GameManager.instance.players.Count);
        Debug.Log(playerIndex);
        int destinationX = GameManager.instance.players[playerIndex].x;
        int destinationY = GameManager.instance.players[playerIndex].y;
        Debug.Log("Current Position:" + x + "," + y);
        Debug.Log("Player Position:" + destinationX + ","+ destinationY);
        
        movesToDo = GameManager.instance.gameCalculation.getShortestPath(x,y,destinationX,destinationY,false);
        GameManager.instance.gameCalculation.printList(movesToDo);
        Debug.Log("End Think");
    }
    //T is the player for this class
    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        
        //Don't have skip move anymore
        //if (skipMove == true) {
        //    skipMove = false;
        //    return;
       // }
        base.AttemptMove<T>(xDir, yDir);
        //skipMove = true;
        if (movePoints <= 0) {
            movePoints = -200;
        }
    }

    //Game manager will call this on each enemy in our enemy list
    public void MoveEnemy() {
        movePoints--;
        int direction;
        if (movesToDo.Count > 0) {
         direction = movesToDo[0];
        }
        else
        {
            direction = Random.Range(1,5); //this range is exclusive, so make it 5
        }
     //the actula top of the list will be popped in Unit.cs in Move method   
        int xDir = 0;
        int yDir = 0;
        if (direction == 1)
        {
            xDir = 0;
            yDir = -1;
        }
        else if (direction == 2)
        {
            xDir = 0;
            yDir = 1;
        }
        else if (direction == 3)
        {
            xDir = 1;
            yDir = 0;
        }
        else if (direction == 4)
        {
            xDir = -1;
            yDir = 0;
        }
        AttemptMove<MonoBehaviour>(xDir, yDir); //no need for abstract function anymore
    }

    protected IEnumerator DieAnimation()
    {
        yield return new WaitForSeconds(1);
        dead = true;
        GameManager.instance.gameCalculation.actualGrid[y, x].hasEnemy = false;
        GameManager.instance.gameCalculation.actualGrid[y, x].walkable = true;
        GameManager.instance.stopAll = false;
    }

    public void LoseHP(int attack)
    {
     
        HP -= (attack - defense);
        if (HP <= 0) {
            HP = 0;
            ATB = 0;
            GameManager.instance.stopAll = true;
            StartCoroutine(DieAnimation()); 
        }
    }
    //now the abstract function
    protected override void OnCantMove<T>(T component)
    {
        if (component is Wall)
        {
            Wall hitWall = component as Wall; //reminder: as casts something
            hitWall.DamageWall(3);
            movePoints--;
        }

        if (component is Player) { 
            SoundManager.instance.RandomizeSfx(enemyAttackSound1,enemyAttackSound2);
            Player hitPlayer = component as Player;
            animator.SetTrigger("EnemyAttack");
            hitPlayer.LoseHP(attack,direction);
        }
    }
}
