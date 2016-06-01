using UnityEngine;
using System.Collections;
//using System;
using UnityEngine.UI;
//Player will inherit from MovingObject instead of MonoBehavior
public class Player : MovingObject {
    public int wallDamage = 1; //amount of damage that player inflicts onto the wall when chopping
    public int pointsPerFood = 10;
    public int pointsPerSoda = 20;
    //both of the above have values that will be the value added to player's score when consumed.
    public float restartLevelDelay = 1f;
    public Text foodText;

    private Animator animator;
    private int food; //stores score during level before passing it back to game manager.

    //Sound effects for the player
    public AudioClip moveSound1;
    public AudioClip moveSound2;
    public AudioClip eatSound1;
    public AudioClip eatSound2;
    public AudioClip drinkSound1;
    public AudioClip drinkSound2;
    public AudioClip gameOverSound;

    public int debuggingVariable = 0;
    // Use this for initialization
    protected override void Start() {
        animator = GetComponent<Animator>();
        food = GameManager.instance.playerFoodPoints; //player will manage this during the level,
                                                      //and then it will be stored in game manager again when we change levels.
                                                      //base is how we refer to our superclass. base.Start() is calling the Start function of
                                                      //MovingObject since that is our superclass      
        foodText.text = "Food: "+ food;
        
        base.Start();

    }

    //when the player is disabled you want to store the score into the GameManager
    private void OnDisable() {
        GameManager.instance.playerFoodPoints = food;
    }
    // Update is called once per frame
    //we're gonna check if it's player turn or not
    void Update() {
        if (!GameManager.instance.playersTurn)
        {
            return;
        }
        //now if it is player turn, then...
        //gonna use the following to store the direction we're moving
        //along an axis as either 1 or -1.
        int horizontal = 0;
        int vertical = 0;

        horizontal = (int)Input.GetAxisRaw("Horizontal");
        vertical = (int)Input.GetAxisRaw("Vertical");

        //following will prevent diagonal moving
        if (horizontal != 0) {
            vertical = 0;
        }

        //now process the movement if there is one
        if (horizontal != 0 || vertical != 0) {
            AttemptMove<Wall>(horizontal, vertical); //this means that we are expecting
            //that the player may encounter a wall when moving
            //enemyscript will expect to be interacting with a player

        }
    }

    //Having <T> there, but then having no T is your parameters means that
    //you will encounter the generic within the body of your function rather than
    //your parameters
    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        debuggingVariable = yDir;
        if (yDir == 1) {
            //need to reset all other triggers to prevent triggers from staying on instead of
            //automatically turning off
            //animator.ResetTrigger("MoveDown");
            //animator.ResetTrigger("MoveUp");
            animator.SetTrigger("MoveUp");
            
        }
        else if (yDir == -1){
            //animator.ResetTrigger("MoveUp");
            //animator.ResetTrigger("MoveDown");
            animator.SetTrigger("MoveDown");

        }
        food--;
        foodText.text = "Food: " + food;
        base.AttemptMove<T>(xDir, yDir);

        RaycastHit2D hit;
        //play 1 of 2 sound effects if player is able to move. Breaks up monotony, having such variation
        if (Move(xDir, yDir, out hit)) {
            SoundManager.instance.RandomizeSfx(moveSound1, moveSound2);
        }
        //Since player has lost food from moving, we should check if game is over
        CheckIfGameOver();
        //player turn is now over
        GameManager.instance.playersTurn = false;
    }

    //this is how we interact with other objects like food and exit
    //this is a unity API function
    private void OnTriggerEnter2D(Collider2D other) {
        //remeber each of the colliders of the things we care about is isTrigger
        //so now just check the tag of this other
        if (other.tag == "Exit") {
            Invoke("Restart", restartLevelDelay); //this will call the function 1 second after colliding
            enabled = false; //player will no longer be enabled
        }
        if (other.tag == "Food") {

            SoundManager.instance.RandomizeSfx(eatSound1, eatSound2);
            food += pointsPerFood;
            foodText.text = "+ " + pointsPerFood + " Food: " + food;
            other.gameObject.SetActive(false);
        }
        if (other.tag == "Soda")
        {
            SoundManager.instance.RandomizeSfx(drinkSound1, drinkSound2);
            food += pointsPerSoda;
            foodText.text = "+ " + pointsPerSoda + " Food: " + food;
            other.gameObject.SetActive(false);
        }



    }

    //now implementing the abstract function
    //for our player this means interacting with a wall
    protected override void OnCantMove<T>(T component)
    {
        Wall hitWall = component as Wall; //reminder: as casts something
        hitWall.DamageWall(wallDamage);
        //now trigger the animation
        animator.SetTrigger("playerChop");
    }

    //this is called if we reach Exit
    private void Restart() {
        //we are just gonna reload our same scene, since the level itself will be
        //procedurally generated differently each time
        Application.LoadLevel(Application.loadedLevel);
    }

    //this is called when enemy attacks the player
    public void LoseFood(int loss) {
        animator.SetTrigger("playerHit");
        food -= loss;
        foodText.text = "-" + loss + " Food: " + food;
        CheckIfGameOver();
    }
    private void CheckIfGameOver() {
        if (food <= 0) {
            SoundManager.instance.PlaySingle(gameOverSound);
            SoundManager.instance.musicSource.Stop();
            GameManager.instance.GameOver();
        }

    }
}
