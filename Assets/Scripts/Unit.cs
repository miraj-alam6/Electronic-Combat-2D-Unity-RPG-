using UnityEngine;

using System.Collections;

public abstract class Unit : MonoBehaviour { //abstract because two both player and enemy will inherit
                                             //absract makes the class incomplete and will have to be completed by the child class
    public string name;
    public int MaxHP;
    public int HP;
    public int attack;
    public bool InflictDamage = true;
    public int defense;
    public bool dead = false;
    public int speed;
    public bool weInTutorialLevel6 = false;
    public float moveTime = 0.1f; //this is the time it takes for the object to move in seconds.
    public int movePoints;
    public int maxMovePoints;
    public int minSpeed; // This is how many points are added to ATB bar
    public int maxSpeed; // This is upper limit. A random value will be chosen each time to add to the ATB bar
    public int ATB = 0; //Once this exceeds 100 this unit will be able to move
    public int ATBCost = 10; //The higher this value, the quicker your ATB drains as you do moves
    public int direction = 1; // 1 is for down, 2 is for up, 3 for right, 4 is for left
    [HideInInspector]public bool isPlayer;
    public LayerMask blockingLayer; //this is the layer where we check collision to see if space open
    // Use this for initialization
    private BoxCollider2D boxCollider;
    private Rigidbody2D rb2D; //stores the rigidbody2d component of the unit we're moving
    private float inverseMoveTime; //makes movement calculation more efficient
    public int x;
    public int y;
    public int previousX;
    public int previousY;
    public bool displayVitals = false; //set true in inspector if you put a health bar
    public VitalBar healthBar;
    public VitalBar ATBBar;
    public GameObject VitalsWrapper;
    public SpecialGauge specialGauge;
    public SpriteRenderer spriteRenderer;
    public Color originalColor;
    public int specialGainRate;
    public Material originalMaterial;
    public Animator animator;
    //flashMaterial is set in inspector, it is the font material
    public Material flashMaterial;

    protected virtual void Start () { //protected virtual can be overridden by inherting classes
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        originalMaterial = spriteRenderer.material;
        animator = GetComponent<Animator>();
        x = (int)GetComponent<Transform>().position.x;
        y = (int)GetComponent<Transform>().position.y;
        previousX = x;
        previousY = y;
        HP = MaxHP;
        movePoints = maxMovePoints;   
        boxCollider = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
        inverseMoveTime = 1f / moveTime; //we do this so we can use multiplaction instead of division later
                                         //on which is more efficient.
        if (GetComponentInChildren<VitalIdentifier>().gameObject) { 
            VitalsWrapper = GetComponentInChildren<VitalIdentifier>().gameObject;
        }
        if ((GetComponentInChildren<SpecialGauge>().gameObject)) {
            specialGauge = GetComponentInChildren<SpecialGauge>();
        }
    }

    public void Update() {
        
    }

    public bool checkIfBackAttack(int attackerDirection, int victimDirection) {
        return (attackerDirection == victimDirection);
    }
    public void flashWhite(float time) {
        spriteRenderer.material = flashMaterial;
        spriteRenderer.color = Color.white;
        Invoke("becomeNormal",time);
    }
    public void turnRed()
    {
       // spriteRenderer.material = flashMaterial;
        spriteRenderer.color = Color.red;
       // Invoke("becomeNormal", time);
    }

