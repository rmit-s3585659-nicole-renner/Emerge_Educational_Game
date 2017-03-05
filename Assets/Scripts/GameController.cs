using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public static GameController _gc;

    public GameObject player;

    public List<Sprite> shelvesSprites;

    public GameObject QuestionPanel;
    public Text questionText;
    public GameObject ButtonPanel;
    public Button genericButton;

    public Text AnnouncementText;


    public GameObject platform;
    public GameObject platformContainer;
    public int totalPlatforms;
    public int currentPlatforms;
    public float minHorizontalDistance;
    public float maxHorizontalDistance;
    public float minVerticalDistance;
    public float maxVerticalDistance;

    private Vector2 lastPlatformPosition;

    private List<QuestionObject> questionList;

    private CharacterControllerScript playerCharacterControllerScript;

    private bool endOfGame = false;

    // Use this for initialization
    void Start()
    {
        if (_gc == null)
        {
            _gc = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        questionList = new List<QuestionObject>();
        loadQuestions();
        currentPlatforms = 0;
        lastPlatformPosition = transform.position;
        playerCharacterControllerScript = player.GetComponent<CharacterControllerScript>();
        Setup();

    }

    public void Setup()
    {
        Time.timeScale = 1;
        ClearPlatforms();
        lastPlatformPosition = new Vector3(0,0,0);
        GameObject obj = (GameObject)Instantiate(platform, new Vector2(0,0), Quaternion.identity);
        obj.GetComponent<SpriteRenderer>().sprite = shelvesSprites[Random.Range(0, shelvesSprites.Count - 1)];
        currentPlatforms++;
        obj.transform.parent = platformContainer.transform;

        CreatePlatform();
    }

    void loadQuestions()
    {
        string question = "Are you happy?";
        List<string> noanswers = new List<string>();
        List<string> yesanswers = new List<string>();
        yesanswers.Add("Yes");
        yesanswers.Add("No");
        QuestionObject quest1 = new QuestionObject(question, yesanswers, noanswers);
        questionList.Add(quest1);

        question = "School is important. Did you go today?";
        noanswers = new List<string>();
        noanswers.Add("No");
        yesanswers = new List<string>();
        yesanswers.Add("Yes");
        quest1 = new QuestionObject(question, yesanswers, noanswers);
        questionList.Add(quest1);

        question = "Goals are important. Did you meet your goal for today?";
        noanswers = new List<string>();
        noanswers.Add("No");
        yesanswers = new List<string>();
        yesanswers.Add("Yes");
        quest1 = new QuestionObject(question, yesanswers, noanswers);
        questionList.Add(quest1);

        question = "Friends and family can help us. How many can you name?";
        noanswers = new List<string>();
        noanswers.Add("0");
        yesanswers = new List<string>();
        yesanswers.Add("1-2");
        yesanswers.Add("2-5");
        yesanswers.Add("More than 5");
        quest1 = new QuestionObject(question, yesanswers, noanswers);
        questionList.Add(quest1);

    }

    public void CreatePlatform()
    {
        if (endOfGame == false)
        {
            for (int i = currentPlatforms; i < totalPlatforms; i++)
            {
                int upwards = Random.Range(0, 2);
                float verticalDistance;
                if (upwards == 1)
                {
                    verticalDistance = Random.Range(2, maxVerticalDistance);
                }
                else
                {
                    verticalDistance = Random.Range(-2, -maxVerticalDistance);
                }
                float horizontalDistance = Random.Range(minHorizontalDistance, maxHorizontalDistance);
                Vector2 randomPosition = lastPlatformPosition + new Vector2(horizontalDistance, verticalDistance);
                if (randomPosition.y < -15)
                {
                    randomPosition.y = randomPosition.y + 4;
                }
                else if (randomPosition.y > 10)
                {
                    randomPosition.y = randomPosition.y - 4;
                }
                if (randomPosition.x >= 270)
                {
                    GameObject finalPlatform = (GameObject)Instantiate(platform, randomPosition, Quaternion.identity);
                    finalPlatform.GetComponent<PlatformFall>().SetAsFinalPlatform();
                    endOfGame = true;
                    return;
                }
                GameObject obj = (GameObject)Instantiate(platform, randomPosition, Quaternion.identity);
                obj.GetComponent<SpriteRenderer>().sprite = shelvesSprites[Random.Range(0, shelvesSprites.Count - 1)];
                currentPlatforms++;
                obj.transform.parent = platformContainer.transform;
                lastPlatformPosition = randomPosition;
                if (Mathf.Abs(verticalDistance) >= 2)
                {
                    CollectibleController._cc.CreateCollectible(randomPosition);
                }
            }
        }
    }

    public void RemovePlatform()
    {
        currentPlatforms--;
    }

    void ClearPlatforms()
    {
        foreach (Transform child in platformContainer.transform)
        {
            Debug.LogError("Destroying: " + child.gameObject.name);
            Destroy(child.gameObject);
            currentPlatforms--;
        }
    }
    public void QuestionTime(bool final = false)
    {
        foreach(Transform child in ButtonPanel.transform)
        {
            Destroy(child.gameObject);
        }

        Time.timeScale = 0;
        int questionIndex = Random.Range(0, questionList.Count - 1);
        questionText.text = questionList[questionIndex].QuestionText;

        foreach (string ans in questionList[questionIndex].correctAnswer)
        {
            Button button2 = Instantiate(genericButton);
            button2.transform.parent = ButtonPanel.transform;
            button2.GetComponentInChildren<Text>().text = ans;
            if (final == false)
            {
                button2.onClick.AddListener(() => answerYes());
            }
            else
            {
                button2.onClick.AddListener(() => LoadNextLevel());
            }
        }

        foreach (string ans in questionList[questionIndex].incorrectAnswer)
        {
            Button button2 = Instantiate(genericButton);
            button2.transform.parent = ButtonPanel.transform;
            button2.GetComponentInChildren<Text>().text = ans;
            button2.onClick.AddListener(() => answerNo());
        }

        QuestionPanel.SetActive(true);
    }

    public void answerYes()
    {
        Debug.Log("Answered Yes");
        QuestionPanel.SetActive(false);
        AnnouncementText.text = "Well done!";
        Time.timeScale = 1;
    }

    public void LoadNextLevel()
    {
        SaveLoad.savedCollectibles = CollectibleController._cc.GetCollectibleAmount();
        SaveLoad.Save();
        Debug.Log("Trying to load next level");
        SceneManager.LoadScene("Game1");
    }

    public void answerNo()
    {
        Debug.Log("Answered No");
        QuestionPanel.SetActive(false);
        AnnouncementText.text = "Oops";
        playerCharacterControllerScript.ReturnToStart();
        Time.timeScale = 1;
    }

    public void GameOver()
    {
        bool isTop = SaveLoad._sl.checkIfTopScore(CollectibleController._cc.GetCollectibleAmount());
        CollectibleController._cc.ReturnCollectiblesToZero();
        if (isTop == true)
        {
            Debug.Log("Top Score");
            AnnouncementText.text = "TOP SCORE!!!!";
            SaveLoad.Save();
        }
        else
        {
            Debug.Log("Not Top Score");
            AnnouncementText.text = "WELL DONE!!";
            SaveLoad.Save();
        }
        SaveLoad._sl.ViewTopScores();
    }

}
