using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour {

    public void PlayGame()
    {
        SceneManager.LoadScene("Game1");
    }
    
    public void Shopping()
    {
        SceneManager.LoadScene("Shop");
    }
}
