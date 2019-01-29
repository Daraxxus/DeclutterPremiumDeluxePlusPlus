using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clippy : MonoBehaviour {
    [SerializeField] private GameObject clippy;
    [SerializeField] private GameObject highScoreMenu;
    [SerializeField] private GameObject creditsMenu;
    [SerializeField] private GameObject startMenu;
    [SerializeField] private AudioSource appear;
    [SerializeField] private AudioSource dissappear;
    bool cooldown = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1) && (!highScoreMenu.activeSelf && !creditsMenu.activeSelf && !startMenu.activeSelf))
        {
            if (clippy.activeSelf)
            {
                if (!cooldown)
                {
                    dissappear.Play();
                    clippy.SetActive(false);
                    StartCoroutine(Cooldown());
                }
            }
            else
            {
                if (!cooldown)
                {
                    appear.Play();
                    clippy.SetActive(true);
                    StartCoroutine(Cooldown());
                }
            }
        }
    }

    IEnumerator Cooldown ()
    {
        cooldown = true;
        yield return new WaitForSeconds(1.0f);
        cooldown = false;
    }

    void OnDisable()
    {
        clippy.SetActive(false);
        StopAllCoroutines();
        cooldown = false;
    }
}
