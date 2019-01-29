using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpdateClock : MonoBehaviour {
    TextMeshProUGUI textToUpdate;

    private void Start()
    {
        textToUpdate = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        textToUpdate.text = System.DateTime.Now.ToString("HH:mm \n dd/MM/yyyy");
    }
}
