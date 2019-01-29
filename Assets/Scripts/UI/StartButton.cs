using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButton : MonoBehaviour, IButton {
    [SerializeField] private GameObject startMenu;
    [SerializeField] private GameObject clippy;
    [SerializeField] private GameObject highScoreMenu;
    [SerializeField] private GameObject creditsMenu;

    [SerializeField] private GameObject resetPanel;
    [SerializeField] private GameObject savePanel;

    public void OnClick()
    {
        if (RecycleBin.GameOver && !savePanel.activeSelf && !resetPanel.activeSelf)
        {
            startMenu.SetActive(!startMenu.activeSelf);
            if (startMenu.activeSelf)
            {
                clippy.SetActive(false);
                highScoreMenu.SetActive(false);
                creditsMenu.SetActive(false);
            }
            else
            {
                clippy.SetActive(true);
            }
        }
    }
}
