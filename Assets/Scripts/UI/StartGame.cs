using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour, IButton {
    RecycleBin recycleBin;
    IconSpawner iconSpawner;
    Grid grid;
    [SerializeField] private GameObject highScoreList;
    [SerializeField] private AudioSource startUpSound;
    [SerializeField] private GameObject popUpHelpBubble;
    [SerializeField] private GameObject credits;
    [SerializeField] private GameObject startMenu;

    void Start()
    {
        recycleBin = FindObjectOfType<RecycleBin>();
        iconSpawner = FindObjectOfType<IconSpawner>();
        grid = FindObjectOfType<Grid>();
    }

    public void OnClick ()
    {
        recycleBin.Reset();
        iconSpawner.Reset();

        if (highScoreList.activeSelf)
        {
            highScoreList.SetActive(false);
        }
           
        if (!startUpSound.isPlaying)
        {
            startUpSound.Play();
        }

        popUpHelpBubble.SetActive(false);
        credits.SetActive(false);
        startMenu.SetActive(false);
    }
}
