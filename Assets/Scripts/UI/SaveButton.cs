using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SaveButton : MonoBehaviour, IButton {
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private RecycleBin recyclingBin;
    [SerializeField] private GameObject savePanel;

	public void OnClick()
    {
        recyclingBin.Save(inputField.text);
        savePanel.SetActive(false);
    }
}