    public void turnBlue()
    {
        spriteRenderer.color = Color.blue;
    }
    public void turnCyan()
    {
        spriteRenderer.color = Color.cyan;
    }
    public void turnGreen() {
        spriteRenderer.color = Color.green;
    }
    public void turnYellow()
    {
        spriteRenderer.color = Color.yellow;
    }
    public void becomeNormal() {
        spriteRenderer.material = originalMaterial;
        spriteRenderer.color = originalColor;
    }
    //Move using XDir and yDir.
    protected bool Move(int xDir, int yDir, out RaycastHit2D hit) //out keyword causes it to be passed by reference instead of value
        //in this case we're using out to return two things, the bool and the
        //RaycastHit2D as well since it is pass by reference
    {
       
        Vector2 start = transform.position; //implicitly discards transform.position's z 
        //axis thus converting a vector3 to a vector2 without need for an explicit cast
        Vector2 end = start + new Vector2(xDir,yDir);
        //disable own boxCollider so that we can cast our ray without hitting ourself
        boxCollider.enabled = false; //SUPER IMPORTANT: make sure you turn it back on before function
        //is over


        hit = Physics2D.Linecast(start,end,blockingLayer);
        //if hit.transform is null that means we didn't hit anything which means we can move into
        //the spot.
        boxCollider.enabled = true; // SUPER IMPORTANT BUG, I forgot this line of code, it is
        //needed to be able to collide with stuff again
        if (hit.transform == null) {
            //Debug.Log(xDir);
            if (isPlayer)
            {
                GameManager.instance.gameCalculation.actualGrid[y, x].hasPlayer = false;
                GameManager.instance.gameCalculation.actualGrid[y, x].walkable = true; 
               
            }
            else
            {
                GameManager.instance.gameCalculation.actualGrid[y, x].hasEnemy = false;
                GameManager.instance.gameCalculation.actualGrid[y, x].walkable = true;
                if (((Enemy)this).movesToDo != null && ((Enemy)this).movesToDo.Count > 0) { 
                ((Enemy)this).movesToDo.RemoveAt(0);
                }
            }
            x = x + xDir;
            y = y + yDir;
            
            if (isPlayer) {
                GameManager.instance.gameCalculation.actualGrid[y, x].hasPlayer = true;
                GameManager.instance.gameCalculation.actualGrid[y, x].walkable = false;
                //Debug.Log("giddy up");
            }
            else
            {
                GameManager.instance.gameCalculation.actualGrid[y, x].hasEnemy = true;
                GameManager.instance.gameCalculation.actualGrid[y, x].walkable = false;
            }
            StartCoroutine(SmoothMovement(end));
            return true; //this means we were able to move.
        }
        return false;

    }

    protected bool CanMove(int xDir, int yDir, out RaycastHit2D hit) //out keyword causes it to be passed by reference instead of value
                                                                  //in this case we're using out to return two things, the bool and the
                                                                  //RaycastHit2D as well since it is pass by reference
    {
        Vector2 start = transform.position; //implicitly discards transform.position's z 
        //axis thus converting a vector3 to a vector2 without need for an explicit cast
        Vector2 end = start + new Vector2(xDir, yDir);
        //disable own boxCollider so that we can cast our ray without hitting ourself
        boxCollider.enabled = false; //SUPER IMPORTANT: make sure you turn it back on before function
        //is over


        hit = Physics2D.Linecast(start, end, blockingLayer);
        //if hit.transform is null that means we didn't hit anything which means we can move into
        //the spot.
        boxCollider.enabled = true; // SUPER IMPORTANT BUG, I forgot this line of code, it is
        //needed to be able to collide with stuff again
        if (hit.transform == null)
        {
           
            return true; //this means we were able to move.
        }
        return false;

    }
    //this coroutine moves a unit from one space to the next
    //parameter end specifies where to move to
    protected IEnumerator SmoothMovement(Vector3 end) {
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude; //sqrmagnitude is being
        //used because it is computationally cheaper than magnitude.
        while (sqrRemainingDistance > float.Epsilon) { //float.Epsilon is super small, almost 0
            //move source point towards destination point
            Vector3 newPosition = Vector3.MoveTowards(rb2D.position, end, inverseMoveTime * Time.deltaTime);
            //this is proportionally closer to the end now
            //now Move the thing to the new position that you found
            rb2D.MovePosition(newPosition);
            //now reupdate sqrRemainingDistance so that we don't infinitely loop
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;
            yield return null; //this means wait for a frame before reevaluating this loop
        }
        
    }
    
