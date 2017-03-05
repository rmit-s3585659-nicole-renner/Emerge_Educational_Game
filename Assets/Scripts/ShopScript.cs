using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ShopScript : MonoBehaviour {

    public GameObject ShopPanel;
    public Text cheeseCountText;

    public GameObject productButton;

    private Dictionary<string, Item> products;
    private Dictionary<Item, GameObject> buttons;

    void Awake()
    {
        products = new Dictionary<string, Item>();
        buttons = new Dictionary<Item, GameObject>();
        products["Slower Falls"] = new Item(30,1,0);
        products["Stronger Feet"] = new Item(19,0,-1);
    }

    // Use this for initialization
    void Start()
    {
        cheeseCountText.text = "Cheese Count: " + SaveLoad.savedCollectibles.ToString();

        foreach (string product in products.Keys)
        {
            GameObject button = (GameObject)Instantiate(productButton, ShopPanel.transform);
            button.GetComponentInChildren<Text>().text = product + "\n" + products[product].cost;
            button.GetComponent<Button>().onClick.AddListener(() => Buy(products[product]));
            Item i = products[product];
            buttons[i] = button;

            if (SaveLoad.savedCollectibles < i.cost)
            {
                Debug.Log("Too expensive");
                button.GetComponent<Button>().interactable = false;
            }
        }
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    void Buy(Item obj)
    {
        SaveLoad.savedCollectibles = SaveLoad.savedCollectibles - obj.cost;
        SaveLoad.Save();
        buttons[obj].GetComponent<Button>().interactable = false;
    }
}

public class Item {

    public int cost;
    int platformFallDelay;
    int gravity;

    public Item(int c, int fall, int grav)
    {
        cost = c;
        platformFallDelay = fall;
        gravity = grav;
    }

}
