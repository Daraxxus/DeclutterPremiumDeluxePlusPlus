using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveData : MonoBehaviour
{
    [System.Serializable]
    public struct HighScore
    {
        public string Name;
        public int Score;
    }

    List<HighScore> listOfHighScores = new List<HighScore>();

    const string folderName = "SaveData";
    const string fileExt = ".dat";

    private void Start()
    {
        string folderPath = Path.Combine(Application.persistentDataPath, folderName);

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
            string path = Path.Combine(Path.Combine(Application.persistentDataPath, folderName), "HighScores" + fileExt);
            FileStream file = File.Create(path);
            file.Close();
            Save("DaraxxusGames", 69);
        }
    }

    public List<HighScore> Load()
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();

        using (FileStream fileStream = File.Open(Path.Combine(Path.Combine(Application.persistentDataPath, folderName), "HighScores" + fileExt), FileMode.Open))
        {
            if (fileStream.Length > 0)
            {
                List<HighScore> highScoreList = (List<HighScore>)binaryFormatter.Deserialize(fileStream);
                fileStream.Close();
                return highScoreList;
            }
            else
            {
                fileStream.Close();
                return null;
            }
        }
    }

    public void Save(string name, int score)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        string path = Path.Combine(Path.Combine(Application.persistentDataPath, folderName), "HighScores" + fileExt);
        List<HighScore> data = Load();

        using (FileStream fileStream = File.Open(path, FileMode.OpenOrCreate))
        {
            HighScore highScore = new HighScore();
            highScore.Name = name;
            highScore.Score = score;

            if (data == null)
            {
                data = new List<HighScore>();
            }

            data.Add(highScore);

            binaryFormatter.Serialize(fileStream, data);
            fileStream.Close();
        }
    }

    private string[] getFiles()
    {
        string folderPath = Path.Combine(Application.persistentDataPath, folderName);
        return Directory.GetFiles(folderPath, fileExt);
    }
}
