using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
//Player will inherit from Unit instead of MonoBehavior
public class Player : Unit {
    
    //assign character in inspector
    public Character character; //this holds all the special moves and stat growth info
    public int wallDamage = 1; //amount of damage that player inflicts onto the wall when chopping
    public int pointsPerFood = 10;
    public int pointsPerSoda = 20;
    public int bulletCost;
    public bool inputCoolDown = false;
    public bool popUpMenuBeingShown = false;

    public int inputCoolDownCounter = 0;
    
    [HideInInspector]public bool shooting = false;
    
    public float restartLevelDelay = 1f;
    public Text foodText;
    public bool isActivePlayer = false;
    //private Animator animator;
    //private int food; //stores score during level before passing it back to game manager.

    //Sound effects for the player
    public AudioClip moveSound1;
    public AudioClip moveSound2;
    public AudioClip eatSound1;
    public AudioClip eatSound2;
    public AudioClip drinkSound1;
    public AudioClip drinkSound2;
    public AudioClip gameOverSound;
    public int debuggingVariable = 0;
    private bool stopShooting;
    public GameObject bullet;
   
    // Use this for initialization
    protected override void Start() {

        VitalBar []vitals = GetComponentsInChildren<VitalBar>();

        if (displayVitals) { 
            //Debug.Log(vitals[0].type);
            //Debug.Log(vitals[1].type);
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
        animator = GetComponent<Animator>();
        isPlayer = true;
        base.Start();
        GameManager.instance.AddPlayerToList(this);
        GameManager.instance.gameCalculation.actualGrid[y, x].hasPlayer = true;
        GameManager.instance.gameCalculation.actualGrid[y, x].walkable = false;
        //set text after superclass sets movepoints
        updateText();
           
    }

    //when the player is disabled you want to store the score into the GameManager
    private void OnDisable() {
        //Here is where you would retain data from Player into your GameManager
        //Commented out the thing that is not applicable anymore 
        //GameManager.instance.playerFoodPoints = food;
    }
    // Update is called once per frame
    //we're gonna check if it's player turn or not
    void Update() {
        //Following is for moving around
        base.Update();

        if (dead)
        {
            dead = false;

            GameManager.instance.currentLevel.updateLevel("restart");

            //Destroy(this.gameObject);
        }
        int padHorizontal = 0;
        int padVertical = 0;

        padHorizontal = (int)Input.GetAxisRaw("Pad Horizontal");
        padVertical = (int)Input.GetAxisRaw("Pad Vertical");
        if (padHorizontal != 0 || padVertical != 0)
        {
            dpad(padHorizontal, padVertical);
        }

        if (Input.GetButtonDown("Submit"))
        {
            submit();
            return;
        }

        //for now, the menu button will turn hints on and off
        if (Input.GetButtonDown("Menu"))
        {
            menu();
            return;
        }

        if (!isActivePlayer) {
            return;
        }

       

        if (inputCoolDown)
        {
            inputCoolDownCounter++;
            if (inputCoolDownCounter >= 20) {
                inputCoolDownCounter = 0;
                inputCoolDown = false;
                stopShooting = false;
            }
        }
        

        if (Input.GetButtonDown("Cancel")){
            cancel();
        }
        if (Input.GetAxisRaw("Shoot") > 0) {
            rightTrigger();
        }

        //these are commands besides moving around
        if (Input.GetButtonDown("Special")) {
            //GameManager.instance.gameCalculation.printGrid();
            rightBumper();
        }
            //Skip turn: you can end turn whenever you want with Backspace
        if (Input.GetButtonDown("Start"))
        //you need the second of the above conditions to prevent mashed output from messing you
        //up
        {
            Invoke("startButton",0.3f);
            return;
        }


        

        //now if it is player turn, then...
        //gonna use the following to store the direction we're moving
        //along an axis as either 1 or -1.
        int horizontal = 0;
        int vertical = 0;

        horizontal = (int)Input.GetAxisRaw("Horizontal");
        vertical = (int)Input.GetAxisRaw("Vertical");
        if (horizontal != 0 || vertical != 0) {
            mainstick(horizontal,vertical);
        }


    }

    //Having <T> there, but then having no T is your parameters means that
    //you will encounter the generic within the body of your function rather than
    //your parameters
    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        debuggingVariable = yDir;
        if (yDir == 1) {
            direction = 2;
            //need to reset all other triggers to prevent triggers from staying on instead of
            //automatically turning off
            //animator.ResetTrigger("MoveDown");
            //animator.ResetTrigger("MoveUp");
            //animator.ResetTrigger("MoveRight");
            //animator.ResetTrigger("MoveLeft");
            animator.SetTrigger("MoveUp");
            
        }
        else if (yDir == -1){
            direction = 1;
            //animator.ResetTrigger("MoveUp");
            //animator.ResetTrigger("MoveDown");
            //animator.ResetTrigger("MoveRight");
            //animator.ResetTrigger("MoveLeft");
            animator.SetTrigger("MoveDown");

        }
        else if (xDir == 1)
        {
            direction = 3;
            //animator.ResetTrigger("MoveUp");
            //animator.ResetTrigger("MoveDown");
            //animator.ResetTrigger("MoveRight");
            //animator.ResetTrigger("MoveLeft");
            animator.SetTrigger("MoveRight");

        }
        else if (xDir == -1)
        {
            direction = 4;
            //animator.ResetTrigger("MoveUp");
            //animator.ResetTrigger("MoveDown");
            //animator.ResetTrigger("MoveRight");
            //animator.ResetTrigger("MoveLeft");
            animator.SetTrigger("MoveLeft");

        }


        RaycastHit2D hit;

        //play 1 of 2 sound effects if player is able to move. Breaks up monotony, having such variation
        if (CanMove(xDir, yDir, out hit))
        {
            SoundManager.instance.RandomizeSfx(moveSound1, moveSound2);
            movePoints--;
            LoseATB(ATBCost); // Reduce your ATB
            GameManager.instance.currentUnitPoints = movePoints;
            //HP--;
            //GameManager.instance.singlePlayerMove = false; //i think i need this
        }

        base.AttemptMove<T>(xDir, yDir);

 

        updateText();
        //Since player has spent a move point you should updateText
        //CheckIfGameOver(); Don't know why this is here 
        //player turn will be over once movePoints is 0
        
        GameManager.instance.singlePlayerMove = false;
    }

