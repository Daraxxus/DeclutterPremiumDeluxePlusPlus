using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShutDown : MonoBehaviour, IButton {

	public void OnClick()
    {
        Application.Quit();
    }
}
