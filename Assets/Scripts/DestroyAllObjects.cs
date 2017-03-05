using UnityEngine;
using System.Collections;

public class DestroyAllObjects : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D coll)
    {
        Debug.Log("Destroying " + coll.gameObject.name);
        if (coll.GetComponent<Collider2D>().gameObject.layer == LayerMask.NameToLayer("Ground")) {
            GameController._gc.RemovePlatform();
            GameController._gc.CreatePlatform();
            Destroy(coll.gameObject);
        }
        else
        {
            Destroy(coll.gameObject);
            Debug.Log("Game Over");
            GameController._gc.GameOver();
        }
    }
}