    //will have a generic field
    //T specifies the type of component we expect unit to interact with if blocked.
    //in case of enemies this will be the player, and in the case of the player it will be the walls
    protected virtual void AttemptMove<T>(int xDir, int yDir) 
        where T :Component
    {
        RaycastHit2D hit;
        bool canMove = Move(xDir, yDir, out hit);
        //hit is an out parameter, so its value is returned by the function essentially 
        //where it already is
        if (hit.transform == null) { 
            return; //thus if we didn't hit anything we won't do the following code.
        }
        
        T hitComponent = hit.transform.GetComponent<T>();
        if (!canMove && hitComponent != null) //second check means we can interact with hitComponent
        {
            //this abstract function will operate different for each child, and is not implement
            //in this class
            OnCantMove(hitComponent);
        }
    }
    public void tutorial6Helper()
    {

        if (weInTutorialLevel6)
        {
            if (name.Equals("Kali") && specialGauge.GetSpecialValue() >= 80)
            {
                ((TutorialLevel6)GameManager.instance.currentLevel).kaliHasEnough = true;
                GameManager.instance.currentLevel.turnBehavior();
            }
            if (name.Equals("Winoa") && specialGauge.GetSpecialValue() >= 75)
            {
                ((TutorialLevel6)GameManager.instance.currentLevel).winoaHasEnough = true;
                GameManager.instance.currentLevel.turnBehavior();
            }
        }
       
    }

    public void gainHP(int gain)
    {
        HP += gain;
        if (HP > MaxHP)
        {
            HP = MaxHP;
        }
        if (healthBar)
        {
            healthBar.UpdateVitalBar(MaxHP, HP);
        }
        
        if (name.Equals("Kali"))
        {
            GameManager.instance.LeftUI.GetComponent<VitalsUI>().UpdateKaliHP(MaxHP, HP);
        }
    }

    //Fill up ATB gauge with your speed
    // NOTE: once ATB > 100 you get a baseline special boost
    public void applySpeed() {
        if (ATB <= 0) {
            ATB = 0;
        }
        ATB += Random.Range(minSpeed, maxSpeed);
        if (ATB >= 100)
        {
            GameManager.instance.RefreshMessage();
            specialGauge.AddSpecialValue(specialGainRate);
            ATB = 100;
            ATBBar.BecomeGreen();
        }
        if (ATBBar) { 
            ATBBar.UpdateVitalBar(100, ATB);
        }
        if (name.Equals("Kali"))
        {
            GameManager.instance.LeftUI.GetComponent<VitalsUI>().UpdateKaliATB(100, ATB);
        }
    }

    public void SetATB(int val)
    {
        ATB = val;
        if (ATB <= 0)
        {
            ATB = 0;
            //wrap next line in safety
            ATBBar.BecomeBlue();

        }
        if (ATB >= 100)
        {
            ATB = 100;
            //wrap next line in safety
            ATBBar.BecomeGreen();

        }
        if (ATBBar)
        {
            ATBBar.UpdateVitalBar(100, ATB);
            if (name.Equals("Kali"))
            {
                GameManager.instance.LeftUI.GetComponent<VitalsUI>().UpdateKaliATB(100, ATB);
            }
        }


    }

    public void LoseATB(int val)
    {
        ATB -= val;
        if (ATB <= 0)
        {
            ATB = 0;
            ATBBar.BecomeBlue();
            
        }
        if (ATBBar) { 
            ATBBar.UpdateVitalBar(100,ATB);
            if (name.Equals("Kali"))
            {
                GameManager.instance.LeftUI.GetComponent<VitalsUI>().UpdateKaliATB(100, ATB);
            }
        }


    }

    public void setDirection(int direction)
    {
        this.direction = direction;
        if (direction == 1)
        {
            animator.SetTrigger("FaceDown");
        }
        else if (direction == 2)
        {
            animator.SetTrigger("FaceUp");
        }
        else if (direction == 3)
        {
            animator.SetTrigger("FaceRight");
        }
        else if (direction == 4)
        {
            animator.SetTrigger("FaceLeft");
        }
    }
    //now the abstract function that was invoked in earlier code, 
    //something that isn't implemented here in the parent class
    //takes a generic parameter T, and a parameter of type T called component
    //this function must be completed by the inherited class
    //because this is an abstract function it has no opening or closing brackets. Will be implemented
    //by the child class.
    protected abstract void OnCantMove<T>(T component)
        where T : Component;

}
