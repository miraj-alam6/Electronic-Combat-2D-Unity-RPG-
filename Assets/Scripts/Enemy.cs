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
    public bool recalculateMoveInMiddle = false;
    public AudioClip enemyAttackSound1;
    public AudioClip enemyAttackSound2;
    public AudioClip meleeSound;
    public AudioClip hitSound;
    public AudioClip dieSound;
    public AudioClip startSpecialSound;
    public List<int> movesToDo;
    public bool canBreakWalls = false;
    public string targetName = "Kali";
    public int AIStyle = 0;
    public int acceptableDistance = 5;
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
        return 5;
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
        return 5;
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
        if(AIStyle != 0)
        {
            SmartThink();
            return;
        }
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
        if (movesToDo.Count <= 0)
        {
            string origName = targetName;
            targetName = "Hugo";
            destinationX = getTargetX();
            destinationY = getTargetY();
            movesToDo = GameManager.instance.gameCalculation.getShortestPath(x, y, destinationX, destinationY, true);
            targetName = origName;
        }
        if (movesToDo.Count <= 0)
        {

            string origName = targetName;
            targetName = "Winoa";
            destinationX = getTargetX();
            destinationY = getTargetY();
            movesToDo = GameManager.instance.gameCalculation.getShortestPath(x, y, destinationX, destinationY, true);
            targetName = origName;
        }
        // GameManager.instance.gameCalculation.printList(movesToDo);
        //  Debug.Log("End Think");
    }

    public void SmartThink()
    {
        //Debug.Log("Begin Think");

        movesToDo = new List<int>();
       /* Debug.Log(GameManager.instance.currentLevel.kaliHere);
        Debug.Log(GameManager.instance.currentLevel.winoaHere);
        Debug.Log(GameManager.instance.currentLevel.hugoHere);
        Debug.Log(GameManager.instance.currentLevel.alejandraHere);
        */
        List<int> kaliMovesToDo = new List<int>();
        int destinationX;
        int destinationY;

        if (GameManager.instance.currentLevel.kaliHere) {
            targetName = "Kali"; //need this for the next time that you do it.
            destinationX = getTargetX();
            destinationY = getTargetY();
            if (AIStyle == 1)
            {
                kaliMovesToDo = GameManager.instance.gameCalculation.getShortestPath(x, y, destinationX, destinationY, true);
            }
            else {
                kaliMovesToDo = GameManager.instance.gameCalculation.getShortestPath(x, y, destinationX, destinationY, false);
            }
        }


        List<int> winoaMovesToDo = new List<int>();
        if (GameManager.instance.currentLevel.winoaHere)
        {

            targetName = "Winoa";
            destinationX = getTargetX();
            destinationY = getTargetY();
            if (AIStyle == 1)
            {
                winoaMovesToDo = GameManager.instance.gameCalculation.getShortestPath(x, y, destinationX, destinationY, true);
            }
            else {
                winoaMovesToDo = GameManager.instance.gameCalculation.getShortestPath(x, y, destinationX, destinationY, false);
            }
            //Debug.Log("Reach 1");
        }

        List<int> hugoMovesToDo = new List<int>();
        if (GameManager.instance.currentLevel.hugoHere)
        {
            targetName = "Hugo";
            destinationX = getTargetX();
            destinationY = getTargetY();

            if (AIStyle == 1)
            {
                hugoMovesToDo = GameManager.instance.gameCalculation.getShortestPath(x, y, destinationX, destinationY, true);
            }
            else {
                hugoMovesToDo = GameManager.instance.gameCalculation.getShortestPath(x, y, destinationX, destinationY, false);
            }
            //Debug.Log("Reach 2");
        }
        List<int> alejandraMovesToDo = new List<int>();
        if (GameManager.instance.currentLevel.alejandraHere)
        {
            targetName = "Alejandra";
            destinationX = getTargetX();
            destinationY = getTargetY();

            if (AIStyle == 1 || AIStyle == 3)
            {
                alejandraMovesToDo = GameManager.instance.gameCalculation.getShortestPath(x, y, destinationX, destinationY, true);
            }
            else {
                alejandraMovesToDo = GameManager.instance.gameCalculation.getShortestPath(x, y, destinationX, destinationY, false);
            }
        }

        int shortestDistanceIndex = -1; //indices: 0 is Kali, 1 is Winoa, 2 is Hugo, 3 is Alejandra 
        int shortestDistanceSoFar = 4000;
        /*Debug.Log("Reached Smart Think");
        Debug.Log("kaliMoves" + kaliMovesToDo.Count);
        Debug.Log("winoaMoves" + winoaMovesToDo.Count);
        Debug.Log("hugoMoves" + hugoMovesToDo.Count);
        Debug.Log("alejandraMoves" + alejandraMovesToDo.Count);
        */
        if (kaliMovesToDo.Count != 0) {
            
            shortestDistanceIndex = 0;
            shortestDistanceSoFar = kaliMovesToDo.Count;
        }
        if (winoaMovesToDo.Count != 0 && winoaMovesToDo.Count < shortestDistanceSoFar)
        {
            shortestDistanceIndex = 1;
            shortestDistanceSoFar = winoaMovesToDo.Count;
        }
        if (hugoMovesToDo.Count != 0 && hugoMovesToDo.Count < shortestDistanceSoFar)
        {
            shortestDistanceIndex = 2;
            shortestDistanceSoFar = hugoMovesToDo.Count;
        }

        if (alejandraMovesToDo.Count != 0 && alejandraMovesToDo.Count < shortestDistanceSoFar)
        {
            shortestDistanceIndex = 3;
            shortestDistanceSoFar = alejandraMovesToDo.Count;
        }

        if(AIStyle == 3)
        {
            int excludeAlejandraShortestDistanceIndex = -1;
            int excludeAlejandraShortestDistanceSoFar = 4000;
            if (kaliMovesToDo.Count != 0)
            {

                excludeAlejandraShortestDistanceIndex = 0;
                excludeAlejandraShortestDistanceSoFar = kaliMovesToDo.Count;
            }
            if (winoaMovesToDo.Count != 0 && winoaMovesToDo.Count < excludeAlejandraShortestDistanceSoFar)
            {
                excludeAlejandraShortestDistanceIndex = 1;
                excludeAlejandraShortestDistanceSoFar = winoaMovesToDo.Count;
            }
            if (hugoMovesToDo.Count != 0 && hugoMovesToDo.Count < excludeAlejandraShortestDistanceSoFar)
            {
                excludeAlejandraShortestDistanceIndex = 2;
                excludeAlejandraShortestDistanceSoFar = hugoMovesToDo.Count;
            }

            if (excludeAlejandraShortestDistanceSoFar < acceptableDistance) {
                shortestDistanceIndex = excludeAlejandraShortestDistanceIndex;
                shortestDistanceSoFar = excludeAlejandraShortestDistanceSoFar;
            }
        }
        switch (shortestDistanceIndex) {
            case -1:
                break;
            case 0:
                movesToDo = kaliMovesToDo;
                break;
            case 1:
                movesToDo = winoaMovesToDo;
                break;
            case 2:
                movesToDo = hugoMovesToDo;
                break;
            case 3:
                movesToDo = alejandraMovesToDo;
                break;

        }
        
        if (AIStyle == 2 && shortestDistanceSoFar > acceptableDistance) {
            movesToDo = new List<int>();
        }
        Debug.Log("Reach 4. Moves to do:" + movesToDo.Count);
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
            if (name.Equals("Roxanne") && specialGauge.GetSpecialValue() >= 75) {
                RoxanneSpecial();
                return;
            }
            recalculateMoveInMiddle = false;
            movePoints = -200;
            ATB = -200;
        }
    }
    public void RoxanneSpecial() {
        flashOrange(3.0f);
        SoundManager.instance.PlaySingle(2, startSpecialSound);
        specialGauge.ReduceSpecialValue(75);
        SetATB(100);
    }
    //Game manager will call this on each enemy in our enemy list
    public void MoveEnemy() {
        //Debug.Log("Reach here 5");
        if (recalculateMoveInMiddle) {
            Debug.Log("reached here");
            Think();
        }
        GameManager.instance.UpdateCamera();
        if (GameManager.instance.stopAll) {
            return;
        }
        movePoints--;
        LoseATB(ATBCost);
        if (movesToDo.Count > 0) {
            direction = movesToDo[0];
        }
        else if (AIStyle == 2) {
            LoseATB(100);
            return;
        }
        else
        {
            direction = Random.Range(1, 5); //this range is exclusive, so make it 5
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
        //Debug.Log("Reach 8");
    }

    protected IEnumerator DieAnimation()
    {
        
        yield return new WaitForSeconds(2);

        dead = true;
        GameManager.instance.gameCalculation.actualGrid[y, x].hasEnemy = false;
        GameManager.instance.gameCalculation.actualGrid[y, x].walkable = true;
        GameManager.instance.enemyDying = false;
        GameManager.instance.stopAll = false;
    }

    //changed this to a bool. It returns true if the enemy was killed, and false if the
    //enemy was not killed.
    public bool LoseHP(int attack, int attackerDirection)
    {
        //this is to stop this weird glitch where you can hit an enemy while it is dying
        //sometimes, thus you can cheat to get a lot of special
        if (HP <= 0) {
            if (name.Equals("Roxanne")) { 
                GameManager.instance.currentLevel.updateLevel("defeat_boss");
            }
            return false;
        }
        SoundManager.instance.PlaySingle(3,hitSound);
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
            if (name.Equals("Roxanne"))
            {
                GameManager.instance.currentLevel.updateLevel("defeat_boss");
            }
            GameManager.instance.stopAll = true;
            SoundManager.instance.PlaySingle(4, dieSound);
            GameManager.instance.enemyDying = true;
            StartCoroutine(DieAnimation()); 
        }
        if (healthBar) { 
            healthBar.UpdateVitalBar(MaxHP, HP);
        }
        if (HP > 0) { 
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
        }
        else
        {
            if (direction == 1)
            {
                animator.SetTrigger("DieDown");
            }
            else if (direction == 2)
            {
                animator.SetTrigger("DieUp");
            }
            else if (direction == 3)
            {
                animator.SetTrigger("DieRight");
            }
            else if (direction == 4)
            {
                animator.SetTrigger("DieLeft");
            }
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
                if (hitWall.DamageWall(attack)) {
                    specialGauge.AddSpecialValue(4*specialGainRate);
                }
            };
            movePoints--;
        }
        if (component is Pot)
        {
            Pot hitPot = component as Pot; //reminder: as casts something
            if (hitPot.DamagePot(attack))
            {
                recalculateMoveInMiddle = true;
            }
            movePoints--;
        }
        if (component is Player) {
            LoseATB((int)(ATBCost * 0.5));
            SoundManager.instance.PlaySingle(2, meleeSound);
            SoundManager.instance.RandomizeSfx(3,enemyAttackSound1,enemyAttackSound2);
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

    public void initializeLevel12Stats(int enemyDifficulty) {
        switch (enemyDifficulty) {
            case 0:
                MaxHP = 30;
                HP = 30;
                attack = 5;
                defense = 1;
                minSpeed = 3;
                maxSpeed = 8;
                ATBCost = 14;
                break;
            case 1:
                MaxHP = 35;
                HP = 35;
                attack = 5;
                defense = 2;
                minSpeed = 3;
                maxSpeed = 8;
                ATBCost = 14;
                break;
            case 2:
                MaxHP = 35;
                HP = 35;
                attack = 6;
                defense = 3;
                minSpeed = 3;
                maxSpeed = 8;
                ATBCost = 13;
                break;
            case 3:
                MaxHP = 40;
                HP = 40;
                attack = 6;
                defense = 3;
                minSpeed = 3;
                maxSpeed = 8;
                ATBCost = 13;
                break;
            case 4:
                MaxHP = 45;
                HP = 45;
                attack = 6;
                defense = 3;
                minSpeed = 3;
                maxSpeed = 8;
                ATBCost = 13;
                break;
            case 5:
                MaxHP = 45;
                HP = 45;
                attack = 7;
                defense = 3;
                minSpeed = 3;
                maxSpeed = 8;
                ATBCost = 13;
                break;
            case 6:
                MaxHP = 50;
                HP = 50;
                attack = 7;
                defense = 3;
                minSpeed = 3;
                maxSpeed = 9;
                ATBCost = 13;
                break;
            case 7:
                MaxHP = 50;
                HP = 50;
                attack = 8;
                defense = 3;
                minSpeed = 4;
                maxSpeed = 10;
                ATBCost = 13;
                break;
            default:
                MaxHP = 50;
                HP = 50;
                attack = 8;
                defense = 3;
                minSpeed = 4;
                maxSpeed = 10;
                ATBCost = 13;
                break;


        }
    }
}
