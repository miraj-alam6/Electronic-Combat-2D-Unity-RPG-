using UnityEngine;
using System.Collections;

public class GridSelector : MonoBehaviour {
    public int x;
    public int y;
    public string message = "What";
    public string shortmessage = "Nothing";
    private BoxCollider2D boxCollider;
    private Rigidbody2D rb2D;
    private float inverseMoveTime; //makes movement calculation more efficient
    public float moveTime = 0.1f;
    private bool isActive = false;
    private bool noSelector = true; // this is true when there is no selector on the screen.
    //This is different from the built in Unity quality of being active. Active in this class just
    //means that it is able to move around.
    public void makeActive() {
        isActive = true;
    }


    //This is will make the Grid disappear after completing its movement.
    public void disappear()
    {
        noSelector = true;
    }

    public void appear()
    {
        noSelector = false;
    }
    // Use this for initialization
    void Start() {
        x = (int)GetComponent<Transform>().position.x;
        y = (int)GetComponent<Transform>().position.y;
        boxCollider = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
        inverseMoveTime = 1f / moveTime; //we do this so we can use multiplaction instead of division later
                                         //on which is more efficient.
    }

    // Update is called once per frame
    void Update() {
        if (isActive){
            if (noSelector) {
                isActive = false;
                GetComponent<Transform>().position = new Vector2(-100, -100);
                return;
            }
            int horizontal = 0;
            int vertical = 0;
            
            horizontal = (int)Input.GetAxisRaw("Horizontal");
            vertical = (int)Input.GetAxisRaw("Vertical");

            if (horizontal == 0 && vertical == 0)
            {
                return;
            }
            //following will prevent diagonal moving
            if (horizontal != 0)
            {
                vertical = 0;
            }

            //Debug.Log("AKSAR" + horizontal + "," + vertical);
            isActive = false;
            Move(horizontal,vertical);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player")
        {
            Player player = other.gameObject.GetComponent<Player>();
            shortmessage = "player";
            message = "" + player.name + " HP:" + player.HP + "\n"
            + "ATG Rate: " + player.minSpeed + "-" + player.maxSpeed + "\n"
            + "ATG Cost:" + player.ATBCost + "\n"
            + "ATK: " + player.attack + " DEF:" + player.defense;
            Debug.Log("Player");
        }
        else if (other.tag == "Bullet")
        {

            Debug.Log("Bullet");
        }
        else if (other.tag == "Collectible")
        {
            Debug.Log(other.gameObject.GetComponent<Collectible>().name);
        }
        else if (other.tag == "GridSelector")
        {
            Debug.Log("this shouldn't be possible");
        }
        else if (other.tag == "Wall")
        {
            Wall wall = other.GetComponent<Wall>();
            message = "Wall HP:" + wall.hp;
            shortmessage = "wall";
            Debug.Log("Wall with HP: " + other.gameObject.GetComponent<Wall>().hp);
        }
        else if (other.tag == "Pot")
        {
            Debug.Log("Pot with HP: " + other.gameObject.GetComponent<Pot>().hp);
        }

        else if (other.tag == "Enemy")
        {
            Enemy enemy = other.GetComponent<Enemy>();
            shortmessage = "enemy";
            Debug.Log("Enemy with HP: " + other.gameObject.GetComponent<Enemy>().HP);
            message = "" + enemy.name + " HP:" + enemy.HP + "\n"
            + "ATG Rate: " + enemy.minSpeed + "-" + enemy.maxSpeed + "\n"
            + "ATG Cost:" + enemy.ATBCost + "\n"
            + "ATK: " + enemy.attack + " DEF:" + enemy.defense;
            Debug.Log("Player");
        }
        
    }
    public bool Move(int xDir, int yDir) 
    {
        message = "Nothing here";
        shortmessage = "none";
        Vector3 start = transform.position; //implicitly discards transform.position's z 
        //axis thus converting a vector3 to a vector2 without need for an explicit cast
        Vector3 end = start + new Vector3(xDir, yDir,0); 
        //Debug.Log(start.x + "," + start.y + "," + end.x + "," + end.y);
        //Not gonna use raycasting
        //disable own boxCollider so that we can cast our ray without hitting ourself
        // boxCollider.enabled = false; //SUPER IMPORTANT: make sure you turn it back on before function
        //is over


        //hit = Physics2D.Linecast(start, end, blockingLayer);
        //if hit.transform is null that means we didn't hit anything which means we can move into
        //the spot.
        // boxCollider.enabled = true; // SUPER IMPORTANT BUG, I forgot this line of code, it is
        //needed to be able to collide with stuff again
        if (true)
        {
            x = x + xDir;
            y = y + yDir;
            if (x < 0 || y < 0 || x >= GameManager.instance.gameCalculation.columns || y >= GameManager.instance.gameCalculation.rows) {
                x = x - xDir;
                y = y - yDir;
                isActive = true;
                return false;
            }
            //Debug.Log("End:" + xDir + "," + yDir);
            StartCoroutine(SmoothMovement(end));
            return true; //this means we were able to move.
        }
        return false;

    }

    public IEnumerator SmoothMovement(Vector3 end)
    {
        
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude; //sqrmagnitude is being
        //used because it is computationally cheaper than magnitude.
        while (sqrRemainingDistance > float.Epsilon)
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
        isActive = true;
    }
}
