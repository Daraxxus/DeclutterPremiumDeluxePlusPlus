using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpdateScore : MonoBehaviour
{
    TextMeshProUGUI textToUpdate;

    private void Start()
    {
        textToUpdate = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        textToUpdate.text = "" + RecycleBin.score;
    }
}
