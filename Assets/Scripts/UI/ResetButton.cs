using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ResetButton : MonoBehaviour, IButton {
    [SerializeField] private TextMeshProUGUI textToUpdate;
    [SerializeField] private GameObject savePanel;
    [SerializeField] private GameObject resetPanel;

    public void Init()
    {
        textToUpdate.text = "You have cleared " + RecycleBin.score + " clutter.";
    }

    public void OnClick()
    {
        savePanel.SetActive(true);
        resetPanel.SetActive(false);
    }
}
