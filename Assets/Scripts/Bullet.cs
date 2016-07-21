using UnityEngine;
using System.Collections;

//The bullet class will have an int that represents how many bullets
public class Bullet : MonoBehaviour
{
    public int x;
    public int y;
    public int bulletDamage = 10;
    public int numOfBullets;
    public int tempDestX = 4;
    public int tempDestY = 4;
    public bool destinationSet = false;
    private BoxCollider2D boxCollider;
    private Rigidbody2D rb2D;
    private float inverseMoveTime; //makes movement calculation more efficient
    public float movePerUnit = 0.02f; //how much time it takes to move one unit
    public float moveTime = 0f; //this is a 
    bool isMoving = false;
    bool bulletBurst = false;
    public float burstTime = 0.1f;
    private Animator animator;

    // Use this for initialization
    void Start()
    {
        x = (int)GetComponent<Transform>().position.x;
        y = (int)GetComponent<Transform>().position.y;
        boxCollider = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        inverseMoveTime = 1f / moveTime;
        //Move(4, 4);
    }
    public void Glue() {
        Debug.Log("Shit");
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.tag == "Player")
        {
            if (other.GetComponent<Player>())
            {
                if (other.GetComponent<Player>().isActivePlayer)
                {
                    return;
                }
                other.GetComponent<Player>().LoseHP(bulletDamage);
            }
        }
        else if (other.tag == "Bullet") {
            return;
        }
        else if (other.tag == "Collectible")
        {
            return;
        }
        else if (other.tag == "GridSelector")
        {
            return;
        }
        else if (other.tag == "Wall")
        {
            if (other.GetComponent<Wall>())
            {
                other.GetComponent<Wall>().DamageWall(500);
            }
        }
        else if (other.tag == "Pot")
        {
            if (other.GetComponent<Pot>())
            {
                other.GetComponent<Pot>().DamagePot(bulletDamage);
            }
        }

        else if (other.tag == "Enemy")
        {
            if (other.GetComponent<Enemy>())
            {
                other.GetComponent<Enemy>().LoseHP(bulletDamage);
            }
        }
        bulletBurst = true;
        animator.SetTrigger("Burst");
        Invoke("bulletDie", burstTime);
    }


    // Update is called once per frame
    void Update()
    {
        if (destinationSet)
       {
            Move(tempDestX, tempDestY);
            destinationSet = false; //do this so that the animator can normally do triggers again
       }
    }

    //Bullet will move based on a destination X and destination Y
    public void Move(int destX, int destY)
    {
        //I want to use smooth movement and calculate
        int diffX = destX - x;
        int diffY = destY - y;
        if (diffX != 0 && diffY != 0)
        {
            if (diffX > 0 && diffY > 0)
            {
                animator.SetTrigger("UpRight");
            }
            else if (diffX > 0 && diffY < 0)
            {
                animator.SetTrigger("DownRight");
            }
            else if (diffX < 0 && diffY > 0)
            {
                animator.SetTrigger("UpLeft");
            }
            else
            {
                animator.SetTrigger("DownLeft");
            }
        }
        else {
            if (diffX == 0)
            {
                if (diffY > 0)
                {
                    animator.SetTrigger("Up");
                }
                else
                {
                    animator.SetTrigger("Down");
                }
            }
            else {
                if (diffX > 0)
                {
                    animator.SetTrigger("Right");
                }
                else
                {
                    animator.SetTrigger("Left");
                }
            }
        }
        moveTime = Mathf.Sqrt(diffX * diffX + diffY * diffY) * movePerUnit;
        float incrementMoveTime = moveTime;
        int xBound = GameManager.instance.gameCalculation.columns;
        int yBound = GameManager.instance.gameCalculation.rows;
        while (destX < xBound && destY < yBound && destX >= 0 && destY >= 0)
        {
            moveTime += incrementMoveTime;
            destX += diffX;
            destY += diffY;
        }
        Vector3 end = new Vector3(destX, destY, 0);
        StartCoroutine(SmoothMovement(end));
    }
    public IEnumerator SmoothMovement(Vector3 end)
    {

        float sqrRemainingDistance = (transform.position - end).sqrMagnitude; //sqrmagnitude is being
        //used because it is computationally cheaper than magnitude.
        while (!bulletBurst && sqrRemainingDistance > float.Epsilon)
        { //float.Epsilon is super small, almost 0
            //move source point towards destination point
            Vector3 newPosition = Vector3.MoveTowards(rb2D.position, end, inverseMoveTime * Time.deltaTime);
            //this is proportionally closer to the end now
            //now Move the thing to the new position that you found
            rb2D.MovePosition(newPosition);
            //now reupdate sqrRemainingDistance so that we don't infinitely loop
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;
            yield return null; //this means wait for a frame before reevaluating this loop
        }
        bulletBurst = true;
    }
    public void bulletDie()
    {
        Destroy(this.gameObject);
    }
}
