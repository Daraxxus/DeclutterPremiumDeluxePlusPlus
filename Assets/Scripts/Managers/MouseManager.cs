using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour
{
    InteractableIcon currentInteractingIcon;
    InputManager inputManager;
    SpriteRenderer renderer;

    float slowDown = 0f;
    float timeTillUpdateSlowDown = 0.0f;

    [SerializeField] private AudioSource clickSound;

    // Use this for initialization
    void Start()
    {
        inputManager = FindObjectOfType<InputManager>();
        if (inputManager == null) throw new System.ArgumentNullException("Missing input manager, please add a input manager into the scene.");
        renderer = GetComponent<SpriteRenderer>();
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMousePosition();
        UpdateMouseInteraction();
    }

    public void SlowDown(float slowDownRate)
    {
        slowDown = slowDownRate;
    }

    void UpdateMousePosition()
    {
        if (slowDown == 0.0f)
        {
            transform.position = inputManager.MousePos;
        }
        else
        {
            timeTillUpdateSlowDown += Time.deltaTime;
            if (timeTillUpdateSlowDown > slowDown)
            {
                transform.position = inputManager.MousePos;
                timeTillUpdateSlowDown = 0.0f;
            }
        }
    }

    void UpdateMouseInteraction()
    {
        if (inputManager.IsClicked)
        {
            clickSound.Stop();
            clickSound.Play();

            if (currentInteractingIcon == null)
            {
                CheckForButtons();
            }

            if (RecycleBin.GameOver)
            {
                if (currentInteractingIcon != null)
                {
                    PlaceIcon();
                    renderer.enabled = true;
                }
            }
            else
            {
                if (currentInteractingIcon == null)
                {
                    currentInteractingIcon = CheckForIcon();
                    if (currentInteractingIcon != null) currentInteractingIcon.OnClick();
                }
                else
                {
                    PlaceIcon();
                    renderer.enabled = true;
                }
            }
        }

        if (currentInteractingIcon != null)
        {
            DragIcon();
            renderer.enabled = false;
        }
    }

    void CheckForButtons()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.zero);
        if (hit.collider != null)
        {
            IButton temp = hit.collider.GetComponent<IButton>();
            if (temp != null)
            {
                temp.OnClick();
            }
        }
    }

    InteractableIcon CheckForIcon()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.zero);
        if (hit.collider != null)
        {
            InteractableIcon temp = hit.collider.gameObject.GetComponent<InteractableIcon>();
            return temp;
        }

        return null;
    }

    void PlaceIcon()
    {
        currentInteractingIcon.OnClick();
        currentInteractingIcon = currentInteractingIcon.IsClickedOn ? currentInteractingIcon : null;
    }

    void DragIcon()
    {
        currentInteractingIcon.transform.position = transform.position;
    }
}
