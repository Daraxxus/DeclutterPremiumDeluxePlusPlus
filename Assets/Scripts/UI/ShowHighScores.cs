using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowHighScores : MonoBehaviour, IButton {
    [SerializeField] private GameObject highScoreList;
    [SerializeField] private SaveData saveData;
    [SerializeField] private TextMeshProUGUI textToEditNames;
    [SerializeField] private TextMeshProUGUI textToEditScores;

    [SerializeField] private GameObject credit;
    [SerializeField] private GameObject infoBubble;
    [SerializeField] private GameObject startMenu;

    public void OnClick ()
    {
        if (!highScoreList.activeSelf && !credit.activeSelf && !startMenu.activeSelf && RecycleBin.GameOver)
        {
            UpdateScores();
            highScoreList.SetActive(true);
            infoBubble.SetActive(false);
        }
    }

    void UpdateScores()
    {
        List<SaveData.HighScore> listOfScores = saveData.Load();
        if (listOfScores != null)
        {
            textToEditNames.text = "";
            textToEditScores.text = "";
            listOfScores = listOfScores.QuickSortList();

            int counter = 0;
            for (int i = listOfScores.Count - 1; i >= 0; i--)
            {
                if (!(counter > 14))
                {
                    textToEditNames.text += (counter+1) + ": " + listOfScores[i].Name + "\n";
                    textToEditScores.text += "Score: " + listOfScores[i].Score + "\n";
                    counter++;
                }
                else return;
            }
        }
    }
}
