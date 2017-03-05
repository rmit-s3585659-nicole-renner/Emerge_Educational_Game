using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;

public class SaveLoad : MonoBehaviour {

    public static SaveLoad _sl;

    public GameObject TopScoresPanel;
    public static int[] topScores;
    public static int savedCollectibles;

    public Text ScoresTextObject;

    public void Awake()
    {
        if (_sl == null)
        {
            _sl = this;
            topScores = new int[5];
            Load();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public static void Save()
    {
        foreach (int score in topScores)
        {
            Debug.Log("Saved Score: " + score);
        }
        print(Application.persistentDataPath);

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/saveData.sav");
        bf.Serialize(file, new SaveData());
        file.Close();
    }

    public static void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/saveData.sav"))
        {
            Debug.Log("Save File exists");
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/saveData.sav", FileMode.Open);
            SaveData data = (SaveData)bf.Deserialize(file);
            file.Close();
            Debug.Log("Save File unserialized");
            for (int i = 0; i < data.savedtopScores.Length; i++)
            {
                topScores[i] = data.savedtopScores[i];
                Debug.Log("Loaded: " + topScores[i]);
            }
                savedCollectibles = data.savedCollectibles;
        }
    }

    public void ViewTopScores()
    {
        TopScoresPanel.SetActive(true);
        string scores = "";
        for (int i = 0; i < 5; i++)
        {
            scores += i + ".   " + topScores[i] + "\n";
        }
        ScoresTextObject.text = scores;
    }

    public void CloseTopScoreWindow()
    {
        TopScoresPanel.SetActive(false);
        SceneManager.LoadScene("Game1");
    }


    public bool checkIfTopScore(int score)
    {
        Debug.Log("Score: " + score + ": " + topScores[4]);
        if (score > topScores[4])
        {
            AddToTopScores(score);
            return true;
        }
        return false;
    }

    public void AddToTopScores(int score)
    {
        if (score > topScores[0])
        {
            topScores[4] = topScores[3];
            topScores[3] = topScores[2];
            topScores[2] = topScores[1];
            topScores[1] = topScores[0];
            topScores[0] = score;
        }
        else if (score > topScores[1])
        {
            topScores[4] = topScores[3];
            topScores[3] = topScores[2];
            topScores[2] = topScores[1];
            topScores[1] = score;
        }
        else if (score > topScores[2])
        {
            topScores[4] = topScores[3];
            topScores[3] = topScores[2];
            topScores[2] = score;
        }
        else if (score > topScores[3])
        {
            topScores[4] = topScores[2];
            topScores[3] = score;
        }
        else if (score > topScores[4])
        {
            topScores[4] = score;
        }
    }
}

[System.Serializable]
public class SaveData
{
    public int[] savedtopScores;
    public int savedCollectibles;

    public SaveData()
    {
        savedtopScores = new int[5];
        savedCollectibles = 0;
        savedtopScores = SaveLoad.topScores;
        savedCollectibles = SaveLoad.savedCollectibles;
    }
}
