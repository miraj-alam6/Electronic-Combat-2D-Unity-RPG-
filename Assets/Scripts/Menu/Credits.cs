using UnityEngine;
using System.Collections;

public class Credits : MonoBehaviour {
    public GameObject screen0;
    public GameObject screen1;
    public GameObject screen2;
    public GameObject screen3;
    public GameObject screen4;
    public GameObject screen5;
    public GameObject screen6;

    float testVariable = 0.4f;
    // Use this for initialization
    void Start() {
        StartCoroutine(PlayCredits());
    }
    public void showScreenOne() {
        screen0.SetActive(false);
        screen1.SetActive(true);
        Invoke("showScreenTwo", 4 * testVariable);
    }
    public void showScreenTwo()
    {
        screen1.SetActive(false);
        screen2.SetActive(true);
        Invoke("showScreenThree", 10 * testVariable);
    }
    public void showScreenThree()
    {
        screen2.SetActive(false);
        screen3.SetActive(true);
        Invoke("showScreenFour", 12 * testVariable);
    }
    public void showScreenFour()
    {
        screen3.SetActive(false);
        screen4.SetActive(true);
        Invoke("showScreenFive", 14 * testVariable);
    }
    public void showScreenFive() { 
    
        screen4.SetActive(false);
        screen5.SetActive(true);
        Invoke("showScreenSix", 4 * testVariable);
    }
    public void showScreenSix()
    {
        screen5.SetActive(false);
        screen6.SetActive(true);
        Invoke("EndCredits", testVariable);
    }
    public void EndCredits() {
        GameManager.instance.levelNumber = 15;
        GameManager.instance.LoadLevelByNumber(15);
    }
    // Update is called once per frame
    void Update () {
	
	}
    protected IEnumerator PlayCredits() {
        screen0.SetActive(false);
        screen1.SetActive(true);
        yield return new WaitForSeconds(8.0f);
        screen1.SetActive(false);
        screen2.SetActive(true);
        yield return new WaitForSeconds(20.0f);
        screen2.SetActive(false);
        screen6.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        EndCredits();
     
    }
}
