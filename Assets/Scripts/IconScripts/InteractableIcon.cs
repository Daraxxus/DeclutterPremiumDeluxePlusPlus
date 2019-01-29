using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IconSnapToGrid))]
[RequireComponent(typeof(RandomizeSprite))]
public class InteractableIcon : MonoBehaviour {
    public bool IsClickedOn { get; private set; } = false;
    IconSnapToGrid iconSnapScript;
    RandomizeSprite randomizer;
    
    public bool HoverOverBin { get; set; } = false;

    [SerializeField] private AudioSource popIn;

	// Use this for initialization
	void Start () {
        randomizer = GetComponent<RandomizeSprite>();
        iconSnapScript = GetComponent<IconSnapToGrid>();
	}

    private void Update()
    {
        inCaseItemSpawnOnObject();
    }

    void inCaseItemSpawnOnObject ()
    {
        if (transform.position.z > 1)
        {
            iconSnapScript.grid.TryUnregisterIconOnPosition(iconSnapScript.CurrentSnapPosition);
            FindObjectOfType<IconSpawner>().GetComponent<ObjectPool>().ReturnGameObjectToPool(gameObject);
        }
    }

    public void OnClick ()
    {
        if (!IsClickedOn)
        {
            //picked up
            IsClickedOn = true;
            if (!iconSnapScript.grid.TryUnregisterIconOnPosition(iconSnapScript.CurrentSnapPosition))
            {
                throw new System.ArgumentException("This item is not registered");
            }
        }
        else
        {
            if (iconSnapScript.TrySnapIcon() || HoverOverBin)
            {
                IsClickedOn = false;
            }
        }
    }

    public void Reset()
    {
        if  (randomizer == null) randomizer = GetComponent<RandomizeSprite>();
        if (iconSnapScript == null) iconSnapScript = GetComponent<IconSnapToGrid>();
        IsClickedOn = false;
        HoverOverBin = false;
        randomizer.Randomize();
        StartCoroutine(GrowIn());
        popIn.Play();
    }

    IEnumerator GrowIn()
    {
        Vector3 originalScale = transform.localScale;
        transform.localScale = Vector3.zero;

        while (transform.localScale != originalScale)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, originalScale, 0.5f);
            yield return null;
        }
    }
}
