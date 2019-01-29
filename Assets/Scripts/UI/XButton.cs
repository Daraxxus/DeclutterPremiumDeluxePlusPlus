using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XButton : MonoBehaviour, IButton {
    public virtual void OnClick()
    {
        gameObject.transform.parent.gameObject.SetActive(false);
    }
}
