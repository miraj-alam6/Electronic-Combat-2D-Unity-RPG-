using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public float cameraX = 3.0f;
    public float cameraY = 2.9f;
    public float offsetX = 3.0f;
    public float offsetY = 2.9f;
    public float goalX;
    public float goalY;
    public float moveTime;
    public float inverseMoveTime;
    public bool testingChumma;
    public bool isActive = false;
    
    public Rigidbody2D rb2D;
    // Use this for initialization
	void Start () {
        testingChumma = false;

        rb2D = GetComponent<Rigidbody2D>();
        inverseMoveTime = 1f / moveTime;
        //Move();
    }

    void UpdateCameraGoal(int x, int y) {
        goalX = x + offsetX;
        goalY = y + offsetY;

    }
	// Update is called once per frame
	void Update () {
        if(testingChumma){
            Move(2,2);
        }
	}

    public bool Move(int x, int y)
    {
        UpdateCameraGoal(x,y);
        Vector3 end = new Vector3(goalX, goalY, transform.position.z); //this was a big error, making it 0, made an infinite thing that didn't work, so making it -10 like the camera
        testingChumma = false;
        StartCoroutine(SmoothMovement(end));
        //cameraX = goalX;
        //cameraY = goalY;
       
        return true; //this means we were able to move.
        
    }

    public IEnumerator SmoothMovement(Vector3 end)
    {
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude; //sqrmagnitude is being
        //used because it is computationally cheaper than magnitude.
        while (sqrRemainingDistance > 0.002)
        { //float.Epsilon is super small, almost 0
            //move source point towards destination point
  
            transform.position = Vector3.Lerp(transform.position, end, inverseMoveTime * Time.fixedDeltaTime);
            //now reupdate sqrRemainingDistance so that we don't infinitely loop
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;
            //Debug.Log(sqrRemainingDistance);
            yield return null; //this means wait for a frame before reevaluating this loop

        }
        transform.position = end;

        isActive = true;
    }
}
