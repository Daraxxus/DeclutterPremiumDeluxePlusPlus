using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScoreX : XButton {
    [SerializeField] private GameObject infoBubble;

    public override void OnClick()
    {
        base.OnClick();
        infoBubble.SetActive(true);
    }
}
