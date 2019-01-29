using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BlockButtonPopUP : MonoBehaviour, IButton {
    [SerializeField] private TextMeshProUGUI textToEdit;
    [SerializeField] private PopUpVirus popUpVirus;
    [SerializeField] private GameObject buttonLess;
    [SerializeField] private GameObject mainWindow;

    public void OnClick()
    {
        StartCoroutine(BlockCountdown());
        buttonLess.SetActive(true);
    }

    IEnumerator BlockCountdown()
    {
        textToEdit.text = "Blocking...";
        yield return new WaitForSeconds(1.5f);
        popUpVirus.BlockPopUp();
        textToEdit.text = "Blocked!";
        yield return new WaitForSeconds(0.5f);
        Reset();
    }

    private void Reset()
    {
        textToEdit.text = "Pop Up Virus Detected!";
        buttonLess.SetActive(false);
        StopAllCoroutines();
        mainWindow.SetActive(false);
    }
}