    //this is how we interact with other objects like food and exit
    //this is a unity API function
    private void OnTriggerEnter2D(Collider2D other) {
        //remeber each of the colliders of the things we care about is isTrigger
        //so now just check the tag of this other
        if (other.tag == "Exit") {
            Invoke("Restart", restartLevelDelay); //this will call the function 1 second after colliding
            GameManager.instance.stopAll = true;
            //GameManager.instance.RestartStuff();
            enabled = false; //player will no longer be enabled
        }
        if (other.tag == "Food") {

            SoundManager.instance.RandomizeSfx(eatSound1, eatSound2);
            gainHP(pointsPerFood);
            other.gameObject.SetActive(false);
        }
        if (other.tag == "Soda")
        {
            SoundManager.instance.RandomizeSfx(drinkSound1, drinkSound2);
            gainHP(pointsPerSoda);
            other.gameObject.SetActive(false);
        }
        if (other.tag == "Collectible")
        {
            other.gameObject.GetComponent<Collectible>().gainCollectible();
        }


    }


    //Won't be using this anymore
    public void updateText() {
        //
       /* if (movePoints < 0) {
            movePoints = 0;
        }
        foodText.text = "HP: " + HP + " | ATB: " + ATB;
        */
    }
    public void updateText(int n, bool adding)
    {//n is how much you are adding or subtracting to the health
     //if adding is true, that means you added it, if adding is false that means you subtracted
        

      /*  if (adding)
       {
            foodText.text = "+ " + n + " HP: " + HP + " | MovePoints: " + movePoints;
        }
        else {
            foodText.text = "- " + n + " HP: " + HP + " | ATB: " + ATB;
        }
       */ 
    }
    //now implementing the abstract function
    //for our player this means interacting with a wall
    protected override void OnCantMove<T>(T component)
    {
        if (component is Player) {
            Player p = component as Player;
            character.CheckIfExecuteSpecial(this,p);
            return;
        }
        if (direction == 1) {
  
            animator.ResetTrigger("MoveDown");
            animator.SetTrigger("AttackDown");

        }
        else if (direction == 2)
        {
            animator.ResetTrigger("MoveUp");
            animator.SetTrigger("AttackUp");
        }
        else if (direction == 3)
        {
            animator.ResetTrigger("MoveRight");
            animator.SetTrigger("AttackRight");
        }
        else if (direction == 4)
        {
            animator.ResetTrigger("MoveLeft");
            animator.SetTrigger("AttackLeft");
        }
        //Change this part to check the type of the component, and based on what type it is, 
        //do a different action
        if (component is Wall) { 
            Wall hitWall = component as Wall; //reminder: as casts something
            
            if (hitWall.DamageWall(attack))
            {
                specialGauge.AddSpecialValue((int)(1.5* specialGainRate));
                tutorial6Helper();
            }
            movePoints -= 2;
            LoseATB((int)(ATBCost * 1.8));
        }
        if (component is Enemy)

        {
            SoundManager.instance.RandomizeSfx(moveSound1, moveSound1); // TODO: Put new sounds here
            Enemy hitEnemy = component as Enemy;
            //Make the enemy lose HP, and if they die, you gain some special
            if (InflictDamage) { 
                if (hitEnemy.LoseHP(attack,direction)) {
                    specialGauge.AddSpecialValue(4*specialGainRate);
                    tutorial6Helper();
                }
            }
            movePoints -= 2;
            LoseATB((int)(ATBCost * 1.8));
            //make another function that you call, that has a big switch statatement with Kali and Winoa and the rest
            character.CheckIfExecuteSpecial(this,hitEnemy);
        }
        if (component is Pot)
        {

            
            Pot hitPot = component as Pot;
            if (hitPot.DamagePot(attack)) {
                specialGauge.AddSpecialValue(specialGainRate);
                tutorial6Helper();
            }
            LoseATB((int)(ATBCost * 1.8));
            movePoints -= 2;
        }
    }

 
    //this will restart the current level. Do this for if you get game over or something.
    private void Restart() {

        Application.LoadLevel(Application.loadedLevel);
    }

