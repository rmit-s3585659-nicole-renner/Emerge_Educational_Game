using UnityEngine;
using System.Collections;

public class Mousehole : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("Player"))
        {
            GameController._gc.QuestionTime(true);
            Debug.Log("Level Complete");
            Time.timeScale = 0;
            }
    }
}
