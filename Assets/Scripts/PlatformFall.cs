using UnityEngine;
using System.Collections;

public class PlatformFall : MonoBehaviour {

    float fallDelay = 4f;

    private Rigidbody2D rb2d;

    bool finalPlatform = false;
    bool notAtEndGoal = true;

    // Use this for initialization
    void Start () {
        rb2d = GetComponent<Rigidbody2D>();

	}

    public void SetAsFinalPlatform()
    {
        finalPlatform = true;
    }
	
    void Fall()
    {
        rb2d.isKinematic = false;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (finalPlatform == false && coll.gameObject.CompareTag("Player"))
        {
            Invoke("Fall", fallDelay);
        }
        else if (finalPlatform == true && coll.gameObject.CompareTag("Player"))
        {
            StartCoroutine(GoToMouseHole(this.gameObject));
        }
    }

    IEnumerator GoToMouseHole(GameObject obj)
    {
        while (notAtEndGoal == true)
        {
            float distThisFrame = 0.01f/Time.deltaTime;
            Debug.Log("dist: " + distThisFrame);

            obj.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y - distThisFrame, 0);
            Debug.Log(obj.transform.position.y);
            if (obj.transform.position.y <= -30) {
                notAtEndGoal = false; }
            yield return new WaitForSeconds(2);
        }
        yield return null;
    }
}