    //Once the level is complete go to the next level
  //  private void LoadNextLevel()
  //  {
  //      Application.LoadLevel("Tutorial2");
  //  }



    

    //this is called when enemy attacks the player //this function was moved to unit
    public bool LoseHP(int attack, int attackerDirection) {
        float multiplier = 1.0f;

        if (checkIfBackAttack(attackerDirection, direction))
        {
            multiplier = 1.4f;
        }

        if (direction == 1) {
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

        int damage = ((int)(multiplier * attack) - defense);
        if (damage < 0) {
            damage = 0;
        }
        HP -= damage;
        updateText(attack - defense,false);
        if (HP <= 0) {
            GameManager.instance.stopAll = true;
            GameManager.instance.doingSetup = true;
            HP = 0;
            Debug.Log("Bullshit");
            StartCoroutine("DieAnimation",1);
            dead = true;
            
        }
        if (healthBar) { 
            healthBar.UpdateVitalBar(MaxHP, HP);

        }
        if (name.Equals("Kali"))
        {
            GameManager.instance.LeftUI.GetComponent<VitalsUI>().UpdateKaliHP(MaxHP, HP);
        }
        return dead;
        //CheckIfGameOver();
    }
    private void CheckIfGameOver() {
        if (HP <= 0) {
            SoundManager.instance.PlaySingle(gameOverSound);
            SoundManager.instance.musicSource.Stop();
            GameManager.instance.GameOver();
        }

    }


    protected IEnumerator DieAnimation()
    {
        yield return new WaitForSeconds(1);
        Debug.Log("January");
        GameManager.instance.gameCalculation.actualGrid[y, x].hasEnemy = false;
        GameManager.instance.gameCalculation.actualGrid[y, x].walkable = true;
        GameManager.instance.stopAll = false;
        
    }

    //The following functions will all be responses to the buttons that are pressed, changing up
    //the way that input works from just having all the code in Update
    public void mainstick(int xDir, int yDir)
    {
        
        if (ATB <= 0 || !isActivePlayer ||  !GameManager.instance.singlePlayerMove || !GameManager.instance.playersTurn
            || GameManager.instance.enemiesMoving || GameManager.instance.stopAll || shooting 
            || popUpMenuBeingShown) {
            return;
        }
        //following will prevent diagonal moving
        if (xDir != 0)
        {
            yDir = 0;
        }

        GameManager.instance.UpdateCamera(); //realize with real eyes, why camera won't update
        //for first move, but will update for subsequent moves.
        AttemptMove<MonoBehaviour>(xDir, yDir); //this means that we are expecting
            //I changed the Wall to just MonoBehavior, might not need the abstract function
            //that the player may encounter a wall when moving
            //enemyscript will expect to be interacting with a player
    }
    public void dpad(int xDir, int yDir)
    {
        if (isActivePlayer) {

        
            animator.ResetTrigger("FaceLeft");
            animator.ResetTrigger("FaceRight");
            animator.ResetTrigger("FaceUp");
            animator.ResetTrigger("FaceDown");
            animator.ResetTrigger("MoveLeft");
            animator.ResetTrigger("MoveRight");
            animator.ResetTrigger("MoveUp");
            animator.ResetTrigger("MoveDown");
            animator.ResetTrigger("AttackLeft");
            animator.ResetTrigger("AttackRight");
            animator.ResetTrigger("AttackUp");
            animator.ResetTrigger("AttackDown");
            if (yDir == 1)
            {
                // Debug.Log("what");
            
                if (direction != 2) {
                    direction = 2;
                    animator.SetTrigger("FaceUp");
                }
            }
            else if (yDir == -1)
            {
                if (direction != 1) { 
                    direction = 1;
                    animator.SetTrigger("FaceDown");
                }
            }
            else if (xDir == -1)
            {
                if (direction != 4)
                {
                    direction = 4;
                    animator.SetTrigger("FaceLeft");
                }
            }
            else if (xDir == 1)
            {
                if (direction != 3) {

                    direction = 3;
                    animator.SetTrigger("FaceRight");
                }
            }
        }
    }
    //eventually make this how you move the camera
    public void secondarystick(int xDir, int yDir)
    {

    }
    public void submit()
    {

        
        if (GameManager.instance.messageBeingShown)
        {
            GameManager.instance.hideMessage();
            return;
        }

        if (!isActivePlayer || popUpMenuBeingShown)
        {
            return;
        }
        if (shooting)
        {
            cancel();
            return;
        }
        else if (ATB <= 0 && GameManager.instance.singlePlayerMove) //removed movePoints <= 0, trying to streamline to only ATB
        {
            ATB = -200;
            movePoints = -200;
            character.wasteSpecial(this);
            GameManager.instance.playersTurn = false;
            GameManager.instance.singlePlayerMove = false;
            isActivePlayer = false;
            GameManager.instance.readyToDestroyPlayerTurn = true;
            
        }

    }
    public void doubleSubmit()
    {

    }
    public void cancel()
    {
        if (shooting)
        {
            GameObject gridSelector = GameObject.FindGameObjectWithTag("GridSelector");
            gridSelector.GetComponent<GridSelector>().disappear();
            shooting = false;
            inputCoolDown = true;
            GameManager.instance.UpdateCamera();
        }
        
    }
    public void doubleCancel()
    {

    }

    //tab for keyboard, use to aim and shoot 
    public void rightTrigger()
    {

        if (!isActivePlayer || !GameManager.instance.singlePlayerMove || !GameManager.instance.playersTurn
            || GameManager.instance.enemiesMoving || GameManager.instance.stopAll)
        {
            return;
        }

        if (inputCoolDown)
        {
            return;
        }
        inputCoolDown = true;

        if (!shooting) {
            GameObject gridSelector = GameObject.FindGameObjectWithTag("GridSelector");
            (gridSelector.GetComponent<GridSelector>()).appear();
            gridSelector.transform.position = new Vector3(x, y, 0);
            (gridSelector.GetComponent<GridSelector>()).x = x;
            (gridSelector.GetComponent<GridSelector>()).y = y;
            (gridSelector.GetComponent<GridSelector>()).makeActive();
            shooting = true;
            return;
        }

        else {
            GameObject gridSelector = GameObject.FindGameObjectWithTag("GridSelector");
            int gridX = gridSelector.GetComponent<GridSelector>().x;
            int gridY = gridSelector.GetComponent<GridSelector>().y;

            //this stops the bullet from appearing at the moment that you press the button first time
            if (gridX == x && gridY == y && !name.Equals("Hugo"))
            {
                return;
            }

            animator.ResetTrigger("FaceLeft");
            animator.ResetTrigger("FaceRight");
            animator.ResetTrigger("FaceUp");
            animator.ResetTrigger("FaceDown");

            if (ATB <= 0 || !(character.fireWeapon(this, gridSelector)))
            {
                stopShooting = true;
                cancel(); //uncomment this to make things work i think
                return;
            }
          
            // GameObject bullet = Instantiate(greenBullet, new Vector3(x, y, 0), Quaternion.identity)
            //     as GameObject;
            int xDiff = gridX - x;
            int yDiff = gridY - y;

            if (Math.Abs(xDiff) > Math.Abs(yDiff))
            {

                if (xDiff > 0)
                {
                    setDirection(3);
                }

                else
                {
                    setDirection(4);
                }

            }
            else {
                if (yDiff > 0)
                {
                    setDirection(2);
                }

                else
                {
                    setDirection(1);
                }
            }
        }
    }
    //z for keyboard, use to use special
    public void rightBumper()
    {
        character.StartSpecial(this);
    }

    //not sure what to use this button for yet
    public void leftTrigger()
    {

    }

    //caps lock on computer, use to turn display off
    public void leftBumper()
    {

    }

    public void startButton() {
        if (!isActivePlayer)
        {
            return;
        }
        if (shooting)
        {
            cancel();
            return;
        }

        if (GameManager.instance.singlePlayerMove && !GameManager.instance.stopAll)
        //you need the second of the above conditions to prevent mashed output from messing you
        //up
        {
            character.wasteSpecial(this);
            movePoints = -200;
            LoseATB(400);
            ATB = -200;
            GameManager.instance.playersTurn = false;
            GameManager.instance.singlePlayerMove = false;
            //  GameManager.instance.singlePlayerMove = false;
            //GameManager.instance.enemiesMoving = true;
            //GameManager.instance.currentUnit = null; can't do this here because the loop in
            //GameManager might in the middle of running when it happens, since parallel processes
            isActivePlayer = false;
            //i wanna just make a pause
            // StartCoroutine(GameManager.instance.MoveUnits());
            GameManager.instance.readyToDestroyPlayerTurn = true;
            return;
        }

    }

    public void menu() {
        if (!GameManager.instance.messageBeingShown && isActivePlayer) {
            popUpMenuBeingShown = true;
            GameManager.instance.popUpMenuBeingShown = true;
            GameManager.instance.PopUpMenu.SetActive(true);
            // GameManager.instance.toggleHints();
        }
    }
}
/*
        if (displayVitals)
        {
            healthBar.gameObject.SetActive(false);
        }
        else {
            healthBar.gameObject.SetActive(true);
        }
        */