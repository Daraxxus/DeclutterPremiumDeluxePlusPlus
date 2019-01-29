using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowCredits : MonoBehaviour, IButton {
    [SerializeField] private GameObject credits;
    [SerializeField] private GameObject infoBubble;
    [SerializeField] private GameObject highScore;
    [SerializeField] private GameObject startMenu;

	public void OnClick()
    {
        if (!credits.activeSelf && !highScore.activeSelf && !startMenu.activeSelf && RecycleBin.GameOver)
        {
            credits.SetActive(true);
            infoBubble.SetActive(false);
        }
    }
}
