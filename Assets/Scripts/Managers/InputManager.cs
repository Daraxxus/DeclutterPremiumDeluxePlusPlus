using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {
    public Vector2 MousePos { get; private set; }
    public bool IsClicked { get; private set; }

    [SerializeField] private Camera mainCam;
	
    private void Start()
    {
        if (mainCam == null)
            mainCam = Camera.main;
    }
	// Update is called once per frame
	void Update () {
        UpdateInput();
	}

    void UpdateInput()
    {
        IsClicked = Input.GetMouseButtonDown(0);
        MousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
    }

}
