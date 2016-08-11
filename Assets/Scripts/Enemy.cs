using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;
public class Enemy : Unit {
   
    public int playerDamage; //amount of damage inflicted onto the player
    public bool analyzedAlready = false;
    //private Animator animator;
    
    private bool skipMove; //use this to make enemy move every other turn
    
    public AudioClip enemyAttackSound1;
    public AudioClip enemyAttackSound2;
    public List<int> movesToDo;
    public bool canBreakWalls = false;
    public string targetName = "Kali";
    private Transform targetTransform; //stores player's position that will tell enemy where to move
                                       // private bool dyingAnim = false;
                                       //public int MoveCount = 6;

    // Use this for initialization

    public int getTargetX() {
        for (int i = 0; i < GameManager.instance.players.Count; i++) {
            if (GameManager.instance.players[i].name.Equals(targetName)) {
                return GameManager.instance.players[i].x;
            }
        }
        return 0;
    }

    public int getTargetY()
    {
        for (int i = 0; i < GameManager.instance.players.Count; i++)
        {
            if (GameManager.instance.players[i].name.Equals(targetName))
            {

                return GameManager.instance.players[i].y;
            }
        }
        return 0;
    }

    protected override void Start () {
        //First off, enemy must add itself to the game manager

        isPlayer = false;
        GameManager.instance.AddEnemyToList(this);
        animator = GetComponent<Animator>();
        targetTransform = GameObject.FindGameObjectWithTag("Player").transform;
        base.Start();
        GameManager.instance.gameCalculation.actualGrid[y, x].hasEnemy = true;
        GameManager.instance.gameCalculation.actualGrid[y, x].walkable = false;
        if (displayVitals)
        {
            VitalBar[] vitals = GetComponentsInChildren<VitalBar>();
          //  Debug.Log(vitals[0].type);
          //  Debug.Log(vitals[1].type);
            if (vitals[0].type.Equals("HEALTH"))
            {
                healthBar = vitals[0];
            }
            else {
                healthBar = vitals[1];
            }
            if (vitals[1].type.Equals("ATB"))
            {
                ATBBar = vitals[1];
            }
            else {
                ATBBar = vitals[0];
            }
        }

    }

    // Update is called once per frame
    void Update () {
        base.Update();
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
        //int playerIndex = Random.Range(0,GameManager.instance.players.Count);
        //Debug.Log(playerIndex);
        int destinationX = getTargetX();
        int destinationY = getTargetY();
       // Debug.Log("Current Position:" + x + "," + y);
       /// Debug.Log("Player Position:" + destinationX + ","+ destinationY);
        
        movesToDo = GameManager.instance.gameCalculation.getShortestPath(x,y,destinationX,destinationY,false);
        if (movesToDo.Count <= 0 && canBreakWalls) {
            movesToDo = GameManager.instance.gameCalculation.getShortestPath(x, y, destinationX, destinationY, true);
        }
        if(movesToDo.Count <= 0)
        {
            string origName = targetName;
            targetName = "Alejandra";
            destinationX = getTargetX();
            destinationY = getTargetY();
            movesToDo = GameManager.instance.gameCalculation.getShortestPath(x, y, destinationX, destinationY, true);
            targetName = origName;
        }
       // GameManager.instance.gameCalculation.printList(movesToDo);
      //  Debug.Log("End Think");
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
        if (ATB <= 0) {
            movePoints = -200;
            ATB = -200;
        }
    }

    //Game manager will call this on each enemy in our enemy list
    public void MoveEnemy() {
        GameManager.instance.UpdateCamera();
        if (GameManager.instance.stopAll) {
            return;
        }
        movePoints--;
        LoseATB(ATBCost);
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
            if (animator)
            {
                animator.SetTrigger("MoveDown");
            }
        }
        else if (direction == 2)
        {
            xDir = 0;
            yDir = 1;
            if (animator) { 
                animator.SetTrigger("MoveUp");
            }
        }
        else if (direction == 3)
        {
            xDir = 1;
            yDir = 0;
            if (animator)
            {
                animator.SetTrigger("MoveRight");
            }
        }
        else if (direction == 4)
        {
            xDir = -1;
            yDir = 0;
            if (animator)
            {
                animator.SetTrigger("MoveLeft");
            }
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

    //changed this to a bool. It returns true if the enemy was killed, and false if the
    //enemy was not killed.
    public bool LoseHP(int attack, int attackerDirection)
    {

        float multiplier = 1.0f;

        if (checkIfBackAttack(attackerDirection, direction))
        {
            multiplier = 1.4f;
        }

        int damage = ((int)(multiplier * attack) - defense);
        if (damage < 0)
        {
            damage = 0;
        }
        HP -= damage;
        if (HP <= 0) {
            HP = 0;
            ATB = 0;
            GameManager.instance.stopAll = true;
            StartCoroutine(DieAnimation()); 
        }
        if (healthBar) { 
            healthBar.UpdateVitalBar(MaxHP, HP);
        }
        if (direction == 1)
        {
            animator.SetTrigger("HitDown");
        }
        else if (direction == 2)
        {
            animator.SetTrigger("HitUp");
        }
        else if (direction == 3)
        {
            animator.SetTrigger("HitRight");
        }
        else if (direction == 4)
        {
            animator.SetTrigger("HitLeft");
        }
        return (HP == 0);
    }
    //now the abstract function
    protected override void OnCantMove<T>(T component)
    {
        if (component is Wall)
        {
            Wall hitWall = component as Wall; //reminder: as casts something
            if (name.Equals("RedRobo")) {
                hitWall.DamageWall(4* attack);
            }
            else {
                hitWall.DamageWall(attack);
                    };
            movePoints--;
        }
        if (component is Pot)
        {
            Pot hitPot = component as Pot; //reminder: as casts something
            hitPot.DamagePot(attack);
            movePoints--;
        }
        if (component is Player) {
            LoseATB((int)(ATBCost * 0.5)); 
            SoundManager.instance.RandomizeSfx(enemyAttackSound1,enemyAttackSound2);
            Player hitPlayer = component as Player;
            if (direction == 1) { 
                animator.SetTrigger("AttackDown");
            }
            else if (direction == 2)
            {
                animator.SetTrigger("AttackUp");
            }
            else if (direction == 3)
            {
                animator.SetTrigger("AttackRight");
            }
            else if (direction == 4)
            {
                animator.SetTrigger("AttackLeft");
            }
            hitPlayer.LoseHP(attack, direction);
        }
    }


}
