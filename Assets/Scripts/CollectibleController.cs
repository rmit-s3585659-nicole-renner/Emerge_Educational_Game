using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CollectibleController : MonoBehaviour {

    public static CollectibleController _cc;

    public GameObject collectible;

//    public int collectibleCount = 0;
    public Text collectibleCountText;

    // Use this for initialization
    void Awake () {
        if (_cc == null)
        {
            _cc = this;
        }	
        else
        {
            Destroy(this.gameObject);
        }
	}

    public int GetCollectibleAmount()
    {
        return SaveLoad.savedCollectibles;
    }

    public void UpdateCollectibleAmount(int x)
    {
        SaveLoad.savedCollectibles += x;
    }

    public void ReturnCollectiblesToZero()
    {
        SaveLoad.savedCollectibles = 0;
    }

    public void CreateCollectible(Vector2 platformPosition)
    {
        int numberofCollectibles = Random.Range(0, 4);
        for (int collectibleNo = 0; collectibleNo <= numberofCollectibles; collectibleNo++)
        {
            Vector2 collectiblePosition = platformPosition + new Vector2(Random.Range(-2.5f, 3f), 4f);
            GameObject collect = (GameObject) Instantiate(collectible, collectiblePosition, Quaternion.identity);
            collect.transform.parent = this.gameObject.transform;
        }
        UpdateCountText();
    }

    public void UpdateCountText()
    {
        collectibleCountText.text = "Count: " + GetCollectibleAmount();
    }

    public void HitCollectible()
    {
        UpdateCollectibleAmount(1);
        UpdateCountText();
        Debug.Log("Collectibles: " + (GetCollectibleAmount() % 20));
        if ((GetCollectibleAmount() % 2) == 0)
        {
  //          GameController._gc.QuestionTime();
        }
    }
}
